using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nonogram.DB
{
    public class USER
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime TimeOfRegistration { get; set; }
        public int Score { get; set; }
        public int Tokens { get; set; }
        public string Avatar { get; set; }

        public USER() { }

        public USER(string csvLine)
        {
            var fields = csvLine.Split(';');
            UserName = fields[0];
            Password = fields[1];
            FirstName = fields[2];
            LastName = fields[3];
            Email = fields[4];
            TimeOfRegistration = DateTime.Parse(fields[5]);
            Score = int.Parse(fields[6]);
            Tokens = int.Parse(fields[7]);
            Avatar = fields[8];
        }
    }
}
