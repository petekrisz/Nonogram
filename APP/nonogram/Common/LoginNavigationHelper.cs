using nonogram.MVVM.View;
using nonogram.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace nonogram.Common
{
    public static class LoginNavigationHelper
    {
        public static void NavigateToLoginWindow(LoginViewModel loginViewModel)
        {
            var parentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginWindow);
            if (parentWindow == null)
            {
                //Debug.WriteLine("Parent window is null.");
                return;
            }

            // Find the named ContentControl
            var contentControl = parentWindow.FindName("MainContentControl") as ContentControl;
            if (contentControl == null)
            {
                //Debug.WriteLine("ContentControl is null.");
                return;
            }

            // Swap to LoginView
            var loginView = new LoginView
            {
                DataContext = loginViewModel
            };
            //Debug.WriteLine($"NavigateToLoginWindow: LoginView DataContext: {loginView.DataContext.GetHashCode()}");
            contentControl.Content = loginView;
        }

        public static void NavigateToRegisterWindow(RegisterViewModel registerViewModel)
        {
            var parentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginWindow);
            if (parentWindow == null)
            {
                //Debug.WriteLine("Parent window is null.");
                return;
            }

            // Find the named ContentControl
            var contentControl = parentWindow.FindName("MainContentControl") as ContentControl;
            if (contentControl == null)
            {
                //Debug.WriteLine("ContentControl is null.");
                return;
            }

            // Swap to RegisterView
            var registerView = new RegisterView
            {
                DataContext = registerViewModel
            };
            //Debug.WriteLine($"NavigateToRegisterWindow: RegisterView DataContext: {registerView.DataContext.GetHashCode()}");
            contentControl.Content = registerView;
        }

        public static void NavigateToForgotPasswordWindow(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            var parentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginWindow);
            if (parentWindow == null)
            {
                //Debug.WriteLine("Parent window is null.");
                return;
            }

            // Find the named ContentControl
            var contentControl = parentWindow.FindName("MainContentControl") as ContentControl;
            if (contentControl == null)
            {
                //Debug.WriteLine("ContentControl is null.");
                return;
            }

            // Swap to ForgotPasswordView
            var forgotPasswordView = new ForgotPasswordView
            {
                DataContext = forgotPasswordViewModel
            };
            //Debug.WriteLine($"NavigateToForgotPasswordWindow: ForgotPasswordView DataContext: {forgotPasswordView.DataContext.GetHashCode()}");
            contentControl.Content = forgotPasswordView;
        }
    }
}
