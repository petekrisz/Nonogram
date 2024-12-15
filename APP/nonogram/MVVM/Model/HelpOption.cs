using nonogram.Common;

namespace nonogram.MVVM.Model
{
    public class HelpOption : ObservableObject
    {
        private string _value;
        private bool _isChecked;

        public string TypeOfHelp { get; set; }
        public string HelpLogoG { get; set; }
        public string HelpLogoL { get; set; }
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }
        public string Description => GetHelpDescription(TypeOfHelp);

        private string GetHelpDescription(string typeOfHelp)
        {
            switch (typeOfHelp)
            {
                case "H1":
                    return "Reveals the selected cell.";
                case "H3":
                    return "Reveals 3 hidden or incorrectly\nmarked cells at random.";
                case "H8":
                    return "Reveals 8 hidden or incorrectly\nmarked cells at random.";
                case "H13":
                    return "Reveals 13 cells around the\nselected cell in diamond shape.";
                case "L1":
                    return "Reveals all cells\nin the selected row or column.";
                case "L3":
                    return "Reveals all cells\nin the selected and neighbouring\nrows or columns.";
                case "Check3H":
                    return "Checks the whole grid,\nif it finds errors,\nit fixes up to 3 of them.";
                case "Erase":
                    return "Erases all wrong guesses.";
                default:
                    return "";
            }
        }
    }
}
