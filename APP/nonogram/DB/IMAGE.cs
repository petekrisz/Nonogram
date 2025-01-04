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
        public string RowFinished { get; set; }
        public string ColumnFinished { get; set; }

        public IMAGE() { }

        public IMAGE(string csvLine)
        {
            var fields = csvLine.Split(';');
            Title = fields[0];
            IMAGERows = int.Parse(fields[1]);
            IMAGEColumns = int.Parse(fields[2]);
            Category = fields[3];
            CategoryLogo = fields[4];
            Content = fields[5];
            Score = int.Parse(fields[6]);
            ColourType = int.Parse(fields[7]);
            RowFinished = fields[8];
            ColumnFinished = fields[9];
        }
    }
}
