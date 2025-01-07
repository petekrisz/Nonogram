using nonogram.MVVM.ViewModel;
using System.Diagnostics;

namespace nonogram.Common
{
    public class ViewModelFactory
    {
        private string _username;
        private LoginViewModel _loginViewModel;

        public ViewModelFactory(string username = null)
        {
            _username = username;
            _loginViewModel = new LoginViewModel(this);
        }

        public LoginViewModel CreateLoginViewModel()
        {
            //Debug.WriteLine($"CreateLoginViewModel called in ViewModelFactory: {_username}");
            return _loginViewModel;
        }

        public RegisterViewModel CreateRegisterViewModel()
        {
            return new RegisterViewModel(_loginViewModel);
        }

        public ForgotPasswordViewModel CreateForgotPasswordViewModel()
        {
            return new ForgotPasswordViewModel(_loginViewModel);
        }

        public MainViewModel CreateMainViewModel(string username)
        {
            return new MainViewModel(username);
        }
        public void SetUsername(string username)
        {
            _username = username;
        }
    }
}
