using nonogram.MVVM.ViewModel;

namespace nonogram.Common
{
    public class ViewModelFactory
    {
        private string _username;

        public ViewModelFactory(string username = null)
        {
            _username = username;
        }

        public LoginViewModel CreateLoginViewModel()
        {
            return new LoginViewModel();
        }

        public RegisterViewModel CreateRegisterViewModel()
        {
            return new RegisterViewModel(CreateLoginViewModel());
        }

        public ForgotPasswordViewModel CreateForgotPasswordViewModel()
        {
            return new ForgotPasswordViewModel(CreateLoginViewModel());
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
