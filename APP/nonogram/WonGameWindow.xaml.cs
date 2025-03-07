﻿using nonogram.DB;
using nonogram.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace nonogram
{
    /// <summary>
    /// Interaction logic for WonGameWindow.xaml
    /// </summary>
    public partial class WonGameWindow : Window
    {
        private string _userName;
        private int _score;
        private int _token;
        private Dictionary<string, double> _helpDictionary;
        private int _imageId;

        private readonly BitmapImage _firework_g;
        private readonly BitmapImage _firework_l;
        private bool _showFirstImage;
        public WonGameWindow(int imageId, string username)
        {
            InitializeComponent();

            //Debug.WriteLine($"WonGameWindow initialized with imageId: {imageId}");

            _firework_g = new BitmapImage(new Uri("pack://application:,,,/Images/firework_icon_gold.png"));
            _firework_l = new BitmapImage(new Uri("pack://application:,,,/Images/firework_icon_light.png"));

            // Initialize the Image control with the first image
            fireworks.Source = _firework_g;
            _showFirstImage = true;

            // Set up the timer
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(400)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            // Start the grow/shrink animation
            Storyboard growShrinkAnimation = (Storyboard)FindResource("GrowShrinkAnimation");
            growShrinkAnimation.Begin();

            _userName = username;
            _imageId = imageId;
            var dbManager = new DbManager();
            var image = dbManager.GetImageById(imageId);
            _score = image.Score;
            _token = (int)Math.Floor((double)image.Score / 50);

            // Step 1: Query the HELP table
            string query = "SELECT TypeOfHelp, Weight, HelpLogoG, HelpLogoL FROM HELP";
            DataTable helpTable = dbManager.ExecuteQuery(query);

            // Step 2: Fill the dictionary
            _helpDictionary = new Dictionary<string, double>();
            foreach (DataRow row in helpTable.Rows)
            {
                string typeOfHelp = row["TypeOfHelp"].ToString();
                double weight = Convert.ToDouble(row["Weight"]);
                _helpDictionary[typeOfHelp] = weight;
            }

            // Step 3: Modify the dictionary values
            Random random = new Random();
            foreach (var key in _helpDictionary.Keys.ToList())
            {
                double weight = _helpDictionary[key];
                int randomNumber = random.Next(150, 501);
                int newValue = (int)Math.Floor(weight * _score / randomNumber);
                _helpDictionary[key] = newValue;
            }
            // Remove key-value pairs where the value is 0
            _helpDictionary = _helpDictionary.Where(kvp => kvp.Value > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Step 4: Create the first two StackPanels for Score and Tokens
            CreatePrizeStackPanelWithImage("/Images/score_icon_light.png", _score.ToString());
            CreatePrizeStackPanelWithImage("/Images/token_icon_light.png", Math.Floor((double)_score / 50).ToString());

            // Step 5: Create the remaining StackPanels for the help
            foreach (var kvp in _helpDictionary)
            {
                string typeOfHelp = kvp.Key;
                int value = (int)kvp.Value;
                DataRow row = helpTable.Rows.Cast<DataRow>().First(r => r["TypeOfHelp"].ToString() == typeOfHelp);
                string helpLogo = row["HelpLogoL"].ToString();
                CreatePrizeStackPanelWithImage($"Images/{helpLogo}", value.ToString());
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            fireworks.Source = _showFirstImage ? _firework_l : _firework_g;
            _showFirstImage = !_showFirstImage;
        }

        private void CreatePrizeStackPanelWithImage(string imageUrl, string valueText)
        {
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(5),
                Width = 70
            };

            // Upper part
            System.Windows.Controls.Image image = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(imageUrl, UriKind.RelativeOrAbsolute)),
                Height = 70,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            stackPanel.Children.Add(image);

            // Bottom part
            TextBlock value = new TextBlock
            {
                Text = valueText,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Bottom,
                TextAlignment = TextAlignment.Center
            };
            stackPanel.Children.Add(value);

            Prizes.Children.Add(stackPanel);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            PerformActionsAndClose();
        }

        private void PerformActionsAndClose()
        {
            //Debug.WriteLine("PerformActionsAndClose called");

            // Get the MainWindow and its DataContext (MainViewModel)
            if (System.Windows.Application.Current.MainWindow.DataContext is MainViewModel mainViewModel)
            {
                // Navigate back to ImageListView
                mainViewModel.ImageListViewCommand.Execute(null);

                // Execute an insert to the database
                InsertToUserImageAndUserHelp();

                // Refresh the ImageListView
                mainViewModel.RefreshImageListView();

                // Close the current window
                this.Close();

                //Refresh Userdetails and HelpTable
                mainViewModel.LoadUserData(_userName);
                mainViewModel.HelpTableVM.LoadHelpOptions();
            }
            else
            {
                MessageBox.Show("Main window or its DataContext is not set correctly.");
            }
        }

        private void InsertToUserImageAndUserHelp()
        {
            //Debug.WriteLine("InsertToUserImageAndUserHelp called");
            var dbManager = new DbManager();


            try
            {
                // Update USER table
                string userQuery = "SELECT Score, Tokens FROM USER WHERE UserName = @UserName";
                var userParameters = new Dictionary<string, object> { { "@UserName", _userName } };
                DataTable userTable = dbManager.ExecuteQuery(userQuery, userParameters);
                if (userTable.Rows.Count > 0)
                {
                    DataRow userRow = userTable.Rows[0];
                    int currentScore = Convert.ToInt32(userRow["Score"]);
                    int currentTokens = Convert.ToInt32(userRow["Tokens"]);
                    int newScore = currentScore + _score;
                    int newTokens = currentTokens + _token;

                    //Debug.WriteLine($"Current score: {currentScore} - {newScore}, Current tokens: {currentTokens} - {newTokens}");

                    string updateUserQuery = "UPDATE USER SET Score = @Score, Tokens = @Tokens WHERE UserName = @UserName";
                    var updateUserParameters = new Dictionary<string, object>
                    {
                            { "@Score", newScore },
                            { "@Tokens", newTokens },
                            { "@UserName", _userName }
                    };

                    dbManager.ExecuteNonQuery(updateUserQuery, updateUserParameters);
                    //Debug.WriteLine($"USER table updated successfully. New Score: {newScore}, New Tokens: {newTokens}");
                }

                // Update USERHELP table
                string userHelpQuery = "SELECT * FROM USERHELP WHERE UserName = @UserName";
                var userHelpParameters = new Dictionary<string, object> { { "@UserName", _userName } };
                DataTable userHelpTable = dbManager.ExecuteQuery(userHelpQuery, userHelpParameters);
                if (userHelpTable.Rows.Count > 0)
                {
                    DataRow userHelpRow = userHelpTable.Rows[0];
                    var updateHelpValues = new Dictionary<string, object> { { "@UserName", _userName } };
                    string updateHelpQuery = "UPDATE USERHELP SET ";
                    foreach (var kvp in _helpDictionary)
                    {
                        string column = kvp.Key;
                        int value = (int)kvp.Value;
                        int currentValue = Convert.ToInt32(userHelpRow[column]);
                        int newValue = currentValue + value;
                        updateHelpQuery += $"{column} = @{column}, ";
                        updateHelpValues[$"@{column}"] = newValue;
                    }

                    updateHelpQuery = updateHelpQuery.TrimEnd(',', ' ') + " WHERE UserName = @UserName";
                    dbManager.ExecuteNonQuery(updateHelpQuery, updateHelpValues);
                    //Debug.WriteLine("USERHELP table updated successfully.");
                }

                // Check if the combination of UserName and IMAGEId already exists in the USERIMAGE table
                string checkUserImageQuery = "SELECT COUNT(*) FROM USERIMAGE WHERE UserName = @UserName AND IMAGEId = @IMAGEId";
                var checkParameters = new Dictionary<string, object>
                {
                    { "@UserName", _userName },
                    { "@IMAGEId", _imageId }
                };
                DataTable checkTable = dbManager.ExecuteQuery(checkUserImageQuery, checkParameters);
                if (checkTable.Rows.Count > 0 && Convert.ToInt32(checkTable.Rows[0][0]) > 0)
                {
                    // If the combination exists, update the record
                    string updateUserImageQuery = "UPDATE USERIMAGE SET Finished = @Finished, Content = @Content WHERE UserName = @UserName AND IMAGEId = @IMAGEId";
                    var updateUserImageParameters = new Dictionary<string, object>
            {
                { "@Finished", true },
                { "@Content", "x" },
                { "@UserName", _userName },
                { "@IMAGEId", _imageId }
            };
                    dbManager.ExecuteNonQuery(updateUserImageQuery, updateUserImageParameters);
                    //Debug.WriteLine("USERIMAGE table updated successfully.");
                }
                else
                {
                    // If the combination doesn't exist, insert a new record
                    var userImageValues = new Dictionary<string, object>
            {
                { "UserName", _userName },
                { "IMAGEId", _imageId },
                { "Finished", true },
                { "Content", "x" }
            };
                    dbManager.ExecuteNonQuery("INSERT INTO USERIMAGE (UserName, IMAGEId, Finished, Content) VALUES (@UserName, @IMAGEId, @Finished, @Content)", userImageValues);
                    //Debug.WriteLine("USERIMAGE table updated successfully.");
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"Error inserting data into the database: {ex.Message}");
            }
        }

    }
}
