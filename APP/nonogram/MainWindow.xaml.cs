using nonogram.DB;
using nonogram.MVVM.View;
using nonogram.MVVM.ViewModel;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace nonogram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private string username = "netuddki"; // Hardcoded for now
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitSelectorWindow exitSelector = new ExitSelectorWindow
            {
                Owner = this // Set the owner to the main window
            };
            exitSelector.ShowDialog();
        }
    }
}
