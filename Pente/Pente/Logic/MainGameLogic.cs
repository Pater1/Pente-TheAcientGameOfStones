using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using Microsoft.Win32;
using Pente.Models;
using Pente.UserControls;

namespace Pente.Logic
{
    public class MainGameLogic
    {
        public MainWindow TheWindow { get; set; }
        public static bool ThrowErrorInsteadOfMessageBox = false;
        private int moveCount;
        public int MoveCount => moveCount;
        private GameScreen gameScreen;
        private string _filePath;
        private bool isGameOver;
        private int turnsSinceTriaOrTessera = 0;
        private bool _canSave;
        private Thread t;
        private Dispatcher d;
        private const int timerMax = 5000;
        private int timerCur;
        public Player CurrentPlayer { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public MainGameLogic(MainWindow window)
        {
            TheWindow = window;
            timerCur = timerMax;
            d = Dispatcher.CurrentDispatcher;
            t = new Thread(() =>
            {
                while (true)
                {
                    while (timerCur >= 0)
                    {
                        Thread.Sleep(1);
                        if (gameScreen != null)
                        {
                            gameScreen.Time = timerCur / 1000;
                        }
                        timerCur--;
                    }
                    try
                    {
                        d.Invoke(TimerFinished);
                    }
                    catch
                    {
                        break;
                    }
                }
            });
        }

        public void ChangeScreenToGameScreen(int width, int height,string player1Name, string player2Name)
        {
            
            TheWindow.Height = MainWindow.GameScreenHeightConst;
            TheWindow.MainGrid.Children.Clear();
            Player1 = new Player{PlayerName = player1Name};
            Player2 = new Player{PlayerName = player2Name};
            CurrentPlayer = Player1;
            gameScreen = new GameScreen(TheWindow);
            gameScreen.Player1Status.PlayerCaptures.DataContext = Player1;
            gameScreen.Player2Status.PlayerCaptures.DataContext = Player2;
            TheWindow.MainGrid.Children.Add(gameScreen);
            StartGame(height,width);
        }

        private void StartGame(int height, int width)
        {
            gameScreen.CreatePenteBoard(height, width);
            _canSave = true;
            t.Start();
        }

        public void SwitchPlayerTurn()
        {
            timerCur = timerMax;
            if (CurrentPlayer.Captures >= 5)
            {
                isGameOver = true;
            }
            else if (!isGameOver)
            {
                CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player1;
                moveCount++;
                turnsSinceTriaOrTessera++;
                if (turnsSinceTriaOrTessera >= 4)
                {
                    gameScreen.GameStatusLabel.Content = $"Game in Progress";
                }
                return;
            }
            EndGame();
        }

        private void EndGame()
        {
            TheWindow.Height = 350;
            TheWindow.Width = 250;
            TheWindow.MainGrid.Children.Clear();
            TheWindow.MainGrid.Children.Add(new GameOverScreen(TheWindow)
            {
                Winner = CurrentPlayer.PlayerName,
                StyleOfWin = CurrentPlayer.Captures >= 5 ? "5+ captures" : "5 Stones in a row"
            });
            timerCur = -1;
        }
        
        public void SaveGame()
        {
            if (_canSave)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    InitialDirectory = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Trim(@"file:\".ToCharArray()).Remove(0, 3)) + @"\saves"}",
                    Title = "Browse Files",
                    DefaultExt = ".pnt",
                    Filter = "Pente files (*.pnt)|*.pnt",
                    FilterIndex = 2,
                    RestoreDirectory = true
                };
                sfd.ShowDialog();
                _filePath = sfd.FileName;
            }
            if (_filePath.EndsWith(".pnt"))
            {
                StoneSaveTemplate[,] stones = new StoneSaveTemplate[gameScreen.Stones.GetLength(0), gameScreen.Stones.GetLength(1)];
                for (int i = 0; i < stones.GetLength(0); i++)
                {
                    for (int j = 0; j < stones.GetLength(1); j++)
                    {
                        stones[i,j] = new StoneSaveTemplate{State = gameScreen.Stones[i,j].CurrentState};
                    }
                }
                SaveFile file = new SaveFile(stones,Player1,Player2,CurrentPlayer,moveCount,gameScreen.GameStatusLabel.Content.ToString());
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(_filePath,
                    FileMode.Create,
                    FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, file);
                stream.Close();
            }
        }

