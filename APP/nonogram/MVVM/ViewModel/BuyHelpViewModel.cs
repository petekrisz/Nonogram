using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using nonogram.Common;
using nonogram.MVVM.Model;

namespace nonogram.MVVM.ViewModel
{
    public class BuyHelpViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<HelpOption> HelpOptions { get; set; }

        public BuyHelpViewModel()
        {
            HelpOptions = new ObservableCollection<HelpOption>
            {
                new HelpOption { TypeOfHelp = "H1", Value = "0", TokenValue = 5 },
                new HelpOption { TypeOfHelp = "H3", Value = "0", TokenValue = 5 },
                new HelpOption { TypeOfHelp = "H8", Value = "0", TokenValue = 5 },
                new HelpOption { TypeOfHelp = "H13", Value = "0", TokenValue = 5 },
                new HelpOption { TypeOfHelp = "L1", Value = "0", TokenValue = 5 },
                new HelpOption { TypeOfHelp = "L3", Value = "0", TokenValue = 5 },
                new HelpOption { TypeOfHelp = "Check3H", Value = "0", TokenValue = 5 },
                new HelpOption { TypeOfHelp = "Erase", Value = "0", TokenValue = 5 }
            };
        }

        private ICommand _increaseCommand;
        public ICommand IncreaseCommand
        {
            get
            {
                return _increaseCommand ?? (_increaseCommand = new RelayCommand<object>(param => IncreaseValue(param), param => param != null));
            }
        }

        private ICommand _decreaseCommand;
        public ICommand DecreaseCommand
        {
            get
            {
                return _decreaseCommand ?? (_decreaseCommand = new RelayCommand<object>(param => DecreaseValue(param), param => param != null));
            }
        }

        private void IncreaseValue(object param)
        {
            if (param is HelpOption helpOption)
            {
                if (int.TryParse(helpOption.Value, out int currentValue))
                {
                    helpOption.Value = (currentValue + 1).ToString();
                    UpdateUserTokens(helpOption.TokenValue);
                }
            }
        }

        private void DecreaseValue(object param)
        {
            if (param is HelpOption helpOption)
            {
                if (int.TryParse(helpOption.Value, out int currentValue) && currentValue > 0)
                {
                    helpOption.Value = (currentValue - 1).ToString();
                    UpdateUserTokens(-helpOption.TokenValue);
                }
            }
        }

        private void UpdateUserTokens(int tokenChange)
        {
            var filePath = "USER.csv";

            if (!File.Exists(filePath))
            {
                return;
            }

            var lines = File.ReadAllLines(filePath).ToList();
            for (int i = 1; i < lines.Count; i++)
            {
                var parts = lines[i].Split(';');
                if (parts.Length >= 8)
                {
                    int currentTokens = int.Parse(parts[7]);
                    currentTokens -= tokenChange;
                    parts[7] = currentTokens.ToString();
                    lines[i] = string.Join(";", parts);
                    break;
                }
            }

            File.WriteAllLines(filePath, lines);

            // Update the tokens in the MainWindow
            if (System.Windows.Application.Current.MainWindow is MainWindow mainWindow &&
                mainWindow.DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.UpdateUserTokens();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
