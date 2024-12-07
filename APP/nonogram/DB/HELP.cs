namespace nonogram.DB
{
    public class HELP
    {
        public string TypeOfHelp { get; set; }
        public int Price { get; set; }

        public HELP() { }

        public HELP(string csvLine)
        {
            var fields = csvLine.Split(';');
            TypeOfHelp = fields[0];
            Price = int.Parse(fields[1]);
        }
    }
}
