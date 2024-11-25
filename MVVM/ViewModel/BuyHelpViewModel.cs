using nonogram.Common;
using System.ComponentModel;
using System.Windows.Input;

namespace nonogram.MVVM.ViewModel
{
    internal class BuyHelpViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _megmutat3HibasCount;
        public int Megmutat3HibasCount
        {
            get => _megmutat3HibasCount;
            set
            {
                _megmutat3HibasCount = value;
                OnPropertyChanged(nameof(Megmutat3HibasCount));
            }
        }

        private int _megmutat2HibasCount;
        public int Megmutat2HibasCount
        {
            get => _megmutat2HibasCount;
            set
            {
                _megmutat2HibasCount = value;
                OnPropertyChanged(nameof(Megmutat2HibasCount));
            }
        }

        private int _randomViewCount;
        public int RandomViewCount
        {
            get => _randomViewCount;
            set
            {
                _randomViewCount = value;
                OnPropertyChanged(nameof(RandomViewCount));
            }
        }

        private int _viewRowCount;
        public int ViewRowCount
        {
            get => _viewRowCount;
            set
            {
                _viewRowCount = value;
                OnPropertyChanged(nameof(ViewRowCount));
            }
        }

        private int _viewColumnCount;
        public int ViewColumnCount
        {
            get => _viewColumnCount;
            set
            {
                _viewColumnCount = value;
                OnPropertyChanged(nameof(ViewColumnCount));
            }
        }

        public ICommand IncreaseCommand { get; }
        public ICommand DecreaseCommand { get; }

        public BuyHelpViewModel()
        {
            IncreaseCommand = new RelayCommand<string>(Increase);
            DecreaseCommand = new RelayCommand<string>(Decrease);
        }

        private void Increase(string parameter)
        {
            switch (parameter)
            {
                case "Megmutat3Hibas":
                    Megmutat3HibasCount++;
                    break;
                case "Megmutat2Hibas":
                    Megmutat2HibasCount++;
                    break;
                case "RandomView":
                    RandomViewCount++;
                    break;
                case "ViewRow":
                    ViewRowCount++;
                    break;
                case "ViewColumn":
                    ViewColumnCount++;
                    break;
            }
        }

        private void Decrease(string parameter)
        {
            switch (parameter)
            {
                case "Megmutat3Hibas":
                    if (Megmutat3HibasCount > 0) Megmutat3HibasCount--;
                    break;
                case "Megmutat2Hibas":
                    if (Megmutat2HibasCount > 0) Megmutat2HibasCount--;
                    break;
                case "RandomView":
                    if (RandomViewCount > 0) RandomViewCount--;
                    break;
                case "ViewRow":
                    if (ViewRowCount > 0) ViewRowCount--;
                    break;
                case "ViewColumn":
                    if (ViewColumnCount > 0) ViewColumnCount--;
                    break;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
