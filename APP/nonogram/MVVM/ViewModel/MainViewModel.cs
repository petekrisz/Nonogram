﻿using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.View;
using System.Diagnostics;
using System.Windows.Input;

namespace nonogram.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand<object> ImageListViewCommand { get; set; }
        public RelayCommand<object> BuyHelpViewCommand { get; set; }
        public RelayCommand<IMAGE> GameViewCommand { get; set; }




        public ImageListViewModel ImageListVM { get; set; }
        public BuyHelpViewModel BuyHelpVM { get; set; }
        public GameViewModel GameVM { get; set; }


        public SearchBarViewModel SearchBarVM { get; set; }
        public TitleBuyViewModel TitleBuyVM { get; set; }
        public TitleGameViewModel TitleGameVM { get; set; }



        private object _currentViewMain;

        public object CurrentViewMain
        {
            get { return _currentViewMain; }
            set
            {
                _currentViewMain = value;
                Debug.WriteLine($"CurrentViewMain set to: {_currentViewMain?.GetType().Name}");
                OnPropertyChanged(nameof(CurrentViewMain));
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
                    Debug.WriteLine($"CurrentViewTitle set to: {_currentViewTitle?.GetType().Name}");
                    OnPropertyChanged(nameof(CurrentViewTitle));
                }
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


        public MainViewModel()
        {
            ImageListViewModel ImageListVM = new ImageListViewModel();
            Debug.WriteLine($"Binding ImagesLeft to {ImageListVM.ImagesLeft.Count} items");

            SearchBarVM = new SearchBarViewModel();
            TitleBuyVM = new TitleBuyViewModel();

            Debug.WriteLine("Subscribing SearchBarVM.SearchTermUpdated to ImageListVM.FilterImages.");
            SearchBarVM.SearchTermUpdated += ImageListVM.FilterImages;
            Debug.WriteLine("Subscription completed.");

            CurrentViewMain = ImageListVM;
            CurrentViewTitle = SearchBarVM;

            Debug.WriteLine($"CurrentViewTitle assigned: {CurrentViewTitle?.GetType().Name}");

            ImageListViewCommand = new RelayCommand<object>(_ =>
            {
                CurrentViewMain = ImageListVM;
                CurrentViewTitle = SearchBarVM;
                Debug.WriteLine("ImageListViewCommand executed.");
            });
            BuyHelpViewCommand = new RelayCommand<object>(_ =>
            {
                CurrentViewMain = BuyHelpVM;
                CurrentViewTitle = TitleBuyVM;
                Debug.WriteLine("BuyHelpViewCommand executed.");
            });
            GameViewCommand = new RelayCommand<IMAGE>(OpenGameView);

            string email = "somethingratherdifferent@something.else"; //That should be acquired from DB
            string hash = HashHelper.ComputeSha256Hash(email);
            AvatarUrl = "https://www.gravatar.com/avatar/" + hash + "?s=140&d=identicon";
        }

        public void OpenGameView(IMAGE selectedImage)
        {
            GameVM = new GameViewModel(selectedImage);
            TitleGameVM = new TitleGameViewModel(selectedImage.Title);
            var gameView = new GameView();
            gameView.SetViewModel(GameVM);
            CurrentViewMain = gameView;
            CurrentViewTitle = TitleGameVM;
        }




    }
}
