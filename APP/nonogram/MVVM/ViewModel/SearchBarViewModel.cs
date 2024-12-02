using nonogram.Common;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace nonogram.MVVM.ViewModel
{
    public class SearchBarViewModel : ObservableObject
    {
        public event Action<string> SearchTermUpdated;

        private string _searchBar;
        public string SearchBar
        {
            get => _searchBar;
            set
            {
                if (_searchBar != value)
                {
                    _searchBar = value;
                    Debug.WriteLine($"SearchBar property updated: {_searchBar}");
                    OnPropertyChanged(nameof(SearchBar));
                }
            }
        }

        public ICommand SearchCommand { get; }

        public SearchBarViewModel()
        {
            SearchCommand = new RelayCommand<object>(_ =>
            {
                Debug.WriteLine($"SearchCommand executed with SearchBar: {_searchBar}");
                SearchTermUpdated?.Invoke(_searchBar); // Trigger filtering explicitly
            });
        }
    }

}
