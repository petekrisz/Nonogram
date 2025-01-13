using Microsoft.VisualStudio.TestTools.UnitTesting;
using nonogram.Common;
using nonogram.MVVM.ViewModel;

namespace nonogram.Tests
{
    /// <summary>
    /// Test Email validations
    /// -- No @ symbol, no domain 
    /// </summary>
    [TestClass]
    public class EmailValidatorTests
    {
        [TestMethod]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            string email = "test@example.com";

            // Act
            bool result = EmailValidator.IsValidEmail(email);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmail_NoAtSymbol_ReturnsFalse()
        {
            // Arrange
            string email = "testexample.com";

            // Act
            bool result = EmailValidator.IsValidEmail(email);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidEmail_NoDomain_ReturnsFalse()
        {
            // Arrange
            string email = "test@";

            // Act
            bool result = EmailValidator.IsValidEmail(email);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
