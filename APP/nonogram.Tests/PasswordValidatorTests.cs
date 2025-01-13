using Microsoft.VisualStudio.TestTools.UnitTesting;
using nonogram.Common;
using nonogram.MVVM.ViewModel;

namespace nonogram.Tests
{
    /// <summary>
    /// Test Password validations
    /// -- Valid password, short password, no number, no uppercase
    /// </summary>
    [TestClass]
    public class PasswordValidatorTests
    {
        [TestMethod]
        public void IsValidPassword_ValidPassword_ReturnsTrue()
        {
            // Arrange
            string password = "Valid123";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidPassword_ShortPassword_ReturnsFalse()
        {
            // Arrange
            string password = "Val1";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_NoNumber_ReturnsFalse()
        {
            // Arrange
            string password = "ValidPassword";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_NoUppercase_ReturnsFalse()
        {
            // Arrange
            string password = "valid123";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
