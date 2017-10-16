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
using Pente.Logic;
using Pente.UserControls;

namespace Pente {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainGameLogic Logic { get; set; }
        public MainWindow() {
            InitializeComponent();
            Logic = new MainGameLogic(this);
            MainGrid.Children.Add(new MainMenu(this));
        }

        public void SwitchToGame()
        {
            Height = 525;
            MainGrid.Children.Clear();
            MainGrid.Children.Add(new GameScreen(this));
        }
    }
}
