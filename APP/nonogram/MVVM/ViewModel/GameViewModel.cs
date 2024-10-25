using nonogram.Common;
using nonogram.DB;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace nonogram.MVVM.ViewModel
{
    public class GameViewModel : ObservableObject
    {
        public ObservableCollection<ButtonModel> Buttons { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public GameViewModel(IMAGE image)
        {
            Rows = image.Rows;
            Columns = image.Columns;
            Buttons = new ObservableCollection<ButtonModel>();

            //checking the length of the content:
            int expectedLength = Rows * Columns;
            if (image.Content.Length != expectedLength)
            {
                throw new System.Exception($"A kép contentjével gond van. Ennyi: {image.Content.Length}, miközben ennyi kellene legyen: {expectedLength}");

            }


                for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Buttons.Add(new ButtonModel
                    {
                        Row = i,
                        Column = j,
                        Content = image.Content[i * Columns + j].ToString(),
                        Command = new RelayCommand<object> (ChangeButtonState)
                    });
                }
            }
        }

        private void ChangeButtonState(object parameter)
        {
            if (parameter is ButtonModel button)
            {
                switch (button.State)
                {
                    case ButtonState.Default:
                        button.State = ButtonState.Black;
                        button.Content = string.Empty;
                        break;
                    case ButtonState.Black:
                        button.State = ButtonState.X;
                        button.Content = "X";
                        break;
                    case ButtonState.X:
                        button.State = ButtonState.Question;
                        button.Content = "?";
                        break;
                    case ButtonState.Question:
                        button.State = ButtonState.Default;
                        button.Content = string.Empty;
                        break;
                }
            }
        }
    }

    public class ButtonModel : ObservableObject
    {
        private ButtonState _state;
        public ButtonState State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public int Row { get; set; }
        public int Column { get; set; }
        public string Content { get; set; }
        public ICommand Command { get; set; }
    }

    public enum ButtonState
    {
        Default,
        Black,
        X,
        Question
    }

}
