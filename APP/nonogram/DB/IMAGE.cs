using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace nonogram.DB
{
    public class IMAGE
    {
        public int idIMAGE;
        public string Title;
        public int Rows;
        public int Columns;
        public string Category;
        public string CategoryLogo;
        public string Content;
        public int Score;
        public int ColourType;

        public IMAGE(string line)
        {

            var splitted = line.Split(';');
            idIMAGE = int.Parse(splitted[0]);
                    Title = splitted[1];
                    Rows = int.Parse(splitted[2]);
                    Columns = int.Parse(splitted[3]);
                    Category = splitted[4];
                    CategoryLogo = splitted[5];
                    Content = splitted[6];
                    Score = int.Parse(splitted[7]);
                    ColourType = int.Parse(splitted[8]);
        }

        public static IEnumerable<IMAGE> LoadFromCsv(string fileName, Encoding e)
        {
            foreach (var item in File.ReadAllLines(fileName, e).ToList().Skip(1))
            {
                yield return new IMAGE(item);
            }
        }

    }
}
