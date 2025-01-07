using Mysqlx.Crud;
using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace nonogram.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public string UserName { get; set; }

        public RelayCommand<object> ImageListViewCommand { get; set; }
        public RelayCommand<object> BuyHelpViewCommand { get; set; }
        public RelayCommand<IMAGE> GameViewCommand { get; set; }
        public RelayCommand<object> UserMenuViewCommand { get; set; }


        public HelpTableViewModel HelpTableVM { get; set; }
        public ImageListViewModel ImageListVM { get; set; }
        public BuyHelpViewModel BuyHelpVM { get; set; }
        public GameViewModel GameVM { get; set; }
        public UserMenuViewModel UserMenuVM { get; set; }


        public SearchBarViewModel SearchBarVM { get; set; }
        public TitleBuyViewModel TitleBuyVM { get; set; }
        public TitleGameViewModel TitleGameVM { get; set; }
        public TitleUserMenuViewModel TitleUserMenuVM { get; set; }

        private bool _isHelpEnabled;
        public bool IsHelpEnabled
        {
            get => _isHelpEnabled;
            set => SetProperty(ref _isHelpEnabled, value);
        }

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
                        // Call method to save game state before changing view
                        SaveGameState();
                    }
                    _currentViewMain = value;
                    //Debug.WriteLine($"CurrentViewMain set to: {_currentViewMain?.GetType().Name}");
                    OnPropertyChanged(nameof(CurrentViewMain));

                    IsHelpEnabled = _currentViewMain is GameView;
                }
            }
        }

        private void SaveGameState()
        {
            if (GameVM != null)
            {
                GameVM.SaveGameState();
                RefreshImageListView();
            }
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
                    //Debug.WriteLine($"CurrentViewTitle set to: {_currentViewTitle?.GetType().Name}");
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
                //Debug.WriteLine($"CurrentViewHelp set to: {_currentViewHelp?.GetType().Name}");
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

        private USER _user;
        public USER User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }


        public MainViewModel(string userName)
        {
            //Debug.WriteLine($"Initializing MainViewModel with username: {userName}");

            LoadUserData(userName);

            UserName = userName;

            ImageListVM = new ImageListViewModel(userName);
            //Debug.WriteLine($"MainViewModel initialized: ImageListVM instance: {ImageListVM.GetHashCode()}");
            SearchBarVM = new SearchBarViewModel();

            TitleBuyVM = new TitleBuyViewModel();
            BuyHelpVM = new BuyHelpViewModel(userName);
            HelpTableVM = new HelpTableViewModel(userName);
            //Debug.WriteLine($"MainViewModel initialized: HelpTableVM instance: {HelpTableVM.GetHashCode()}");

            UserMenuVM = new UserMenuViewModel(userName);
            TitleUserMenuVM = new TitleUserMenuViewModel(userName);

            //Debug.WriteLine("Subscribing SearchBarVM.SearchTermUpdated to ImageListVM.FilterImages.");
            SearchBarVM.SearchTermUpdated += ImageListVM.FilterImages;
            //Debug.WriteLine("Subscription completed.");

            CurrentViewMain = ImageListVM;
            CurrentViewTitle = SearchBarVM;
            CurrentViewHelp = null;

            //Debug.WriteLine($"CurrentViewTitle assigned: {CurrentViewTitle?.GetType().Name}");

            UserMenuViewCommand = new RelayCommand<object>(_ =>
            {
                CurrentViewMain = UserMenuVM;
                CurrentViewTitle = TitleUserMenuVM;
                CurrentViewHelp = null;
                //Debug.WriteLine("UserMenuViewCommand executed.");
            });

            ImageListViewCommand = new RelayCommand<object>(_ =>
            {
                CurrentViewMain = ImageListVM;
                CurrentViewTitle = SearchBarVM;
                CurrentViewHelp = null;
                //Debug.WriteLine($"ImageListViewCommand executed. Instance: {ImageListVM.GetHashCode()}");

            });
            BuyHelpViewCommand = new RelayCommand<object>(_ =>
            {
                CurrentViewMain = BuyHelpVM;
                CurrentViewTitle = TitleBuyVM;
                CurrentViewHelp = HelpTableVM;
                //Debug.WriteLine("BuyHelpViewCommand executed.");
            });
            GameViewCommand = new RelayCommand<IMAGE>(OpenGameView);

            string hash = HashHelper.ComputeSha256Hash(User.Email.ToString());
            AvatarUrl = "https://www.gravatar.com/avatar/" + hash + "?s=140&d=identicon";
            
        }

        public void LoadUserData(string userName)
        {
            var dbManager = new DbManager();
            string query = "SELECT * FROM USER WHERE UserName = @UserName";
            var parameters = new Dictionary<string, object> { { "@UserName", userName } };
            var userTable = dbManager.ExecuteQuery(query, parameters);

            if (userTable.Rows.Count > 0)
            {
                var userRow = userTable.Rows[0];
                User = new USER
                {
                    UserName = userRow["UserName"].ToString(),
                    Score = int.Parse(userRow["Score"].ToString()),
                    Tokens = int.Parse(userRow["Tokens"].ToString()),
                    Email = userRow["Email"].ToString()
                };
            }
        }

        public void OpenGameView(IMAGE selectedImage)
        {
            GameVM = new GameViewModel(selectedImage, UserName, Application.Current.MainWindow);
            TitleGameVM = new TitleGameViewModel(selectedImage.Title);
            var gameView = new GameView();
            gameView.SetViewModel(GameVM);
            CurrentViewMain = gameView;
            CurrentViewTitle = TitleGameVM;
            CurrentViewHelp = HelpTableVM;
            //Debug.WriteLine($"HelpTableVM instance in OpenGameView: {HelpTableVM.GetHashCode()}");

            GameVM.GameWon += (sender, e) =>
            {
                var wonGameWindow = new WonGameWindow(selectedImage.IMAGEId, UserName)
                {
                   Owner = Application.Current.MainWindow
                };
                wonGameWindow.Show();
            };
        }

        public void RefreshImageListView()
        {
            ImageListVM.FilterImages(string.Empty);
        }




    }
}