        public void LoadGame()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Trim(@"file:\".ToCharArray()).Remove(0, 3)) + @"\saves"}",
                Title = "Browse Files",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".pnt",
                Filter = "Pente files (*.pnt)|*.pnt",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            ofd.ShowDialog();
            string path = ofd.FileName;
            if (path.EndsWith(".pnt"))
            {
                _canSave = true;
                try
                {
                    IFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(path,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read);
                    SaveFile file = (SaveFile)formatter.Deserialize(stream);
                    stream.Close();

                    TheWindow.Height = MainWindow.GameScreenHeightConst;
                    TheWindow.MainGrid.Children.Clear();
                    gameScreen = new GameScreen(TheWindow);

                    Player1 = file.Player1;
                    gameScreen.Player1Status.PlayerCaptures.DataContext = Player1;
                    gameScreen.Player1Status.PlayerName = Player1.PlayerName;
                    Player2 = file.Player2;
                    gameScreen.Player2Status.PlayerCaptures.DataContext = Player2;
                    gameScreen.Player2Status.PlayerName = Player2.PlayerName;
                    CurrentPlayer = file.CurrentPlayer;
                    moveCount = file.MoveCount;
                    StoneSaveTemplate[,] stones = file.Stones;
                    int height = stones.GetLength(0);
                    int width = stones.GetLength(1);

                    gameScreen.GameGrid.Columns = width;
                    gameScreen.GameGrid.Rows = height;
                    gameScreen.Stones = new Stone[height,width];
                    Stone[,] newStones = gameScreen.Stones;
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            Stone newGuy = new Stone
                            {
                                CurrentState = stones[i, j].State,
                                Content = $"{i},{j}",
                                FontSize = 6
                            };
                            newStones[i, j] = newGuy;
                            gameScreen.AddStone(newStones[i,j]);
                        }
                    }
                    TheWindow.MainGrid.Children.Add(gameScreen);
                    gameScreen.GameStatusLabel.Content = file.GameStatusMessage;
                    timerCur = timerMax;
                }
                catch (Exception e)
                {
                    if (ThrowErrorInsteadOfMessageBox)
                    {
                        throw new Exception("Invalid or Currupted Save File -- Unable to load.");
                    }
                    else
                    {
                        MessageBox.Show("Invalid or Currupted Save File -- Unable to load.");
                    }
                }
            }
        }

        private void TimerFinished()
        {
            if (ThrowErrorInsteadOfMessageBox)
            {
                throw new Exception($"{CurrentPlayer.PlayerName} took too long to take a turn.\nSkipping their turn...");
            }
            else
            {
                MessageBox.Show($"{CurrentPlayer.PlayerName} took too long to take a turn.\nSkipping their turn...");
            }
            SwitchPlayerTurn();
        }

        #region Captures
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
        private void CheckCaptureLeft(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            if (x < 3) return;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (x - 1 < 0) return;
            if (stones[y, x - 1].CurrentState == currentState || stones[y, x - 1].CurrentState == StoneState.Open) return;
            if (x - 2 < 0) return;
            if (stones[y, x - 2].CurrentState == currentState || stones[y, x - 2].CurrentState == StoneState.Open) return;
            if (x - 3 < 0) return;
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
            if (y  - 1 < 0) return;
            if (stones[y - 1, x].CurrentState == currentState || stones[y - 1, x].CurrentState == StoneState.Open) return;
            if (y  - 2 < 0) return;
            if (stones[y - 2, x].CurrentState == currentState || stones[y - 2, x].CurrentState == StoneState.Open) return;
            if (y  - 3 < 0) return;
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
            if (x + 1 > stones.GetLength(1) - 1) return;
            if (stones[y, x + 1].CurrentState == currentState || stones[y, x + 1].CurrentState == StoneState.Open) return;
            if (x + 2 > stones.GetLength(1) - 1) return;
            if (stones[y, x + 2].CurrentState == currentState || stones[y, x + 2].CurrentState == StoneState.Open) return;
            if (x + 3 > stones.GetLength(1) - 1) return;
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
            if (y + 1 > stones.GetLength(0) - 1) return;
            if (stones[y + 1, x].CurrentState == currentState || stones[y + 1, x].CurrentState == StoneState.Open) return;
            if (y + 2 > stones.GetLength(0) - 1) return;
            if (stones[y + 2, x].CurrentState == currentState || stones[y + 2, x].CurrentState == StoneState.Open) return;
            if (y + 3 > stones.GetLength(0) - 1) return;
            if (stones[y + 3, x].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y + 1, x].CurrentState = StoneState.Open;
            stones[y + 2, x].CurrentState = StoneState.Open;
        }
        private void CheckCaptureDLDiagonal(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (x - 1 < 0 || y + 1 > stones.GetLength(0) - 1) return;
            if (stones[y + 1, x - 1].CurrentState == currentState || stones[y + 1, x - 1].CurrentState == StoneState.Open) return;
            if (x - 2 < 0 || y + 2 > stones.GetLength(0) - 1) return;
            if (stones[y + 2, x - 2].CurrentState == currentState || stones[y + 2, x - 2].CurrentState == StoneState.Open) return;
            if (x - 3 < 0 || y + 3 > stones.GetLength(0) - 1) return;
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
            if (x  - 1 < 0 || y - 1 < 0) return;
            if (stones[y - 1, x - 1].CurrentState == currentState || stones[y - 1, x - 1].CurrentState == StoneState.Open) return;
            if (x  - 2 < 0 || y - 2 < 0) return;
            if (stones[y - 2, x - 2].CurrentState == currentState || stones[y - 2, x - 2].CurrentState == StoneState.Open) return;
            if (x  - 3 < 0 || y - 3 < 0) return;
            if (stones[y - 3, x - 3].CurrentState != currentState || stones[y - 3, x - 3].CurrentState == StoneState.Open) return;
            CurrentPlayer.Captures++;
            stones[y - 1, x - 1].CurrentState = StoneState.Open;
            stones[y - 2, x - 2].CurrentState = StoneState.Open;

        }
        private void CheckCaptureDRDiagonal(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            if (x > stones.GetLength(1) - 4 || y > stones.GetLength(0) - 4) return;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (x  + 1 > stones.GetLength(1) - 1 || y + 1 > stones.GetLength(0) - 1) return;
            if (stones[y + 1, x + 1].CurrentState == currentState || stones[y + 1, x + 1].CurrentState == StoneState.Open) return;
            if (x  + 2 > stones.GetLength(1) - 1 || y + 2 > stones.GetLength(0) - 1) return;
            if (stones[y + 2, x + 2].CurrentState == currentState || stones[y + 2, x + 2].CurrentState == StoneState.Open) return;
            if (x  + 3 > stones.GetLength(1) - 1 || y + 3 > stones.GetLength(0) - 1) return;
            if (stones[y + 3, x + 3].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y + 1, x + 1].CurrentState = StoneState.Open;
            stones[y + 2, x + 2].CurrentState = StoneState.Open;

        }
        private void CheckCaptureURDiagonal(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            if (x  + 1 > stones.GetLength(1) - 1 || y - 1 < 0) return;
            if (stones[y - 1, x + 1].CurrentState == currentState || stones[y - 1, x + 1].CurrentState == StoneState.Open) return;
            if (x  + 2 > stones.GetLength(1) - 1 || y - 1 < 0) return;
            if (stones[y - 2, x + 2].CurrentState == currentState || stones[y - 2, x + 2].CurrentState == StoneState.Open) return;
            if (x  + 3 > stones.GetLength(1) - 1 || y - 1 < 0) return;
            if (stones[y - 3, x + 3].CurrentState != currentState) return;
            CurrentPlayer.Captures++;
            stones[y - 1, x + 1].CurrentState = StoneState.Open;
            stones[y - 2, x + 2].CurrentState = StoneState.Open;

        }
        #endregion

        #region FiveInARow Validation
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
            for (int i = 1; i < 5; i++)
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
            for (int i = 1; i < 5; i++)
            {
                if (x + i >= stones.GetLength(1) - 1)
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
            for (int i = 1; i < 5; i++)
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
            for (int i = 1; i < 5; i++)
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
        #endregion

        #region Tria Validation
        public void CheckTrias(int x, int y)
        {
            if (CheckHorizontalTria(x, y) || CheckVerticalTria(x, y) || CheckDiagonalTria(x, y))
            {
                string currentPlayer = CurrentPlayer == Player1 ? "Player1" : "Player2";
                gameScreen.GameStatusLabel.Content = $"{currentPlayer} has a Tria!";
                turnsSinceTriaOrTessera = 0;
            }
        }
        private bool CheckHorizontalTria(int x, int y)
        {
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            bool tria = false;
            if (x == 0 || x == stones.GetLength(1) - 1) tria = false;
            if (x - 1 >= 0  && stones[y, x - 1].CurrentState == currentState)
            {
                if (x - 2 >= 0 && stones[y,x - 2].CurrentState == StoneState.Open)
                {
                    if (x + 1 <= stones.GetLength(1) - 1 && stones[y, x + 1].CurrentState == currentState)
                    {
                        if (x + 2 <= stones.GetLength(1) - 1 && stones[y, x + 2].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
                else if (x - 2 >= 0 && stones[y, x - 2].CurrentState == currentState)
                {
                    if (x - 3 >= 0 && stones[y, x - 3].CurrentState == StoneState.Open)
                    {
                        if (x + 1 <= stones.GetLength(1) - 1 && stones[y, x + 1].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            }
            else if (x - 1 >= 0 && stones[y, x - 1].CurrentState == StoneState.Open)
            {
                if (x + 1 <= stones.GetLength(1) - 1 && stones[y, x + 1].CurrentState == currentState)
                {
                    if (x + 2 <= stones.GetLength(1) - 1 && stones[y, x + 2].CurrentState == currentState)
                    {
                        if (x + 3 <= stones.GetLength(1) - 1 && stones[y, x + 3].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            }
            else if (x + 1 <= stones.GetLength(1) - 1 && stones[y, x + 1].CurrentState == StoneState.Open)
            {
                if (x - 1 >= 0 && stones[y, x - 1].CurrentState == currentState)
                {
                    if (x - 2 >= 0 && stones[y, x - 2].CurrentState == currentState)
                    {
                        if (x - 3 >= 0 && stones[y, x - 3].CurrentState == currentState)
                        {
                            if (x - 4 >= 0 && stones[y, x - 4].CurrentState == StoneState.Open)
                            {
                                tria = true;
                            }
                        }
                    }
                }
            }
            return tria;
        }
        private bool CheckVerticalTria(int x, int y)
        {
            Stone[,] stones = gameScreen.Stones;
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            bool tria = false;
            if (y == 0 || x == stones.GetLength(0) - 1) tria = false;
            if (y - 1 >= 0 && stones[y - 1, x].CurrentState == currentState)
            {
                if (y - 2 >= 0 && stones[y - 2, x].CurrentState == StoneState.Open)
                {
                    if (y + 1 <= stones.GetLength(0) - 1 && stones[y + 1, x].CurrentState == currentState)
                    {
                        if (y + 2 <= stones.GetLength(0) - 1 && stones[y + 2, x].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
                else if (y - 2 >= 0 && stones[y - 2, x].CurrentState == currentState)
                {
                    if (y - 3 >= 0 && stones[y - 3, x].CurrentState == StoneState.Open)
                    {
                        if (y + 1 <= stones.GetLength(1) - 1 && stones[y + 1, x].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            }
            else if (y - 1 >= 0 && stones[y - 1, x].CurrentState == StoneState.Open)
            {
                if (y + 1 <= stones.GetLength(0) - 1 && stones[y + 1, x].CurrentState == currentState)
                {
                    if (y + 2 <= stones.GetLength(0) - 1 && stones[y + 2, x].CurrentState == currentState)
                    {
                        if (y + 3 <= stones.GetLength(0) - 1 && stones[y + 3, x].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            }
            else if (y + 1 <= stones.GetLength(0) - 1 && stones[y + 1, x].CurrentState == StoneState.Open)
            {
                if (y - 1 >= 0 && stones[y - 1, x].CurrentState == currentState)
                {
                    if (y - 2 >= 0 && stones[y - 2, x].CurrentState == currentState)
                    {
                        if (y - 3 >= 0 && stones[y - 3, x].CurrentState == currentState)
                        {
                            if (y - 4 >= 0 && stones[y - 4, x].CurrentState == StoneState.Open)
                            {
                                tria = true;
                            }
                        }
                    }
                }
            }
            return tria;
        }
        private bool CheckDiagonalTria(int x, int y)
        {
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            bool tria = false;
            if (x - 1 >= 0 && y - 1 >= 0 && stones[y - 1, x - 1].CurrentState == StoneState.Open)
            {
                if (x + 1 <= stones.GetLength(1) - 1 && y - 1 <= stones.GetLength(0) - 1 &&
                    stones[y + 1, x + 1].CurrentState == StoneState.Open)
                {
                    tria = CheckDiagDLtoURTria(currentState, stones, x, y);
                }
                else if (x + 1 <= stones.GetLength(1) - 1 && y + 1 <= stones.GetLength(0) - 1 &&
                         stones[y + 1, x + 1].CurrentState == currentState)
                {
                    if (x + 2 <= stones.GetLength(1) - 1 && y + 2 <= stones.GetLength(0) - 1 &&
                        stones[y + 2, x + 2].CurrentState == currentState)
                    {
                        if (x + 3 <= stones.GetLength(1) - 1 && y + 3 <= stones.GetLength(0) - 1 &&
                            stones[y + 3, x + 3].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            }
            else if (x + 1 <= stones.GetLength(1) - 1 && y + 1 <= stones.GetLength(0) - 1 &&
                     stones[y + 1, x + 1].CurrentState == StoneState.Open)
            {
                if (x - 1 >= 0 && y - 1 >= 0 && stones[y - 1, x - 1].CurrentState == currentState)
                {
                    if (x - 2 >= 0 && y - 2 >= 0 && stones[y - 2, x - 2].CurrentState == currentState)
                    {
                        if (x - 3 >= 0 && y - 3 >= 0 && stones[y - 3, x - 3].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            } else if (x + 1 <= stones.GetLength(1) - 1 && y + 1 <= stones.GetLength(0) - 1 &&
                       stones[y + 1, x + 1].CurrentState == currentState)
            {
                if (x - 1 >= 0 && y - 1 >= 0 && stones[y - 1, x - 1].CurrentState == currentState)
                {
                    if (x - 2 >= 0 && y - 2 >= 0 && stones[y - 2, x - 2].CurrentState == StoneState.Open)
                    {
                        if (x + 2 <= stones.GetLength(1) - 1 && y + 2 <= stones.GetLength(0) - 1 &&
                            stones[y + 2, x + 2].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            }
            return tria;
        }
        private bool CheckDiagDLtoURTria(StoneState currentState, Stone[,] stones, int x, int y)
        {
            bool tria = false;
            int maxYBound = stones.GetLength(0) - 1;
            int maxXBound = stones.GetLength(1) - 1;
            if (x + 1 <= maxXBound && y - 1 >= 0 && stones[y - 1, x + 1].CurrentState == StoneState.Open)
            {
                if (x - 1 >= 0 && y + 1 <= maxYBound && stones[y + 1, x - 1].CurrentState == currentState)
                {
                    if (x - 2 >= 0 && y + 2 <= maxYBound && stones[y + 2, x - 2].CurrentState == currentState)
                    {
                        if (x - 3 >= 0 && y + 3 <= maxYBound && stones[y + 3, x - 3].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            }
            else if (x - 1 >= 0 && y + 1 <= maxYBound && stones[y + 1, x - 1].CurrentState == StoneState.Open)
            {
                if (x + 1 <= maxXBound && y - 1 >= 0 && stones[y - 1, x + 1].CurrentState == currentState)
                {
                    if (x + 2 <= maxXBound && y - 2 >= 0 && stones[y - 2, x + 2].CurrentState == currentState)
                    {
                        if (x + 3 <= maxXBound && y - 3 >= 0 && stones[y - 3, x + 3].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            } else if (x + 1 <= maxXBound && y - 1 >= 0 && stones[y - 1, x + 1].CurrentState == currentState)
            {
                if (x - 1 >= 0 && y + 1 <= maxYBound && stones[y + 1, x - 1].CurrentState == currentState)
                {
                    if (x - 2 >= 0 && y + 2 <= maxYBound && stones[y + 2, x - 2].CurrentState == StoneState.Open)
                    {
                        if (x + 2 <= maxXBound && y - 2 >= 0 && stones[y - 2, x + 2].CurrentState == StoneState.Open)
                        {
                            tria = true;
                        }
                    }
                }
            }
            return tria;
        }
        #endregion

        #region Tessera Validation
        public void CheckTessera(int x, int y)
        {
            if (CheckHorizontalTessera(x, y) || 
                CheckVerticalTessera(x, y) ||
                CheckULtoDRTessera(x, y) ||
                CheckDLtoURTessera(x, y))
            {
                string currentPlayer = CurrentPlayer == Player1 ? "Player1" : "Player2";
                gameScreen.GameStatusLabel.Content = $"{currentPlayer} has a Tessera!";
                turnsSinceTriaOrTessera = 0;
            }
        }
        private bool CheckHorizontalTessera(int x, int y)
        {
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            bool tessera = false;
            int maxYValue = stones.GetLength(0) - 1;
            int maxXValue = stones.GetLength(1) - 1;
            bool startedT = false;
            int count = 0;
            for (var i = 0; i < maxXValue; i++)
            {
                if (stones[y, i].CurrentState == currentState)
                {
                    if (!startedT && i - 1 >= 0 && stones[y, i - 1].CurrentState == StoneState.Open)
                    {
                        startedT = true;
                    }
                    if (startedT)
                    {
                        count++;
                    }
                }
                else if (stones[y, i].CurrentState == StoneState.Open)
                {
                    if (count == 4 && i - 1 >= 0 && stones[y, i - 1].CurrentState == currentState)
                    {
                        tessera = true;
                    }
                    else
                    {
                        startedT = false;
                        count = 0;
                    }
                }
                else
                {
                    startedT = false;
                    count = 0;
                }
            }
            return tessera;
        }
        private bool CheckVerticalTessera(int x, int y)
        {
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            bool tessera = false;
            int maxYValue = stones.GetLength(0) - 1;
            int maxXValue = stones.GetLength(1) - 1;
            bool startedT = false;
            int count = 0;
            for (var i = 0; i < maxYValue; i++)
            {
                if (stones[i, x].CurrentState == currentState)
                {
                    if (!startedT && i - 1 >= 0 && stones[i- 1, x].CurrentState == StoneState.Open)
                    {
                        startedT = true;
                    }
                    if (startedT)
                    {
                        count++;
                    }
                }
                else if (stones[i, x].CurrentState == StoneState.Open)
                {
                    if (count == 4 && i - 1 >= 0 && stones[i - 1, x].CurrentState == currentState)
                    {
                        tessera = true;
                    }
                    else
                    {
                        startedT = false;
                        count = 0;
                    }
                }
                else
                {
                    startedT = false;
                    count = 0;
                }
            }
            return tessera;
        }
        private bool CheckULtoDRTessera(int x, int y)
        {
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            bool tessera = true;
            int maxYValue = stones.GetLength(0) - 1;
            int maxXValue = stones.GetLength(1) - 1;
            int disFromCurPoint = 0;

            while (CheckBounds(x - disFromCurPoint, y - disFromCurPoint) && stones[y - disFromCurPoint, x - disFromCurPoint].CurrentState == currentState)
            {
                disFromCurPoint++;
            }

            disFromCurPoint--;
            int startX = x - disFromCurPoint;
            int startY = y - disFromCurPoint;
            for (int i = 0; i < 4; i++)
            {
                if (CheckBounds(startX + i, startY + i) && stones[startY + i, startX + i].CurrentState != currentState)
                {
                    tessera = false;
                    break;
                }
            }

            if (!CheckBounds(startX - 1, startY - 1) ||
                (CheckBounds(startX - 1, startY - 1) && stones[startY - 1, startX - 1].CurrentState != StoneState.Open))
            {
                if (!CheckBounds(startX + 4, startY + 4) ||
                    (CheckBounds(startX + 4, startY + 4) && stones[startY + 4, startX + 4].CurrentState != StoneState.Open))
                {
                    tessera = false;
                }
            }
            return tessera;
        }
        private bool CheckDLtoURTessera(int x, int y)
        {
            StoneState currentState = CurrentPlayer == Player1 ? StoneState.Black : StoneState.White;
            Stone[,] stones = gameScreen.Stones;
            bool tessera = true;
            int maxYValue = stones.GetLength(0) - 1;
            int maxXValue = stones.GetLength(1) - 1;
            int disFromCurPoint = 0;

            while (CheckBounds(x - disFromCurPoint, y + disFromCurPoint) && stones[y + disFromCurPoint, x - disFromCurPoint].CurrentState == currentState)
            {
                disFromCurPoint++;
            }

            disFromCurPoint--;
            int startX = x - disFromCurPoint;
            int startY = y + disFromCurPoint;
            for (int i = 0; i < 4; i++)
            {
                if (CheckBounds(startX + i, startY - i) && stones[startY - i, startX + i].CurrentState != currentState)
                {
                    tessera = false;
                    break;
                }
            }

            if (!CheckBounds(startX - 1, startY + 1) ||
                (CheckBounds(startX - 1, startY + 1) && stones[startY + 1, startX - 1].CurrentState != StoneState.Open))
            {
                if (!CheckBounds(startX + 4, startY - 4) ||
                    (CheckBounds(startX + 4, startY - 4) && stones[startY - 4, startX + 4].CurrentState != StoneState.Open))
                {
                    tessera = false;
                }
            }
            return tessera;
        }
        private bool CheckBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < gameScreen.Stones.GetLength(1) && y < gameScreen.Stones.GetLength(0);
        }
        #endregion

    }
}
