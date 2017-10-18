using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Pente.Models;
using Pente.UserControls;

namespace Pente.Logic
{
    public class MainGameLogic
    {
        public MainWindow TheWindow { get; set; }
        private GameScreen gameScreen;
        private int moveCount;
        public int MoveCount => moveCount;

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
            gameScreen = new GameScreen(TheWindow);
            TheWindow.MainGrid.Children.Add(gameScreen);
            Player1 = new Player();
            Player2 = new Player();
            CurrentPlayer = Player1;
            StartGame();
        }

        private void StartGame()
        {
            gameScreen.CreatePenteBoard(19, 19);

        }

        public void SwitchPlayerTurn()
        {
            CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player1;
            MessageBox.Show($"{MoveCount}");
            moveCount++;
        }

        public void CheckCapture(int x, int y)
        {
            if (!CheckBounds(x, y)) return;




        }

        private bool CheckBounds(int x, int y)
        {
            return x >= 0 && x <= gameScreen.Stones.GetLength(1) && y >= 0 && y <= gameScreen.Stones.GetLength(0);
        }
    }

}
