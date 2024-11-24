using nonogram.Common;

namespace nonogram.MVVM.ViewModel
{
    internal class TitleGameViewModel : ObservableObject
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public TitleGameViewModel(string title)
        {
            Title = title;
        }
    }
}

