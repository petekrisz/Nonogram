using nonogram.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;


namespace nonogram.MVVM.ViewModel
{
    
    public class ImageListViewModel : INotifyPropertyChanged
    {
        
        private static Random rnd = new Random(); // rnd is used her for generating dummy strings for ListImage class - it is not needed in the final version

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ListImage> ImagesLeft { get; set; }
        public ObservableCollection<ListImage> ImagesRight { get; set; }

        public ImageListViewModel()
        {
            ImagesLeft = new ObservableCollection<ListImage>();
            ImagesRight = new ObservableCollection<ListImage>();
            ShowImages();
        }

        private void ShowImages()
        {
            
            int NumbersOfImages = 20; // Here the NumberOfImages should be obtained from DB Image table
            for (int i = 0; i < NumbersOfImages; i++) 
            {
                var item = new ListImage
                {
                    ImageSource = "/Images/logo.png", // Now it is the logo, but it should be replaced the URL from the DB
                    ImageTitle = $"Title: {RandomString(16)} {i + 1}", //Title should be obtained from DB
                    ImageDetails = $"Colour: {RandomString(2)} / Size:{RandomString(8)} / Score: {RandomString(5)} {i + 1}" //Deatails should be obtained from DB
                };

                if (i % 2 == 0)
                {
                    ImagesLeft.Add(item);
                }
                else
                {
                    ImagesRight.Add(item);
                }
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

    }

    
}
