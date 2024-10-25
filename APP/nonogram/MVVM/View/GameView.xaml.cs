using System.Windows.Controls;
using nonogram.MVVM.ViewModel;

namespace nonogram.MVVM.View
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();
        }

        public void SetViewModel(GameViewModel viewModel)
        {
            DataContext = viewModel;

        }
    }
}
