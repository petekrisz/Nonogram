namespace nonogram.DB
{
    public class USERIMAGE
    {
        public string UserName { get; set; }
        public int IMAGEId { get; set; }
        public bool Finished { get; set; }
        public string Content { get; set; }

        public USERIMAGE() { }

        public USERIMAGE(string csvLine)
        {
            var fields = csvLine.Split(';');
            UserName = fields[0];
            IMAGEId = int.Parse(fields[1]);
            Finished = bool.Parse(fields[2]);
            Content = fields[3];
        }
    }
}
