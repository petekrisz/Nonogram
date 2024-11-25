using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.View;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
                OnPropertyChanged();
            }
        }

        private object _currentViewTitle;
        public object CurrentViewTitle
        {
            get { return _currentViewTitle; }
            set
            {
                _currentViewTitle = value;
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

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private int _userPoints;
        public int UserPoints
        {
            get { return _userPoints; }
            set
            {
                _userPoints = value;
                OnPropertyChanged();
            }
        }

        private int _userTokens;
        public int UserTokens
        {
            get { return _userTokens; }
            set
            {
                _userTokens = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ImageListVM = new ImageListViewModel();
            BuyHelpVM = new BuyHelpViewModel();

            CurrentViewMain = ImageListVM; // For now it is set to ImageListView, because this is the only that is ready and it should be also the first View after registration 

            //In the final version it should be dependent on whether the player has an unfinished image or not. In the first case the current view should be set to the unfinished image and in the second it should be the ImageListVM

        }
    }
}
