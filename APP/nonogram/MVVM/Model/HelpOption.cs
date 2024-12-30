using nonogram.Common;

namespace nonogram.MVVM.Model
{
    public class HelpOption : ObservableObject
    {
        private string _value;
        private bool _isChecked;
        private int _amount;


        public string TypeOfHelp { get; set; }
        public int Price { get; set; }
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
        public int Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }
        public string Description => GetHelpDescription(TypeOfHelp);

        private string GetHelpDescription(string typeOfHelp)
        {
            switch (typeOfHelp)
            {
                case "H1":
                    return "Reveals the selected cell.";
                case "H3":
                    return "Reveals 3 hidden or incorrectly marked cells at random.";
                case "H8":
                    return "Reveals 8 hidden or incorrectly marked cells at random.";
                case "H13":
                    return "Reveals 13 cells around the selected cell in diamond shape.";
                case "L1":
                    return "Reveals all cells in the selected row or column.";
                case "L3":
                    return "Reveals all cells in the selected and neighbouring rows or columns.";
                case "Check3H":
                    return "Checks the whole grid, if it finds errors, it fixes up to 3 of them.";
                case "Erase":
                    return "Erases all wrong guesses.";
                default:
                    return "";
            }
        }
    }
}
