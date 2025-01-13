using Microsoft.VisualStudio.TestTools.UnitTesting;
using nonogram.DB;
using System.Data;
using System.Collections.Generic;

namespace nonogram.Tests
{
    /// <summary>
    /// Test Database operations
    /// -- Drop and create tables
    /// -- Create, Read, Update, Delete operations on the User table
    /// </summary>
    [TestClass]
    public class DbManagerTests
    {
        private DbManager dbManager;

        [TestInitialize]
        public void Setup()
        {
            dbManager = new DbManager();
            dbManager.InitializeDatabaseAndTables();
        }

        [TestMethod]
        public void ExecuteNonQuery_ShouldExecuteWithoutErrors()
        {
            // Arrange
            string dropQuery = "DROP TABLE IF EXISTS TestTable";
            string createQuery = "CREATE TABLE TestTable (ID INT PRIMARY KEY, Name VARCHAR(50))";

            // Act
            dbManager.ExecuteNonQuery(dropQuery);
            dbManager.ExecuteNonQuery(createQuery);
        }

        [TestMethod]
        public void ExecuteQuery_ShouldReturnDataTable()
        {
            // Arrange
            string query = "SELECT * FROM TestTable";

            // Act
            DataTable result = dbManager.ExecuteQuery(query);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IsTablePopulated_ShouldReturnFalseForEmptyTable()
        {
            // Arrange
            string tableName = "TestTable";

            // Act
            bool isPopulated = dbManager.IsTablePopulated(tableName);

            // Assert
            Assert.IsFalse(isPopulated);
        }

        [TestMethod]
        public void CreateUser_Success()
        {
            // Arrange
            string query = "INSERT INTO USER (UserName, Password, FirstName, LastName, Email, TimeOfRegistration, Score, Tokens) VALUES (@UserName, @Password, @FirstName, @LastName, @Email, @TimeOfRegistration, 0, 50)";
            var parameters = new Dictionary<string, object>
            {
                { "@UserName", "testUser" },
                { "@Password", "hashedPassword" },
                { "@FirstName", "Test" },
                { "@LastName", "User" },
                { "@Email", "test@example.com" },
                { "@TimeOfRegistration", System.DateTime.Now }
            };

            // Act
            dbManager.ExecuteNonQuery(query, parameters);

            // Assert
            string selectQuery = "SELECT * FROM USER WHERE UserName = @UserName";
            var selectParameters = new Dictionary<string, object> { { "@UserName", "testUser" } };
            DataTable result = dbManager.ExecuteQuery(selectQuery, selectParameters);
            Assert.AreEqual(1, result.Rows.Count);
        }

        [TestMethod]
        public void ReadUser_Success()
        {
            // Arrange
            string query = "INSERT INTO USER (UserName, Password, FirstName, LastName, Email, TimeOfRegistration, Score, Tokens) VALUES (@UserName, @Password, @FirstName, @LastName, @Email, @TimeOfRegistration, 0, 50)";
            var parameters = new Dictionary<string, object>
            {
                { "@UserName", "testUser" },
                { "@Password", "hashedPassword" },
                { "@FirstName", "Test" },
                { "@LastName", "User" },
                { "@Email", "test@example.com" },
                { "@TimeOfRegistration", System.DateTime.Now }
            };
            dbManager.ExecuteNonQuery(query, parameters);

            // Act
            string selectQuery = "SELECT * FROM USER WHERE UserName = @UserName";
            var selectParameters = new Dictionary<string, object> { { "@UserName", "testUser" } };
            DataTable result = dbManager.ExecuteQuery(selectQuery, selectParameters);

            // Assert
            Assert.AreEqual(1, result.Rows.Count);
            Assert.AreEqual("testUser", result.Rows[0]["UserName"]);
        }

        [TestMethod]
        public void UpdateUser_Success()
        {
            // Arrange
            CreateUser_Success(); // Ensure the user exists
            string updateQuery = "UPDATE USER SET FirstName = @FirstName, LastName = @LastName WHERE UserName = @UserName";
            var updateParameters = new Dictionary<string, object>
            {
                { "@FirstName", "Updated" },
                { "@LastName", "User" },
                { "@UserName", "testUser" }
            };

            // Act
            dbManager.ExecuteNonQuery(updateQuery, updateParameters);

            // Assert
            string selectQuery = "SELECT * FROM USER WHERE UserName = @UserName";
            var selectParameters = new Dictionary<string, object> { { "@UserName", "testUser" } };
            DataTable result = dbManager.ExecuteQuery(selectQuery, selectParameters);
            Assert.AreEqual(1, result.Rows.Count);
            Assert.AreEqual("Updated", result.Rows[0]["FirstName"]);
            Assert.AreEqual("User", result.Rows[0]["LastName"]);
        }

        [TestMethod]
        public void DeleteUser_Success()
        {
            // Arrange
            CreateUser_Success(); // Ensure the user exists
            string deleteQuery = "DELETE FROM USER WHERE UserName = @UserName";
            var deleteParameters = new Dictionary<string, object> { { "@UserName", "testUser" } };

            // Act
            dbManager.ExecuteNonQuery(deleteQuery, deleteParameters);

            // Assert
            string selectQuery = "SELECT * FROM USER WHERE UserName = @UserName";
            var selectParameters = new Dictionary<string, object> { { "@UserName", "testUser" } };
            DataTable result = dbManager.ExecuteQuery(selectQuery, selectParameters);
            Assert.AreEqual(0, result.Rows.Count);
        }
    }
}


