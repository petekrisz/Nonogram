namespace nonogram.DB
{
    public class HELP
    {
        public string TypeOfHelp { get; set; }
        public int Price { get; set; }
        public double Weight { get; set; }
        public string HelpLogoG { get; set; }
        public string HelpLogoL { get; set; }

        public HELP() { }

        public HELP(string csvLine)
        {
            var fields = csvLine.Split(';');
            TypeOfHelp = fields[0];
            Price = int.Parse(fields[1]);
            Weight = double.Parse(fields[2]);
            HelpLogoG = fields[3];
            HelpLogoL = fields[4];
        }
    }
}
