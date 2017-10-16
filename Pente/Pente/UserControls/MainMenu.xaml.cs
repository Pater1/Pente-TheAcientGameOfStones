using System;
using System.Collections.Generic;
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

namespace Pente.UserControls
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainWindow TheWindow { get; set; }
        public MainMenu()
        {
            InitializeComponent();
        }
        public MainMenu(MainWindow window)
        {
            InitializeComponent();
            TheWindow = window;
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            TheWindow.SwitchToGame();
            
        }

        private void Rules_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            TheWindow.Close();
        }
    }
}
