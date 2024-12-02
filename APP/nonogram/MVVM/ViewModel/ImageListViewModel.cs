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


namespace nonogram.MVVM.ViewModel
{

    public class ImageListViewModel : INotifyPropertyChanged
    {
        private string _searchBar;

        // The property for the search term
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
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
        public ICommand ImageSelectedCommand { get; set; }

        public ImageListViewModel()
        {
            ImagesLeft = new ObservableCollection<ListImage>();
            ImagesRight = new ObservableCollection<ListImage>();
            SearchBar = string.Empty; // Initialize SearchBar to empty
            FilterImages(SearchBar); // Load images based on the initial search term
        }

        public void FilterImages(string searchTerm)
        {
            Debug.WriteLine($"FilterImages called with searchTerm: '{searchTerm}'");


            ImagesLeft.Clear();
            ImagesRight.Clear();

            // New temporary collections
            var newImagesLeft = new ObservableCollection<ListImage>();
            var newImagesRight = new ObservableCollection<ListImage>();

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

            foreach (DataRow row in dataTable.Rows)
            {
                var item = new ListImage
                {
                    IMAGEId = Convert.ToInt32(row["IMAGEId"]),
                    ImageSource = $"/Images/{row["CategoryLogo"]}",
                    ImageTitle = row["Title"].ToString(),
                    ImageDetails = $"Colour: {(Convert.ToInt32(row["ColourType"]) == 0 ? "BW" : "C")} / Size: {row["IMAGERows"]} * {row["IMAGEColumns"]} / Score: {row["Score"]}"
                };

                if (newImagesLeft.Count <= newImagesRight.Count)
                    newImagesLeft.Add(item);
                else
                    newImagesRight.Add(item);
            }

            if (dataTable.Rows.Count == 0)
            {
                var noImageItem = new ListImage
                {
                    IMAGEId = -1,
                    ImageSource = "/Images/No_icon_gold.png",
                    ImageTitle = "No Image Found!",
                    ImageDetails = ""
                };
                newImagesLeft.Add(noImageItem);
            }

            // Replace collections
            ImagesLeft = newImagesLeft;
            ImagesRight = newImagesRight;

            Debug.WriteLine($"ImagesLeft count: {ImagesLeft.Count}");
            Debug.WriteLine($"ImagesRight count: {ImagesRight.Count}");

            OnPropertyChanged(nameof(ImagesLeft));
            OnPropertyChanged(nameof(ImagesRight));
        }



        //public void FilterImages(string searchTerm)
        //{
        //    Debug.WriteLine($"FilterImages called with searchTerm: '{searchTerm}'");

        //    ImagesLeft.Clear();
        //    ImagesRight.Clear();

        //    DbManager dbManager = new DbManager();
        //    string query = @"
        //        SELECT IMAGEId, Title, IMAGERows, IMAGEColumns, Category, CategoryLogo, Score, ColourType
        //        FROM IMAGE
        //        WHERE Category LIKE @Search OR Title LIKE @Search";
        //    var parameters = new Dictionary<string, object>
        //    {
        //        { "@Search", $"%{searchTerm}%" }
        //    };
        //    var dataTable = dbManager.ExecuteQuery(query, parameters);
        //    Debug.WriteLine($"FilterImages: Found {dataTable.Rows.Count} results.");

        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        var item = new ListImage
        //        {
        //            IMAGEId = Convert.ToInt32(row["IMAGEId"]),
        //            ImageSource = $"/Images/{row["CategoryLogo"]}",
        //            ImageTitle = row["Title"].ToString(),
        //            ImageDetails = $"Colour: {(Convert.ToInt32(row["ColourType"]) == 0 ? "BW" : "C")} / Size: {row["IMAGERows"]} * {row["IMAGEColumns"]} / Score: {row["Score"]}"
        //        };
        //        if (ImagesLeft.Count <= ImagesRight.Count)
        //        {
        //            ImagesLeft.Add(item);
        //            Debug.WriteLine("one item added to ImagesLeft.");
        //        }
        //        else
        //        {
        //            ImagesRight.Add(item);
        //            Debug.WriteLine("one item added to ImagesRight.");
        //        }
        //    }

        //    if (dataTable.Rows.Count == 0)
        //    {
        //        Debug.WriteLine($"No result called.");
        //        var noImageItem = new ListImage
        //        {
        //            IMAGEId = -1,
        //            ImageSource = "/Images/No_icon_gold.png",
        //            ImageTitle = "No Image Found!",
        //            ImageDetails = ""
        //        };
        //        ImagesLeft.Add(noImageItem);
        //        Debug.WriteLine($"noImageItem added as special case.");
        //    }

        //    // Notify the UI that the collections have changed
        //    OnPropertyChanged(nameof(ImagesLeft));
        //    OnPropertyChanged(nameof(ImagesRight));
        //    Debug.WriteLine($"ImagesLeft count: {ImagesLeft.Count}");
        //    Debug.WriteLine($"ImagesRight count: {ImagesRight.Count}");
        //}
    }


}
