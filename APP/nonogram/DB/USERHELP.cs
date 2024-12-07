using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nonogram.DB
{
    public class USERHELP
    {
        public string UserName { get; set; }
        public int H1 { get; set; }
        public int H3 { get; set; }
        public int H8 { get; set; }
        public int H13 { get; set; }
        public int L1 { get; set; }
        public int L3 { get; set; }
        public int Check3H { get; set; }
        public int Erase { get; set; }


        public USERHELP() { }

        public USERHELP(string csvLine)
        {
            var fields = csvLine.Split(';');
            UserName = fields[0];
            H1 = int.Parse(fields[1]);
            H3 = int.Parse(fields[2]);
            H8 = int.Parse(fields[3]);
            H13 = int.Parse(fields[4]);
            L1 = int.Parse(fields[5]);
            L3 = int.Parse(fields[6]);
            Check3H = int.Parse(fields[7]);
            Erase = int.Parse(fields[8]);
        }
    }
}
