using nonogram.Common;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace nonogram.MVVM.ViewModel
{
    public class UserMenuViewModel : ObservableObject
    {
        private string _fullName;
        private int _userPoints;
        private int _userTokens;
        private string _newPassword;

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public int UserPoints
        {
            get => _userPoints;
            set => SetProperty(ref _userPoints, value);
        }

        public int UserTokens
        {
            get => _userTokens;
            set => SetProperty(ref _userTokens, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public ICommand ChangePasswordCommand { get; }

        public UserMenuViewModel()
        {
            ChangePasswordCommand = new RelayCommand<object>(_ => ChangePassword());
            LoadUserData();
        }

        private void LoadUserData()
        {
            var filePath = "USER.csv"; // Helyes elérési útvonal

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Nem található a következő fájl: „{filePath}”", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var email = "somethingratherdifferent@something.else"; // A bejelentkezett felhasználó e-mail címe

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(';');
                if (parts.Length >= 5 && parts[4] == email)
                {
                    FullName = $"{parts[2]} {parts[3]}";
                    UserPoints = int.Parse(parts[6]);
                    UserTokens = int.Parse(parts[7]);
                    break;
                }
            }
        }

        private void ChangePassword()
        {
            // A jelszóváltoztatás logikájának megvalósítása
            // Például, ellenőrizzük, hogy az új jelszó nem üres-e, majd mentjük az adatokat
            if (!string.IsNullOrWhiteSpace(NewPassword))
            {
                var filePath = "USER.csv"; // Helyes elérési útvonal

                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Nem található a következő fájl: „{filePath}”", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var email = "somethingratherdifferent@something.else"; // A bejelentkezett felhasználó e-mail címe
                var lines = File.ReadAllLines(filePath).ToList();

                for (int i = 1; i < lines.Count; i++)
                {
                    var parts = lines[i].Split(';');
                    if (parts.Length >= 5 && parts[4] == email)
                    {
                        parts[5] = NewPassword; // Jelszó frissítése
                        lines[i] = string.Join(";", parts);
                        break;
                    }
                }

                File.WriteAllLines(filePath, lines);
                MessageBox.Show("Jelszó sikeresen megváltoztatva!", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
