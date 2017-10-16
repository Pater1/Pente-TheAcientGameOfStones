using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pente.UserControls;

namespace Pente.Logic
{
    public class MainGameLogic
    {
        public MainWindow TheWindow { get; set; }
        public MainGameLogic(MainWindow window)
        {
            TheWindow = window;
        }

        public void StartNewGame()
        {
            TheWindow.Height = 525;
            TheWindow.MainGrid.Children.Clear();
            TheWindow.MainGrid.Children.Add(new GameScreen(TheWindow));
        }
    }

}
