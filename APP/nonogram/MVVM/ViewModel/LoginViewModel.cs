﻿using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using nonogram.Common;
using nonogram.DB;
using nonogram.MVVM.View;
using System.Diagnostics;

namespace nonogram.MVVM.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        public string UN { get; set; }
        public string PW { get; set; }


        private string _userName;
        private bool _isLoginSuccessful;

        public string UserName
        {
            get => _userName;
            private set => SetProperty(ref _userName, value);
        }
        public bool IsLoginSuccessful
        {
            get => _isLoginSuccessful;
            set => SetProperty(ref _isLoginSuccessful, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        private readonly ViewModelFactory _viewModelFactory;

        public LoginViewModel(ViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            RegisterCommand = new RelayCommand<object>(ShowRegisterView);
            ForgotPasswordCommand = new RelayCommand<object>(ShowForgotPasswordView);
            LoginCommand = new RelayCommand<object>(Login);

            IsLoginSuccessful = false;
        }

        private void ShowRegisterView(object parameter)
        {
            var registerViewModel = _viewModelFactory.CreateRegisterViewModel();
            LoginNavigationHelper.NavigateToRegisterWindow(registerViewModel);
        }

        private void ShowForgotPasswordView(object parameter)
        {
            var forgotPasswordViewModel = _viewModelFactory.CreateForgotPasswordViewModel();
            LoginNavigationHelper.NavigateToForgotPasswordWindow(forgotPasswordViewModel);
        }

        private void Login(object parameter)
        {
            var loginView = parameter as LoginView;

            var username = UN ?? loginView?.username_tb.Text;
            var password = PW ?? loginView?.password_tb.Password;
            //var username = loginView.username_tb.Text;
            //var password = loginView.password_tb.Password;

            //Validate input
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                // Check the database for the user
                var dbManager = new DbManager();
                string query = "SELECT * FROM USER WHERE UserName = @Username OR Email = @Email";
                var parameters = new Dictionary<string, object>
                {
                    { "@Username", username },
                    { "@Email", username }
                };
                var userTable = dbManager.ExecuteQuery(query, parameters);

                if (userTable.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid username or password.");
                    IsLoginSuccessful = false;
                    return;
                }

                var userRow = userTable.Rows[0];
                var storedPasswordHash = userRow["Password"].ToString();
                var inputPasswordHash = HashHelper.ComputeSha256Hash(password);

                if (storedPasswordHash != inputPasswordHash)
                {
                    MessageBox.Show("Invalid username or password.");
                    IsLoginSuccessful = false;
                    return;
                }

                // Successful login
                UserName = userRow["UserName"].ToString();
                IsLoginSuccessful = true;
                //Debug.WriteLine($"Login successful for user: {UserName}");

                // Close the login window
                Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault()?.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                IsLoginSuccessful = false;
                //Debug.WriteLine("Login failed. Error.");
            }
        }
    }
}
