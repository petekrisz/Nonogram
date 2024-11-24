using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

namespace nonogram.DB
{
    public class DataStorage
    {
        public ObservableCollection<IMAGE> ImageTableList { get; private set; }

        public DataStorage(string csvFileName)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "DB", csvFileName);
            ImageTableList = new ObservableCollection<IMAGE>(IMAGE.LoadFromCsv(fullPath, Encoding.Default).ToList());
        }

    }
}
