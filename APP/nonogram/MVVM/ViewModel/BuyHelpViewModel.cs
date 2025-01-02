using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.Model;

namespace nonogram.MVVM.ViewModel
{
    public class BuyHelpViewModel : ObservableObject
    {
        public ObservableCollection<HelpOption> HelpOptions { get; set; }
        //private UserModel _user;

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public BuyHelpViewModel(string username)
        {
            Username = username;
            LoadHelpOptions();
        }

        private void LoadHelpOptions()
        {
            var dbManager = new DbManager();
            string query = "SELECT TypeOfHelp, Price, HelpLogoL FROM HELP";
            DataTable helpTable = dbManager.ExecuteQuery(query);

            HelpOptions = new ObservableCollection<HelpOption>();
            foreach (DataRow row in helpTable.Rows)
            {
                var helpOption = new HelpOption
                {
                    TypeOfHelp = row["TypeOfHelp"].ToString(),
                    Price = int.Parse(row["Price"].ToString()),
                    HelpLogoL = $"/Images/{row["HelpLogoL"]}",
                    Amount = 0
                };
                HelpOptions.Add(helpOption);
            }
        }

        private ICommand _increaseCommand;
        public ICommand IncreaseCommand
        {
            get
            {
                if (_increaseCommand == null)
                {
                    _increaseCommand = new RelayCommand<HelpOption>(IncreaseValue);
                }
                return _increaseCommand;
            }
        }

        private ICommand _decreaseCommand;
        public ICommand DecreaseCommand
        {
            get
            {
                if (_decreaseCommand == null)
                {
                    _decreaseCommand = new RelayCommand<HelpOption>(DecreaseValue);
                }
                return _decreaseCommand;
            }
        }

        private ICommand _buyCommand;
        public ICommand BuyCommand
        {
            get
            {
                if (_buyCommand == null)
                {
                    _buyCommand = new RelayCommand<HelpOption>(BuyHelp);
                }
                return _buyCommand;
            }
        }

        private void IncreaseValue(HelpOption helpOption)
        {
            helpOption.Amount++;
            OnPropertyChanged(nameof(HelpOptions));
        }

        private void DecreaseValue(HelpOption helpOption)
        {
            if (helpOption.Amount > 0)
            {
                helpOption.Amount--;
                OnPropertyChanged(nameof(HelpOptions));
            }
        }

        private void BuyHelp(HelpOption helpOption)
        {
            if (helpOption.Amount <= 0)
            {
                MessageBox.Show("Please select a valid amount to buy.", "Invalid Amount", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int totalCost = helpOption.Amount * helpOption.Price;

            var dbManager = new DbManager();
            // Update USER table
            string userQuery = "SELECT Tokens FROM USER WHERE UserName = @UserName";
            var userParameters = new Dictionary<string, object> { { "@UserName", _username } };
            DataTable userTable = dbManager.ExecuteQuery(userQuery, userParameters);

            if (userTable.Rows.Count > 0)
            {
                DataRow userRow = userTable.Rows[0];
                int currentTokens = int.Parse(userRow["Tokens"].ToString());

                if (currentTokens >= totalCost)
                {
                    int newTokens = currentTokens - totalCost;
                    string updateUserQuery = "UPDATE USER SET Tokens = @Tokens WHERE UserName = @UserName";
                    var updateUserParameters = new Dictionary<string, object>
                    {
                        { "@Tokens", newTokens },
                        { "@UserName", _username }
                    };
                    dbManager.ExecuteNonQuery(updateUserQuery, updateUserParameters);

                    // Update USERHELP table
                    string updateHelpQuery = $"UPDATE USERHELP SET {helpOption.TypeOfHelp} = {helpOption.TypeOfHelp} + @Amount WHERE UserName = @UserName";
                    var updateHelpParameters = new Dictionary<string, object>
                    {
                        { "@Amount", helpOption.Amount },
                        { "@UserName", _username }
                    };
                    dbManager.ExecuteNonQuery(updateHelpQuery, updateHelpParameters);


                    // Refresh HelpTableViewModel
                    if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.DataContext is MainViewModel mainViewModel)
                    {
                        mainViewModel.HelpTableVM.LoadHelpOptions();
                        mainViewModel.LoadUserData(_username);
                    }


                    // Reset amount
                    helpOption.Amount = 0;
                    OnPropertyChanged(nameof(HelpOptions));


                }
                else
                {
                    MessageBox.Show("Not enough tokens to complete the purchase.", "Insufficient Tokens", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        protected new void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
