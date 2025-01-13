using Microsoft.VisualStudio.TestTools.UnitTesting;
using nonogram.MVVM.ViewModel;
using nonogram.Common;
using nonogram.MVVM.View;
using System.Windows.Controls;

namespace nonogram.Tests.ViewModelTests
{
    [TestClass]
    public class LoginViewModelTests
    {
            private ViewModelFactory _viewModelFactory;
            private LoginViewModel _viewModel;

            [TestInitialize]
            public void SetUp()
            {
                // Initialize the ViewModelFactory
                _viewModelFactory = new ViewModelFactory();

                // Use the factory to create the LoginViewModel
                _viewModel = _viewModelFactory.CreateLoginViewModel();
            }

            [TestMethod]
            public void LoginCommand_ShouldExecute_WhenCredentialsAreValid()
            {
                // Arrange
                _viewModel.UN = "testuser";
                _viewModel.PW = "password123";

                // Act
                var canExecute = _viewModel.LoginCommand.CanExecute(null);
                if (canExecute)
                {
                    _viewModel.LoginCommand.Execute(null);
                }

                // Assert
                Assert.IsTrue(canExecute, "LoginCommand should execute for valid credentials.");
            }

        [TestMethod]
        public void LoginCommand_ShouldFail_WhenCredentialsAreInvalid()
        {
            // Arrange
            _viewModel.UN = "invaliduser";
            _viewModel.PW = "wrongpassword";

            // Act
            var canExecute = _viewModel.LoginCommand.CanExecute(null);
            if (canExecute)
            {
                _viewModel.LoginCommand.Execute(null);
            }

            // Assert
            Assert.IsTrue(canExecute, "LoginCommand should execute even for invalid credentials.");
            Assert.IsFalse(_viewModel.IsLoginSuccessful, "Login should fail for invalid credentials.");
        }


    }
}