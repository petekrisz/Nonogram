using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using IniParser;
using IniParser.Model;

namespace nonogram.DB
{
    public class DbManager
    {
        private string ConnectionStringWithoutDb; // = "Server=localhost;Port=3306;Uid=root;Pwd=;";
        private string ConnectionStringWithDb; // = "Server=localhost;Port=3306;Database=nonogram;Uid=root;Pwd=;";
        private static bool isDatabaseInitialized = false;

        public DbManager()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");

            string server = data["Database"]["Server"];
            string port = data["Database"]["Port"];
            string username = data["Database"]["Username"];
            string password = data["Database"]["Password"];

            ConnectionStringWithoutDb = $"Server={server};Port={port};Uid={username};Pwd={password};";
            ConnectionStringWithDb = $"Server={server};Port={port};Database=nonogram;Uid={username};Pwd={password};";

            if (!isDatabaseInitialized)
            {
                InitializeDatabaseAndTables();
                isDatabaseInitialized = true;
            }
        }

        /// <summary>
        /// Creates the database, tables, and populates data from CSV files.
        /// </summary>
        public void InitializeDatabaseAndTables(bool populateFromCsv = true)
        {
            CreateDatabase();
            CreateTables();
            if (populateFromCsv) PopulateTablesFromCsv();
        }

        /// <summary>
        /// Ensures the database exists.
        /// </summary>
        private void CreateDatabase()
        {
            using (var connection = new MySqlConnection(ConnectionStringWithoutDb))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "CREATE DATABASE IF NOT EXISTS nonogram CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Creates all tables for the application.
        /// </summary>
        private void CreateTables()
        {
            using (var connection = new MySqlConnection(ConnectionStringWithDb))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = GetTableCreationSql();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Returns the SQL for creating all necessary tables.
        /// </summary>
        private string GetTableCreationSql()
        {
            return @"
        -- Drop all tables first to remove any dependencies
        DROP TABLE IF EXISTS USERIMAGE;
        DROP TABLE IF EXISTS USERHELP;
        DROP TABLE IF EXISTS HELP;
        DROP TABLE IF EXISTS IMAGE;
        DROP TABLE IF EXISTS USER;

        -- Create tables in dependency order
        CREATE TABLE IF NOT EXISTS USER (
            UserName VARCHAR(50) PRIMARY KEY,
            Password VARCHAR(255),
            FirstName VARCHAR(50),
            LastName VARCHAR(50),
            Email VARCHAR(100),
            TimeOfRegistration DATETIME,
            Score INT,
            Tokens INT,
            Avatar VARCHAR(255) NULL
        );

        CREATE TABLE IF NOT EXISTS IMAGE (
            IMAGEId INT PRIMARY KEY AUTO_INCREMENT,
            Title VARCHAR(50),
            IMAGERows INT,
            IMAGEColumns INT,
            Category VARCHAR(50),
            CategoryLogo VARCHAR(255),
            Content TEXT,
            Score INT,
            ColourType INT,
            RowFinished VARCHAR(100),
            ColumnFinished VARCHAR(100)
        );

        CREATE TABLE IF NOT EXISTS HELP (
            TypeOfHelp VARCHAR(50) PRIMARY KEY,
            Price INT,
            Weight DOUBLE,
            HelpLogoG VARCHAR(50),
            HelpLogoL VARCHAR(50)
        );

        CREATE TABLE IF NOT EXISTS USERHELP (
            UserName VARCHAR(50) PRIMARY KEY,
            H1 INT,
            H3 INT,
            H8 INT,
            H13 INT,
            L1 INT,
            L3 INT,
            Check3H INT,
            Erase INT,
            FOREIGN KEY (UserName) REFERENCES USER(UserName)
        );

        CREATE TABLE IF NOT EXISTS USERIMAGE (
            UserName VARCHAR(50),
            IMAGEId INT,
            Finished BOOLEAN,
            Content TEXT,
            PRIMARY KEY (UserName, IMAGEId),
            FOREIGN KEY (UserName) REFERENCES USER(UserName),
            FOREIGN KEY (IMAGEId) REFERENCES IMAGE(IMAGEId)
        );";
        }
        /// <summary>
        /// Populates tables from corresponding CSV files in the DB folder.
        /// </summary>
        private void PopulateTablesFromCsv()
        {
            foreach (var tableName in new[] { "USER", "IMAGE", "HELP", "USERHELP", "USERIMAGE" })
            {
                if (IsTablePopulated(tableName)) continue;

                var filePath = $"DB/{tableName}.csv";
                if (!File.Exists(filePath)) continue;

                var csvLines = File.ReadLines(filePath).Skip(1); // Skip header
                foreach (var line in csvLines)
                {
                    InsertRecordFromCsvLine(tableName, line);
                }
            }
        }

        /// <summary>
        /// Checks status of tables to avoid doubl operations.
        /// </summary>
        private bool IsTablePopulated(string tableName)
        {
            string query = $"SELECT COUNT(*) FROM {tableName}";
            var result = ExecuteQuery(query);
            return result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0][0]) > 0;
        }

