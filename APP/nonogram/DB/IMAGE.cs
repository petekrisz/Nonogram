namespace nonogram.DB
{
    public class IMAGE
    {
        public int IMAGEId { get; set; }
        public string Title { get; set; }
        public int IMAGERows { get; set; }
        public int IMAGEColumns { get; set; }
        public string Category { get; set; }
        public string CategoryLogo { get; set; }
        public string Content { get; set; }
        public int Score { get; set; }
        public int ColourType { get; set; }

        public IMAGE() { }

        public IMAGE(string csvLine)
        {
            var fields = csvLine.Split(';');
            IMAGEId = int.Parse(fields[0]);
            Title = fields[1];
            IMAGERows = int.Parse(fields[2]);
            IMAGEColumns = int.Parse(fields[3]);
            Category = fields[4];
            CategoryLogo = fields[5];
            Content = fields[6];
            Score = int.Parse(fields[7]);
            ColourType = int.Parse(fields[8]);
        }
    }
}
