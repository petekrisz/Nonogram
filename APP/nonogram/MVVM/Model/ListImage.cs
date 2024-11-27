using nonogram.DB;


namespace nonogram.MVVM.Model
{
    public class ListImage
    {
        public string ImageSource { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDetails { get; set; }
        public IMAGE Image { get; set; }
    }
}