        /// <summary>
        /// Inserts a record into the specified table from a CSV line.
        /// </summary>
        private void InsertRecordFromCsvLine(string tableName, string csvLine)
        {
            //Debug.WriteLine($"Inserting record into table {tableName} from CSV line: {csvLine}");
            try
            {
                var className = $"nonogram.DB.{tableName}";
                var type = Assembly.GetExecutingAssembly().GetType(className) ?? throw new Exception($"Class {className} not found for table {tableName}.");

                var record = Activator.CreateInstance(type, csvLine);

                var properties = type.GetProperties();
                var columnNames = string.Join(", ", properties.Select(p => p.Name));
                var parameterNames = string.Join(", ", properties.Select(p => $"@{p.Name}"));

                var parameters = properties.ToDictionary(
                    prop => $"@{prop.Name}",
                    prop => prop.GetValue(record)
                );

                ExecuteNonQuery($"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames});", parameters);
            }
            catch (Exception ex)
            {
                // Log the error (to a file, database, or console)
                //Console.WriteLine($"Error inserting record into table {tableName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Executes a non-query SQL command.
        /// </summary>
        public void ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            //Debug.WriteLine($"Executing SQL: {query}");
            using (var connection = new MySqlConnection(ConnectionStringWithDb))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    AddParameters(cmd, parameters);

                    //// Log query and parameters
                    //Console.WriteLine($"Executing SQL: {cmd.CommandText}");
                    //foreach (MySqlParameter param in cmd.Parameters)
                    //{
                    //    Console.WriteLine($"Param: {param.ParameterName} = {param.Value}");
                    //}

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes a query and returns the result as a DataTable.
        /// </summary>
        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            using (var connection = new MySqlConnection(ConnectionStringWithDb))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    AddParameters(cmd, parameters);

                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        private void AddParameters(MySqlCommand cmd, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
        }

        public IMAGE GetImageById(int imageId)
        {
            string query = "SELECT * FROM IMAGE WHERE IMAGEId = @IMAGEId";
            var parameters = new Dictionary<string, object>
    {
        { "@IMAGEId", imageId }
    };

            var dataTable = ExecuteQuery(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new IMAGE
                {
                    IMAGEId = Convert.ToInt32(row["IMAGEId"]),
                    Title = row["Title"].ToString(),
                    IMAGERows = Convert.ToInt32(row["IMAGERows"]),
                    IMAGEColumns = Convert.ToInt32(row["IMAGEColumns"]),
                    Category = row["Category"].ToString(),
                    CategoryLogo = row["CategoryLogo"].ToString(),
                    Content = row["Content"].ToString(),
                    Score = Convert.ToInt32(row["Score"]),
                    ColourType = Convert.ToInt32(row["ColourType"]),
                    RowFinished = row["RowFinished"].ToString(),
                    ColumnFinished = row["ColumnFinished"].ToString()
                };
            }

            return null;
        }

        public void ExportTableToCsv(string tableName, string filePath)
        {
            string query = $"SELECT * FROM {tableName}";
            DataTable table = ExecuteQuery(query);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write the header, excluding ImageId for the IMAGE table, since it has been changed to AUTO_INCREMENT in the meantime
                var columnNames = table.Columns.Cast<DataColumn>()
                    .Where(column => !(tableName == "IMAGE" && column.ColumnName == "IMAGEId"))
                    .Select(column => column.ColumnName);
                writer.WriteLine(string.Join(";", columnNames));

                // Write the rows, excluding ImageId for the IMAGE table
                foreach (DataRow row in table.Rows)
                {
                    var fields = row.ItemArray
                        .Where((field, index) => !(tableName == "IMAGE" && table.Columns[index].ColumnName == "IMAGEId"))
                        .Select(field => field.ToString());
                    writer.WriteLine(string.Join(";", fields));
                }
            }
        }
    }
}
