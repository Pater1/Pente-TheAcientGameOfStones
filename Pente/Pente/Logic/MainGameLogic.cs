using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
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
        private bool isGameOver = false;
        private bool p1Tria = false;
        private bool p2Tria = false;
        private bool p1Tessera = false;
        private bool p2Tessera = false;

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
            Player1 = new Player();
            Player2 = new Player();
            CurrentPlayer = Player1;
            gameScreen = new GameScreen(TheWindow);
            gameScreen.Player1Status.PlayerCaptures.DataContext = Player1;
            gameScreen.Player2Status.PlayerCaptures.DataContext = Player2;
            TheWindow.MainGrid.Children.Add(gameScreen);
            StartGame();
        }

        private void StartGame()
        {
            gameScreen.CreatePenteBoard(19, 19);
        }

        public void SwitchPlayerTurn()
        {
            if (CurrentPlayer.Captures >= 5)
            {
                isGameOver = true;
            }
            else if (!isGameOver)
            {
                CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player1;
                MessageBox.Show($"{MoveCount}");
                moveCount++;
                return;
            }
            MessageBox.Show("Gameover");
        }

        public void CheckCapture(int x, int y)
        {
            CheckCaptureLeft(x, y);
            CheckCaptureUp(x, y);
            CheckCaptureRight(x, y);
            CheckCaptureDown(x, y);
            CheckCaptureDLDiagonal(x, y);
            CheckCaptureULDiagonal(x, y);
            CheckCaptureDRDiagonal(x, y);
            CheckCaptureURDiagonal(x, y);
        }

        #region Captures
        private void CheckCaptureLeft(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            if (x < 3) return;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y, x - 1].CurrentState == StoneState.Open) return;
            if (stones[y, x - 1].CurrentState == currentState) return;
            if (stones[y, x - 2].CurrentState == currentState) return;
            if (stones[y, x - 3].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y, x - 1].CurrentState = StoneState.Open;
            stones[y, x - 2].CurrentState = StoneState.Open;
        }
        private void CheckCaptureUp(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            if (y < 3) return;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y - 1, x].CurrentState == StoneState.Open) return;
            if (stones[y - 1, x].CurrentState == currentState) return;
            if (stones[y - 2, x].CurrentState == currentState) return;
            if (stones[y - 3, x].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y - 1, x].CurrentState = StoneState.Open;
            stones[y - 2, x].CurrentState = StoneState.Open;
        }
        private void CheckCaptureRight(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            if (x > stones.GetLength(1) - 4) return;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y, x + 1].CurrentState == StoneState.Open) return;
            if (stones[y, x + 1].CurrentState == currentState) return;
            if (stones[y, x + 2].CurrentState == currentState) return;
            if (stones[y, x + 3].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y, x + 1].CurrentState = StoneState.Open;
            stones[y, x + 2].CurrentState = StoneState.Open;
        }
        private void CheckCaptureDown(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            if (y > stones.GetLength(0) - 4) return;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y + 1, x].CurrentState == StoneState.Open) return;
            if (stones[y + 1, x].CurrentState == currentState) return;
            if (stones[y + 2, x].CurrentState == currentState) return;
            if (stones[y + 3, x].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y + 1, x].CurrentState = StoneState.Open;
            stones[y + 2, x].CurrentState = StoneState.Open;
        }
        private void CheckCaptureDLDiagonal(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            if (x < 3 || y > stones.GetLength(0) - 4) return;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y + 1, x - 1].CurrentState == StoneState.Open) return;
            if (stones[y + 1, x - 1].CurrentState == currentState) return;
            if (stones[y + 2, x - 2].CurrentState == currentState) return;
            if (stones[y + 3, x - 3].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y + 1, x - 1].CurrentState = StoneState.Open;
            stones[y + 2, x - 2].CurrentState = StoneState.Open;

        }
        private void CheckCaptureULDiagonal(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            if (x < 3 || y < 3) return;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y - 1, x - 1].CurrentState == StoneState.Open) return;
            if (stones[y - 1, x - 1].CurrentState == currentState) return;
            if (stones[y - 2, x - 2].CurrentState == currentState) return;
            if (stones[y - 3, x - 3].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y - 1, x - 1].CurrentState = StoneState.Open;
            stones[y - 2, x - 2].CurrentState = StoneState.Open;

        }
        private void CheckCaptureDRDiagonal(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            if (x > stones.GetLength(1) - 4 || y > stones.GetLength(0) - 4) return;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y + 1, x + 1].CurrentState == StoneState.Open) return;
            if (stones[y + 1, x + 1].CurrentState == currentState) return;
            if (stones[y + 2, x + 2].CurrentState == currentState) return;
            if (stones[y + 3, x + 3].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y + 1, x + 1].CurrentState = StoneState.Open;
            stones[y + 2, x + 2].CurrentState = StoneState.Open;

        }
        private void CheckCaptureURDiagonal(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            if (x < stones.GetLength(1) - 4 || y < 3) return;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (stones[y - 1, x + 1].CurrentState == StoneState.Open) return;
            if (stones[y - 1, x + 1].CurrentState == currentState) return;
            if (stones[y - 2, x + 2].CurrentState == currentState) return;
            if (stones[y - 3, x + 3].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y - 1, x + 1].CurrentState = StoneState.Open;
            stones[y - 2, x + 2].CurrentState = StoneState.Open;

        }
        #endregion

        public void CheckFiveInRow(int x, int y)
        {
            int vert = CheckRowVertical(x, y);
            if (vert < 5)
            {
                int hori = CheckRowHorizontal(x, y);
                if (hori < 5)
                {
                    int dialr = CheckRowDiagonalLR(x, y);
                    if (dialr < 5)
                    {
                        int diarl = CheckRowDiagonalRL(x, y);
                        if (diarl < 5) 
                        {
                            return;
                        }
                    }
                }
            }
            isGameOver = true;
        }
        private int CheckRowVertical(int x, int y)
        {
            int count = 0;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            for (int i = 0; i < 5; i++)
            {
                if (y - i < 0)
                {
                    break;
                }
                else
                {
                    if (stones[y - i, x].CurrentState == currentState)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                if (y + i > stones.GetLength(0)-1)
                {
                    break;
                }
                else
                {
                    if (stones[y + i, x].CurrentState == currentState)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return count;
        }
        private int CheckRowHorizontal(int x, int y)
        {
            int count = 0;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            for (int i = 0; i < 5; i++)
            {
                if (x - i < 0)
                {
                    break;
                }
                else
                {
                    if (stones[y, x - i].CurrentState == currentState)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                if (x + i > stones.GetLength(1) - 1)
                {
                    break;
                }
                else
                {
                    if (stones[y, x + i].CurrentState == currentState)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return count;
        }
        private int CheckRowDiagonalLR(int x, int y)
        {
            int count = 0;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            for (int i = 0; i < 5; i++)
            {
                if (x + i > stones.GetLength(1) - 1 || y - i < 0)
                {
                    break;
                }
                else
                {
                    if (stones[y - i, x + i].CurrentState == currentState)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                if (x - i < 0 || y + i > stones.GetLength(1) - 1)
                {
                    break;
                }
                else
                {
                    if (stones[y + i, x - i].CurrentState == currentState)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return count;
        }
        private int CheckRowDiagonalRL(int x, int y)
        {
            int count = 0;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            for (int i = 0; i < 5; i++)
            {
                if (x - i < 0 || y - i < 0)
                {
                    break;
                }
                else
                {
                    if (stones[y - i, x - i].CurrentState == currentState)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                if (x + i > stones.GetLength(1) - 1 || y + i > stones.GetLength(0) - 1)
                {
                    break;
                }
                else
                {
                    if (stones[y + i, x + i].CurrentState == currentState)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return count;
        }

        private bool CheckHorizontalTria(int x, int y)
        {
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            bool tria = false;
            if (x == 0 || x == stones.GetLength(1) - 1) tria = false;

            if (x - 3 < 0 || x + 3 >= stones.GetLength(1)) tria = false; //TODO: GOING TO BREAK. CALLED AT 9:40  -- place 2 from edge
            if (stones[y, x - 1].CurrentState == currentState)
            {
                if (stones[y,x - 2].CurrentState == StoneState.Open)
                {
                    if (stones[y,x +1].CurrentState == currentState)
                    {
                        if (stones[y,x+2].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            }
            else if (stones[y, x - 1].CurrentState == StoneState.Open)
            {
                if (stones[y, x + 1].CurrentState == currentState)
                {
                    if (stones[y, x + 2].CurrentState == currentState)
                    {
                        if (stones[y, x + 3].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            }
            else if (stones[y, x + 1].CurrentState == StoneState.Open)
            {
                if (stones[y, x - 1].CurrentState == currentState)
                {
                    if (stones[y, x - 2].CurrentState == currentState)
                    {
                        if (stones[y, x - 3].CurrentState == currentState)
                        {
                            if (stones[y, x - 4].CurrentState == StoneState.Open)
                            {
                                tria = true;
                            }
                        }
                    }
                }
            }
            return tria;
        }
        private bool CheckRightTria(int x, int y)
        {
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            if (x == 0 || x == stones.GetLength(1) - 1) return false;
            if (x - 1 < 0 || x + 3 >= stones.GetLength(1)) return false;
            if (stones[y, x - 1].CurrentState != StoneState.Open) return false;
            if (stones[y, x + 1].CurrentState != currentState) return false;
            if (stones[y, x + 2].CurrentState != currentState) return false;
            if (stones[y, x + 3].CurrentState != StoneState.Open) return false;
            return true;
        }
        private bool CheckUpTria(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (y == 0 || y == stones.GetLength(0) - 1) return false;
            if (y - 3 < 0 || y + 1 >= stones.GetLength(0)) return false;
            if (stones[y + 1, x].CurrentState != StoneState.Open) return false;
            if (stones[y - 1, x].CurrentState != currentState) return false;
            if (stones[y - 2, x].CurrentState != currentState) return false;
            if (stones[y - 3, x].CurrentState != StoneState.Open) return false;
            return true;
        }
        private bool CheckDownTria(int x, int y)
        {
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            if (y == 0 || y == stones.GetLength(0) - 1) return false;
            if (y - 1 < 0 || y + 3 >= stones.GetLength(1)) return false;
            if (stones[y - 1, x].CurrentState != StoneState.Open) return false;
            if (stones[y + 1, x].CurrentState != currentState) return false;
            if (stones[y + 2, x].CurrentState != currentState) return false;
            if (stones[y + 3, x].CurrentState != StoneState.Open) return false;
            return true;
        }
        //TODO: DIAGONAL TRIA
    }
}
