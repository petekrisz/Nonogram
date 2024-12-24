using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace nonogram.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand<object> ImageListViewCommand { get; set; }
        public RelayCommand<object> BuyHelpViewCommand { get; set; }
        public RelayCommand<IMAGE> GameViewCommand { get; set; }
        public RelayCommand<object> OpenUserMenuViewCommand { get; set; } // Javítva: biztosítva van a helyes típusargumentum

        public HelpTableViewModel HelpTableVM { get; set; }
        public DummyViewModel DummyVM { get; set; }
        public ImageListViewModel ImageListVM { get; set; }
        public BuyHelpViewModel BuyHelpVM { get; set; }
        public GameViewModel GameVM { get; set; }
        public UserMenuViewModel UserMenuVM { get; set; }

        public SearchBarViewModel SearchBarVM { get; set; }
        public TitleBuyViewModel TitleBuyVM { get; set; }
        public TitleGameViewModel TitleGameVM { get; set; }

        private object _currentViewMain;
        public object CurrentViewMain
        {
            get { return _currentViewMain; }
            set
            {
                if (_currentViewMain != value)
                {
                    if (_currentViewMain is GameView)
                    {
                        SaveGameState();
                    }
                    _currentViewMain = value;
                    Debug.WriteLine($"CurrentViewMain set to: {_currentViewMain?.GetType().Name}");
                    OnPropertyChanged(nameof(CurrentViewMain));
                }
            }
        }

        private void SaveGameState()
        {
            GameVM?.SaveGameState();
        }

        private object _currentViewTitle;
        public object CurrentViewTitle
        {
            get { return _currentViewTitle; }
            set
            {
                if (_currentViewTitle != value)
                {
                    _currentViewTitle = value;
                    Debug.WriteLine($"CurrentViewTitle set to: {_currentViewTitle?.GetType().Name}");
                    OnPropertyChanged(nameof(CurrentViewTitle));
                }
            }
        }

        private object _currentViewHelp;
        public object CurrentViewHelp
        {
            get => _currentViewHelp;
            set
            {
                _currentViewHelp = value;
                Debug.WriteLine($"CurrentViewHelp set to: {_currentViewHelp?.GetType().Name}");
                OnPropertyChanged();
            }
        }

        private string _avatarUrl;
        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set
            {
                _avatarUrl = value;
                OnPropertyChanged();
            }
        }

        private double _zoomLevel = 1.0;
        public double ZoomLevel
        {
            get => _zoomLevel;
            set
            {
                _zoomLevel = value;
                OnPropertyChanged();
            }
        }

        private string _userFullName;
        public string UserFullName
        {
            get => _userFullName;
            set
            {
                _userFullName = value;
                OnPropertyChanged(nameof(UserFullName));
            }
        }

        private int _userPoints;
        public int UserPoints
        {
            get => _userPoints;
            set
            {
                _userPoints = value;
                OnPropertyChanged(nameof(UserPoints));
            }
        }

        private int _userTokens;
        public int UserTokens
        {
            get => _userTokens;
            set
            {
                _userTokens = value;
                OnPropertyChanged(nameof(UserTokens));
            }
        }

        public MainViewModel()
        {
            ImageListVM = new ImageListViewModel();
            Debug.WriteLine($"MainViewModel initialized: ImageListVM instance: {ImageListVM.GetHashCode()}");
            SearchBarVM = new SearchBarViewModel();

            TitleBuyVM = new TitleBuyViewModel();
            BuyHelpVM = new BuyHelpViewModel();
            HelpTableVM = new HelpTableViewModel();
            UserMenuVM = new UserMenuViewModel();
            Debug.WriteLine($"MainViewModel initialized: HelpTableVM instance: {HelpTableVM.GetHashCode()}");

            DummyVM = new DummyViewModel();

            SearchBarVM.SearchTermUpdated += ImageListVM.FilterImages;

            CurrentViewMain = ImageListVM;
            CurrentViewTitle = SearchBarVM;
            CurrentViewHelp = null;

            Debug.WriteLine($"CurrentViewTitle assigned: {CurrentViewTitle?.GetType().Name}");

            ImageListViewCommand = new RelayCommand<object>(_ =>
            {
                CurrentViewMain = ImageListVM;
                CurrentViewTitle = SearchBarVM;
                CurrentViewHelp = null;
                Debug.WriteLine($"ImageListViewCommand executed. Instance: {ImageListVM.GetHashCode()}");
            });
            BuyHelpViewCommand = new RelayCommand<object>(_ =>
            {
                CurrentViewMain = BuyHelpVM;
                CurrentViewTitle = TitleBuyVM;
                CurrentViewHelp = null;
                Debug.WriteLine("BuyHelpViewCommand executed.");
            });
            GameViewCommand = new RelayCommand<IMAGE>(OpenGameView);
            OpenUserMenuViewCommand = new RelayCommand<object>(_ => OpenUserMenuView()); // Javítva: helyes típusargumentum

            string email = "somethingratherdifferent@something.else"; // This should be acquired from DB
            string hash = HashHelper.ComputeSha256Hash(email);
            AvatarUrl = "https://www.gravatar.com/avatar/" + hash + "?s=140&d=identicon";

            LoadUserDataFromCsv(email); // Load user data from CSV
        }

        public void UpdateUserTokens()
        {
            string email = "somethingratherdifferent@something.else"; // Ezt a megfelelő helyről kell beszerezni
            LoadUserDataFromCsv(email);
        }

        private void LoadUserDataFromCsv(string email)
        {
            var filePath = "USER.csv"; // Helyes elérési útvonal

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Nem található a következő fájl: „{filePath}”", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(';');
                if (parts.Length >= 5 && parts[4] == email)
                {
                    UserFullName = $"{parts[2]} {parts[3]}";
                    UserPoints = int.Parse(parts[6]);
                    UserTokens = int.Parse(parts[7]);
                    break;
                }
            }
        }

        public void OpenGameView(IMAGE selectedImage)
        {
            GameVM = new GameViewModel(selectedImage);
            TitleGameVM = new TitleGameViewModel(selectedImage.Title);
            var gameView = new GameView();
            gameView.SetViewModel(GameVM);
            CurrentViewMain = gameView;
            CurrentViewTitle = TitleGameVM;
            CurrentViewHelp = HelpTableVM;
            Debug.WriteLine($"HelpTableVM instance in OpenGameView: {HelpTableVM.GetHashCode()}");
        }

        public void OpenUserMenuView()
        {
            CurrentViewMain = UserMenuVM;
            CurrentViewTitle = null;
            CurrentViewHelp = null;
            Debug.WriteLine("UserMenuView opened.");
        }
    }
}
