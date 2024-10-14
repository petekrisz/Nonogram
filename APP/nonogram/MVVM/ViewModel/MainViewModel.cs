using nonogram.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nonogram.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        public ImageListViewModel ImageListVM { get; set; }
        public SearchBarViewModel SearchBarVM { get; set; }


        private object _currentViewMain;

        public object CurrentViewMain 
        {
            get {  return _currentViewMain; } 
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


        public MainViewModel()
        {
            ImageListVM = new ImageListViewModel();
            CurrentViewMain = ImageListVM; // For now it is set to ImageListView, because this is the only that is ready and it should be also the first View after registration 

            //In the final version it should be dependent on whether the player has an unfinished image or not. In the first case the current view should be set to the unfinished image and in the second it should be the ImageListVM

            SearchBarVM = new SearchBarViewModel();
            CurrentViewTitle = SearchBarVM; // For now it is set to SearchBarView, because this is the only that is ready and it should be also the first View after registration

            //In the final version it should be dependent on whether the player has an unfinished image or not. In the first case the current view should be set to the title of unfinished image and in the second it should be the SearchBarVM

            string email = "something@something.else"; //That shoud be acquired from DB
            string hash = HashHelper.ComputeSha256Hash(email);
            AvatarUrl = "https://www.gravatar.com/avatar/" + hash + "?s=140&d=identicon";



        }
    }
}
