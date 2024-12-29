using nonogram.DB;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public static class UserService
{
    private static USER _currentUser;

    public static USER CurrentUser
    {
        get { return _currentUser; }
        set { _currentUser = value; }
    }

    public static void LoadCurrentUser(string email)
    {
        var filePath = "USER.csv"; // A helyes elérési útvonal

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Nem található a következő fájl: {filePath}");
        }

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(';');
            if (parts.Length >= 5 && parts[4] == email)
            {
                _currentUser = new USER
                {
                    UserName = parts[0],
                    Password = parts[1],
                    FirstName = parts[2],
                    LastName = parts[3],
                    Email = parts[4],
                    TimeOfRegistration = DateTime.Parse(parts[5]),
                    Score = int.Parse(parts[6]),
                    Tokens = int.Parse(parts[7]),
                    Avatar = parts[8]
                };
                break;
            }
        }
    }

    public static void UpdateCurrentUserTokens(int tokenChange)
    {
        if (_currentUser == null) return;

        _currentUser.Tokens += tokenChange;
        var dbManager = new DbManager();
        string updateQuery = $"UPDATE USER SET Tokens = @Tokens WHERE Email = @Email";
        var parameters = new Dictionary<string, object>
        {
            {"@Tokens", _currentUser.Tokens},
            {"@Email", _currentUser.Email}
        };
        dbManager.ExecuteNonQuery(updateQuery, parameters);
    }
}
