using nonogram.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
