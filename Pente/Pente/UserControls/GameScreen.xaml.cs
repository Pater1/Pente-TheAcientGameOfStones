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
using Pente.Models;

namespace Pente.UserControls
{
    /// <summary>
    /// Interaction logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : UserControl
    {
        public Stone[,] Stones { get; set; }
        public MainWindow TheWindow { get; set; }
        public GameScreen()
        {
            InitializeComponent();
        }
        public GameScreen(MainWindow window)
        {
            InitializeComponent();
            TheWindow = window;
            Stone stone = new Stone();
            stone.MouseLeftButtonDown += (sender, args) =>
            {
                if (stone.CurrentState != StoneState.Open) return;
                if (TheWindow.Logic.CurrentPlayer == TheWindow.Logic.Player1)
                {
                    stone.CurrentState = StoneState.Black;
                }
                else
                {   
                    stone.CurrentState = StoneState.White;
                }
            };
            GameGrid.Children.Add(stone);
        }
    }
}
