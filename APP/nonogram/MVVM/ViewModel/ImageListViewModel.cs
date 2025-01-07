using nonogram.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using nonogram.DB;
using System.Windows.Input;
using System.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using nonogram.Common;
using nonogram.MVVM.View;
using System.Windows.Data;


namespace nonogram.MVVM.ViewModel
{
    public class ImageListViewModel : ObservableObject
    {
        public Action RefreshView { get; set; }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        // The property for the search term
        private string _searchBar;
        public string SearchBar
        {
            get { return _searchBar; }
            set
            {
                if (_searchBar != value)
                {
                    _searchBar = value;
                    OnPropertyChanged(nameof(SearchBar));
                    //FilterImages(_searchBar); // Call FilterImages whenever the search term changes
                }
            }
        }

        // Notifies the UI when a property value changes.
        public new event PropertyChangedEventHandler PropertyChanged;
        protected new void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Observable collection for the images displayed in the left column.
        private ObservableCollection<ListImage> _imagesLeft;
        public ObservableCollection<ListImage> ImagesLeft
        {
            get { return _imagesLeft; }
            set
            {
                if (_imagesLeft != value)
                {
                    _imagesLeft = value;
                    OnPropertyChanged(nameof(ImagesLeft));
                }
            }
        }
        // Observable collection for the images displayed in the right column.
        private ObservableCollection<ListImage> _imagesRight;
        public ObservableCollection<ListImage> ImagesRight
        {
            get { return _imagesRight; }
            set
            {
                if (_imagesRight != value)
                {
                    _imagesRight = value;
                    OnPropertyChanged(nameof(ImagesRight));
                }
            }
        }
        //public ICommand ImageSelectedCommand { get; set; }


        // Parameterless constructor for design-time
        public ImageListViewModel() : this("netuddki") { }

        public ImageListViewModel(string username)
        {
            Username = username;
            ImagesLeft = new ObservableCollection<ListImage>();
            ImagesRight = new ObservableCollection<ListImage>();
            SearchBar = string.Empty; // Initialize SearchBar to empty
            FilterImages(SearchBar); // Load images based on the initial search term

        }

        public void FilterImages(string searchTerm)
        {
            //Debug.WriteLine($"FilterImages called with searchTerm: '{searchTerm}'");

            ImagesLeft.Clear();
            ImagesRight.Clear();

            //// New temporary collections
            //var newImagesLeft = new ObservableCollection<ListImage>();
            //var newImagesRight = new ObservableCollection<ListImage>();

            // Query IMAGE table for _username
            DbManager dbManager = new DbManager();
            string query = @"
                            SELECT IMAGEId, Title, IMAGERows, IMAGEColumns, Category, CategoryLogo, Score, ColourType
                            FROM IMAGE
                            WHERE Category LIKE @Search OR Title LIKE @Search";
            var parameters = new Dictionary<string, object>
            {
                { "@Search", $"%{searchTerm}%" }
            };
            var dataTable = dbManager.ExecuteQuery(query, parameters);

            // Query USERIMAGE table for _username
            string userImageQuery = @"
            SELECT IMAGEId, Finished
            FROM USERIMAGE
            WHERE UserName = @UserName";
            var userImageParameters = new Dictionary<string, object>
            {
                { "@UserName", _username }
            };
            var userImageTable = dbManager.ExecuteQuery(userImageQuery, userImageParameters);

            // Create a dictionary to store the USERIMAGE data
            var userImageDict = new Dictionary<int, bool>();
            foreach (DataRow row in userImageTable.Rows)
            {
                int imageId = Convert.ToInt32(row["IMAGEId"]);
                bool finished = Convert.ToBoolean(row["Finished"]);
                userImageDict[imageId] = finished;
            }



            foreach (DataRow row in dataTable.Rows)
            {
                int imageId = Convert.ToInt32(row["IMAGEId"]);
                string categoryLogo = row["CategoryLogo"].ToString();
                string imageSource = $"/Images/{categoryLogo}";

                // Check if the image is in the USERIMAGE table
                if (userImageDict.TryGetValue(imageId, out bool finished))
                {
                    if (finished)
                    {
                        imageSource = "/Images/Done_icon_2.png";
                    }
                    else
                    {
                        imageSource = $"/Images/{categoryLogo.Replace("gold", "light")}";
                    }
                }
                var item = new ListImage
                {
                    IMAGEId = imageId,
                    ImageSource = imageSource,
                    ImageTitle = row["Title"].ToString(),
                    ImageDetails = $"Colour: {(Convert.ToInt32(row["ColourType"]) == 0 ? "BW" : "C")} / Size: {row["IMAGERows"]} * {row["IMAGEColumns"]} / Score: {row["Score"]}"
                };

                if (ImagesLeft.Count <= ImagesRight.Count)
                    ImagesLeft.Add(item);
                else
                    ImagesRight.Add(item);
            }

            if (dataTable.Rows.Count == 0)
            {
                var noImageItem = new ListImage
                {
                    IMAGEId = -1,
                    ImageSource = "/Images/NO_IMAGE_icon.png",
                    ImageTitle = "No Image Found!",
                    ImageDetails = ""
                };
                ImagesLeft.Add(noImageItem);
            }

            //// Replace collections
            //ImagesLeft = newImagesLeft;
            //ImagesRight = newImagesRight;

            //Debug.WriteLine($"ImagesLeft count: {ImagesLeft.Count}");
            //Debug.WriteLine($"ImagesRight count: {ImagesRight.Count}");

            OnPropertyChanged(nameof(ImagesLeft));
            OnPropertyChanged(nameof(ImagesRight));

            RefreshView?.Invoke();
        }

    }


}
