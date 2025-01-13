using Microsoft.VisualStudio.TestTools.UnitTesting;
using nonogram.Common;
using nonogram.MVVM.ViewModel;
using System.Diagnostics;

namespace nonogram.Tests.ViewModelTests
{
    [TestClass]
    public class RegisterViewModelTests
    {
        private ViewModelFactory _viewModelFactory;
        private RegisterViewModel _viewModel;

        [TestInitialize]
        public void SetUp()
        {
            // Initialize the ViewModelFactory
            _viewModelFactory = new ViewModelFactory();

            // Use the factory to create the RegisterViewModel
            _viewModel = _viewModelFactory.CreateRegisterViewModel();
        }

        [TestMethod]
        public void RegisterCommand_ShouldExecute_WhenInputsAreValid()
        {
            Debug.WriteLine("RegisterCommand_ShouldExecute_WhenInputsAreValid");
            // Arrange
            _viewModel.UN = "newuser";
            _viewModel.FN = "firstnametest";
            _viewModel.LN = "lastnametest";
            _viewModel.PW = "Password123";
            _viewModel.CPW = "Password123";
            _viewModel.EM = "newusertest@example.com";

            // Act
            var canExecute = _viewModel.RegisterCommand.CanExecute(null);
            if (canExecute)
            {
                _viewModel.RegisterCommand.Execute(null);
            }

            // Assert
            Assert.IsTrue(canExecute, "RegisterCommand should execute for valid inputs.");
            Assert.IsTrue(_viewModel.IRS, "Registration should succeed for valid inputs.");
        }

        [TestMethod]
        public void RegisterCommand_ShouldFail_WhenPasswordsDoNotMatch()
        {
            Debug.WriteLine("RegisterCommand_ShouldFail_WhenPasswordsDoNotMatch");
            // Arrange
            _viewModel.UN = "newuser";
            _viewModel.FN = "firstnametest";
            _viewModel.LN = "lastnametest";
            _viewModel.PW = "Password123";
            _viewModel.CPW = "Password456"; // Mismatched password
            _viewModel.EM = "newusertest2@example.com";

            // Act
            var canExecute = _viewModel.RegisterCommand.CanExecute(null);
            if (canExecute)
            {
                _viewModel.RegisterCommand.Execute(null);
            }

            // Assert
            Assert.IsTrue(canExecute, "RegisterCommand should execute even for invalid inputs.");
            Assert.IsFalse(_viewModel.IRS, "Registration should fail when passwords do not match.");
        }

        [TestMethod]
        public void RegisterCommand_ShouldFail_WhenEmailIsMissing()
        {
            Debug.WriteLine("RegisterCommand_ShouldFail_WhenEmailIsMissing");
            // Arrange
            _viewModel.UN = "newuser";
            _viewModel.FN = "firstnametest";
            _viewModel.LN = "lastnametest";
            _viewModel.PW = "password123";
            _viewModel.CPW = "password123";
            _viewModel.EM = ""; // Missing email

            // Act
            var canExecute = _viewModel.RegisterCommand.CanExecute(null);
            if (canExecute)
            {
                _viewModel.RegisterCommand.Execute(null);
            }

            // Assert
            Assert.IsTrue(canExecute, "RegisterCommand should execute even for invalid inputs.");
            Assert.IsFalse(_viewModel.IRS, "Registration should fail when email is missing.");
        }


    }
}
