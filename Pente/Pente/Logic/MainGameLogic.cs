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
            int testing = gameScreen.Stones.GetLength(0);
            int testing3 = gameScreen.Stones.GetLength(1);
            if (x < 3)
            {
                CheckRight(x,y);
                if (y < 3)
                {
                    CheckDown(x,y);
                }
                else if (y > gameScreen.Stones.GetLength(0) - 4)
                {
                    CheckUp(x, y);
                }
                else
                {
                    CheckDown(x,y);
                    CheckUp(x,y);
                }
            }
            else if (x > gameScreen.Stones.GetLength(1) - 4)
            {
                CheckLeft(x, y);
                if (y < 3)
                {
                    CheckDown(x, y);
                }
                else if (y > gameScreen.Stones.GetLength(0) - 4)
                {
                    CheckUp(x, y);
                }
                else
                {
                    CheckDown(x, y);
                    CheckUp(x, y);
                }
            }
            else
            {
                CheckLeft(x,y);
                CheckRight(x,y);
                if (y < 3)
                {
                    CheckDown(x, y);
                }
                else if (y > gameScreen.Stones.GetLength(0) - 4)
                {
                    CheckUp(x, y);
                }
                else
                {
                    CheckDown(x, y);
                    CheckUp(x, y);
                }
            }



        }

        private void CheckLeft(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y, x - 1].CurrentState == StoneState.Open) return;
            if (currentState == StoneState.Black)
            {
                if (stones[y, x - 1].CurrentState != StoneState.White) return;
                if (stones[y, x - 2].CurrentState != StoneState.White) return;
                if (stones[y, x - 3].CurrentState != StoneState.Black) return;
                CurrentPlayer.Captures++;
                stones[y, x - 1].CurrentState = StoneState.Open;
                stones[y, x - 2].CurrentState = StoneState.Open;
            }
            else
            {
                if (stones[y, x - 1].CurrentState != StoneState.Black) return;
                if (stones[y, x - 2].CurrentState != StoneState.Black) return;
                if (stones[y, x - 3].CurrentState != StoneState.White) return;
                CurrentPlayer.Captures++;
                stones[y, x - 1].CurrentState = StoneState.Open;
                stones[y, x - 2].CurrentState = StoneState.Open;
            }
        }

        private void CheckUp(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y - 1, x].CurrentState == StoneState.Open) return;
            if (currentState == StoneState.Black)
            {
                if (stones[y - 1, x].CurrentState != StoneState.White) return;
                if (stones[y - 2, x].CurrentState != StoneState.White) return;
                if (stones[y - 3, x].CurrentState != StoneState.Black) return;
                CurrentPlayer.Captures++;
                stones[y - 1, x].CurrentState = StoneState.Open;
                stones[y - 2, x].CurrentState = StoneState.Open;
            }
            else
            {
                if (stones[y - 1, x].CurrentState != StoneState.Black) return;
                if (stones[y - 2, x].CurrentState != StoneState.Black) return;
                if (stones[y - 3, x].CurrentState != StoneState.White) return;
                CurrentPlayer.Captures++;
                stones[y - 1, x].CurrentState = StoneState.Open;
                stones[y - 2, x].CurrentState = StoneState.Open;
            }
        }

        private void CheckRight(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y, x + 1].CurrentState == StoneState.Open) return;
            if (currentState == StoneState.Black)
            {
                if (stones[y, x + 1].CurrentState != StoneState.White) return;
                if (stones[y, x + 2].CurrentState != StoneState.White) return;
                if (stones[y, x + 3].CurrentState != StoneState.Black) return;
                CurrentPlayer.Captures++;
                stones[y, x + 1].CurrentState = StoneState.Open;
                stones[y, x + 2].CurrentState = StoneState.Open;
            }
            else
            {
                if (stones[y, x + 1].CurrentState != StoneState.Black) return;
                if (stones[y, x + 2].CurrentState != StoneState.Black) return;
                if (stones[y, x + 3].CurrentState != StoneState.White) return;
                CurrentPlayer.Captures++;
                stones[y, x + 1].CurrentState = StoneState.Open;
                stones[y, x + 2].CurrentState = StoneState.Open;
            }
        }

        private void CheckDown(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y + 1, x].CurrentState == StoneState.Open) return;
            if (currentState == StoneState.Black)
            {
                if (stones[y + 1, x].CurrentState != StoneState.White) return;
                if (stones[y + 2, x].CurrentState != StoneState.White) return;
                if (stones[y + 3, x].CurrentState != StoneState.Black) return;
                CurrentPlayer.Captures++;
                stones[y + 1, x].CurrentState = StoneState.Open;
                stones[y + 2, x].CurrentState = StoneState.Open;
            }
            else
            {
                if (stones[y + 1, x].CurrentState != StoneState.Black) return;
                if (stones[y + 2, x].CurrentState != StoneState.Black) return;
                if (stones[y + 3, x].CurrentState != StoneState.White) return;
                CurrentPlayer.Captures++;
                stones[y + 1, x].CurrentState = StoneState.Open;
                stones[y + 2, x].CurrentState = StoneState.Open;
            }
        }
    }
}
