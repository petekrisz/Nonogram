using nonogram.Common;

namespace nonogram.MVVM.ViewModel
{
    internal class SearchBarViewModel : ObservableObject
    {
        private string _searchBar;
        public string SearchBar
        {
            get { return _searchBar; }
            set
            {
                _searchBar = value;
                OnPropertyChanged();
            }
        }


    }
}
