using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.Model;
using nonogram.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nonogram.MVVM.ViewModel
{
    public class HelpTableViewModel : ObservableObject
    {
        public string Username { get; set; } = "netuddki"; // Hardcoded for now

        private ObservableCollection<HelpOption> _helpOptions;
        public ObservableCollection<HelpOption> HelpOptions
        {
            get => _helpOptions;
            set => SetProperty(ref _helpOptions, value);
        }

        public ICommand HelpOptionCommand { get; }
        public ICommand CheckBoxCheckedCommand { get; }

        public HelpTableViewModel()
        {
            Debug.WriteLine($"HelpTableViewModel instance created (HelpTableView.cs): {GetHashCode()}");

            CheckBoxCheckedCommand = new RelayCommand<HelpOption>(ExecuteCheckBoxCheckedCommand); CheckBoxCheckedCommand = new RelayCommand<HelpOption>(ExecuteCheckBoxCheckedCommand);
            LoadHelpOptions();
        }

        private async void ExecuteCheckBoxCheckedCommand(HelpOption helpOption)
        {
            bool performed = false;

            if (helpOption.IsChecked)
            {
                performed = await ExecuteHelpOptionCommand(helpOption.TypeOfHelp, helpOption);
            }

            if (performed)
            {
                helpOption.IsChecked = false;
            }
        }
        private async Task<bool> ExecuteHelpOptionCommand(string typeOfHelp, HelpOption helpOption)
        {
            Debug.WriteLine($"Executing HelpOptionCommand: {typeOfHelp}");

            bool performed = false;
            int initialValue = int.Parse(helpOption.Value);

            // Set the TypeOfHelp property in GameView.xaml.cs
            if (System.Windows.Application.Current.MainWindow is MainWindow mainWindow &&
                mainWindow.DataContext is MainViewModel mainViewModel &&
                mainViewModel.CurrentViewMain is GameView gameView)
            {
                if (typeOfHelp == "H1" || typeOfHelp == "H13" || typeOfHelp == "L1" || typeOfHelp == "L3")
                {
                    gameView.SelectedHelpOption = typeOfHelp;

                    // Wait until the help operation is performed
                    while (int.Parse(helpOption.Value) == initialValue && helpOption.IsChecked)
                    {
                        await Task.Delay(100); // Check every 100ms
                    }
                    if (int.Parse(helpOption.Value) < initialValue)
                    {
                        performed = true;
                    }
                    else
                    {
                        gameView.SelectedHelpOption = null;
                        performed = false;
                    }
                }
                else if (gameView.DataContext is GameViewModel viewModel)
                {
                    performed = viewModel.ExecuteHelpOption(0, 0, typeOfHelp);
                    if (performed)
                    {
                        DecreaseHelpOptionValue(typeOfHelp);
                    }
                    else if (typeOfHelp == "Check3H")
                    {
                        performed = true;
                    }
                }
            }
            return performed;
        }

        private void LoadHelpOptions()
        {
            var dbManager = new DbManager();
            string helpQuery = "SELECT TypeOfHelp, HelpLogoG, HelpLogoL FROM HELP";
            DataTable helpTable = dbManager.ExecuteQuery(helpQuery);

            string userHelpQuery = "SELECT * FROM USERHELP WHERE UserName = @UserName";
            var parameters = new Dictionary<string, object> { { "@UserName", Username } };
            DataTable userHelpTable = dbManager.ExecuteQuery(userHelpQuery, parameters);

            HelpOptions = new ObservableCollection<HelpOption>();
            foreach (DataRow helpRow in helpTable.Rows)
            {
                string typeOfHelp = helpRow["TypeOfHelp"].ToString();
                string value = userHelpTable.Rows[0][typeOfHelp].ToString();

                HelpOptions.Add(new HelpOption
                {
                    TypeOfHelp = typeOfHelp,
                    HelpLogoG = $"/Images/{helpRow["HelpLogoG"]}",
                    HelpLogoL = $"/Images/{helpRow["HelpLogoL"]}",
                    Value = value
                });
            }
        }

        public void DecreaseHelpOptionValue(string typeOfHelp)
        {
            Debug.WriteLine($"Decreasing {typeOfHelp}");
            var helpOption = HelpOptions.FirstOrDefault(h => h.TypeOfHelp == typeOfHelp);
            if (helpOption != null && int.TryParse(helpOption.Value, out int currentValue) && currentValue > 0)
            {

                Debug.WriteLine($"Before Update: {helpOption.TypeOfHelp} -> {helpOption.Value} (Instance: {helpOption.GetHashCode()})");
                helpOption.Value = (currentValue - 1).ToString();
                Debug.WriteLine($"After Update: {helpOption.TypeOfHelp} -> {helpOption.Value} (Instance: {helpOption.GetHashCode()})");

                // Update the USERHELP table
                var dbManager = new DbManager();
                string updateQuery = $"UPDATE USERHELP SET {typeOfHelp} = {typeOfHelp} - 1 WHERE UserName = @UserName";
                var parameters = new Dictionary<string, object> { { "@UserName", Username } };
                dbManager.ExecuteNonQuery(updateQuery, parameters);
            }
        }

        //public void RefreshHelpOptions()
        //{
        //    var temp = HelpOptions;
        //    HelpOptions = null; // Break the binding temporarily
        //    HelpOptions = temp; // Restore the binding
        //}
    }
}
