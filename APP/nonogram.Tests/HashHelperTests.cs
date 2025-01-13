using Microsoft.VisualStudio.TestTools.UnitTesting;
using nonogram.Common;

namespace nonogram.Tests
{
    /// <summary>
    /// Test HashHelper class
    /// </summary>
    [TestClass]
    public class HashHelperTests
    {
        [TestMethod]
        public void ComputeSha256Hash_SameInput_ReturnsSameHash()
        {
            // Arrange
            string input = "test@example.com";

            // Act
            string hash1 = HashHelper.ComputeSha256Hash(input);
            string hash2 = HashHelper.ComputeSha256Hash(input);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void ComputeSha256Hash_DifferentInputs_ReturnsDifferentHashes()
        {
            // Arrange
            string input1 = "test@example.com";
            string input2 = "different@example.com";

            // Act
            string hash1 = HashHelper.ComputeSha256Hash(input1);
            string hash2 = HashHelper.ComputeSha256Hash(input2);

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
