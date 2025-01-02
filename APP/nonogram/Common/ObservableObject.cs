using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace nonogram.Common
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            //Debug.WriteLine($"Property {propertyName} changed to {value}");
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
