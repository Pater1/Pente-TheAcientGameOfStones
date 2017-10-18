using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pente.Models;
using Pente.UserControls;

namespace Pente.Logic
{
    public class MainGameLogic
    {
        public MainWindow TheWindow { get; set; }
        public Player CurrentPlayer { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public MainGameLogic(MainWindow window)
        {
            TheWindow = window;
        }

        public void ChangeScreenToGameScreen()
        {
            TheWindow.Height = MainWindow.GameScreenHeightConst;
            TheWindow.MainGrid.Children.Clear();
            TheWindow.MainGrid.Children.Add(new GameScreen(TheWindow));
            Player1 = new Player();
            Player2 = new Player();
            CurrentPlayer = Player1;
        }
    }

}
