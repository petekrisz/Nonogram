using nonogram.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nonogram.MVVM.ViewModel
{
    public class TitleUserMenuViewModel : ObservableObject
    {
        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public TitleUserMenuViewModel(string userName)
        {
            Username = userName;
        }
    }
}
