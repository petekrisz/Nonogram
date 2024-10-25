using nonogram.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using nonogram.DB;
using System.Windows.Input;


namespace nonogram.MVVM.ViewModel
{

    public class ImageListViewModel : INotifyPropertyChanged
    {
        
        // private static Random rnd = new Random(); <-- it is not used now

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ListImage> ImagesLeft { get; set; }
        public ObservableCollection<ListImage> ImagesRight { get; set; }
        public ICommand ImageSelectedCommand { get; set; }

        public ImageListViewModel()
        {
            ImagesLeft = new ObservableCollection<ListImage>();
            ImagesRight = new ObservableCollection<ListImage>();
            LoadImages();
        }

        private void LoadImages()
        {
            var dataStorage = new DataStorage("Image_table.csv"); // For now it is set to a static csv file instead of DB
            var imageTableList = dataStorage.ImageTableList;

            foreach (var image in imageTableList) 
            {
                var item = new ListImage
                {
                    ImageSource = $"/Images/{image.CategoryLogo}",
                    ImageTitle = image.Title,
                    ImageDetails = $"Colour: {(image.ColourType == 0 ? "BW" : "C")} / Size: {image.Rows} * {image.Columns} / Score: {image.Score}",
                    Image = image
                };

                if (ImagesLeft.Count <= ImagesRight.Count)
                {
                    ImagesLeft.Add(item);
                }
                else
                {
                    ImagesRight.Add(item);
                }
            }
        }

        /* Now it is not needed anymore
         * public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }*/

    }

    
}
