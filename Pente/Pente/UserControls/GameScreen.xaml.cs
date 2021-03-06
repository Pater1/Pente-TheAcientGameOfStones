﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class GameScreen : UserControl, INotifyPropertyChanged
    {
        public Stone[,] Stones { get; set; }
        private static GameScreen thisScreen;
        public MainWindow TheWindow { get; set; }
        private int time;

        public int Time
        {
            get { return time; }
            set
            {
                time = value; 
                OnPropertyChanged();
            }
        }

        public GameScreen()
        {
            InitializeComponent();
            thisScreen = this;
        }
        public GameScreen(MainWindow window)
        {
            InitializeComponent();
            TheWindow = window;
            GameTimer.DataContext = this;
            thisScreen = this;
        }

        public static GameScreen ReturnGameScreen()
        {
            return thisScreen;
        }
        public void CreatePenteBoard(int rows, int columns)
        {
            Player1Status.PlayerName = TheWindow.Logic.Player1.PlayerName;
            Player2Status.PlayerName = TheWindow.Logic.Player2.PlayerName;
            GameGrid.Children.Clear();
            GameGrid.Rows = rows;
            GameGrid.Columns = columns;
            Stones = new Stone[rows,columns];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    Stone stone = new Stone();
                    stone.MouseLeftButtonDown += StoneOnMouseLeftButtonDown;
                    stone.Content = $"{i},{j}";
                    stone.FontSize = 6;
                    Stones[i, j] = stone;
                    GameGrid.Children.Add(stone);
                }
            }
            int stoneRows = Stones.GetLength(0);
            int stoneColumns = Stones.GetLength(1);
            int centerRow = stoneRows - (rows / 2) - 1;
            int centerColumn = stoneColumns - (columns / 2) - 1;
            Stones[centerRow, centerColumn].CurrentState = StoneState.Black;
            TheWindow.Logic.SwitchPlayerTurn();

        }

        public void AddStone(Stone stone)
        {
            stone.MouseLeftButtonDown += StoneOnMouseLeftButtonDown;
            GameGrid.Children.Add(stone);

        }
        
        private void StoneOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (sender is Stone stone)
            {
                switch (TheWindow.Logic.MoveCount)
                {
                    case 2:
                    {
                        int rows = Stones.GetLength(0);
                        int columns = Stones.GetLength(1);
                        int centerRow = rows - (rows / 2) - 1;
                        int centerColumn = columns - (columns / 2) - 1;
                        bool valid = true;
                        for (int i = centerRow - 2; i < centerRow + 3; i++)
                        {
                            for (int j = centerColumn - 2; j < centerColumn + 3; j++)
                            {
                                if (!stone.Equals(Stones[i, j])) continue;
                                valid = false;
                                break;
                            }
                            if (!valid)
                            {
                                break;
                            }
                        }
                        if (valid)
                        {
                            stone.CurrentState = StoneState.Black;
                            TheWindow.Logic.SwitchPlayerTurn();
                        }
                        break;
                    }
                    default:
                        if (stone.CurrentState != StoneState.Open) return;
                        stone.CurrentState = TheWindow.Logic.CurrentPlayer == TheWindow.Logic.Player1 ? StoneState.Black : StoneState.White;
                        bool found = false;
                        int foundX = 0;
                        int foundY = 0;
                        for (var i = 0; i < Stones.GetLength(0); i++)
                        {
                            for (var j = 0; j < Stones.GetLength(1); j++)
                            {
                                if (!stone.Equals(Stones[i, j])) continue;
                                foundX = j;
                                foundY = i;
                                found = true;
                                break;
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                        TheWindow.Logic.CheckCapture(foundX,foundY);
                        TheWindow.Logic.CheckFiveInRow(foundX, foundY);
                        TheWindow.Logic.CheckTrias(foundX,foundY);
                        TheWindow.Logic.CheckTessera(foundX,foundY);
                        TheWindow.Logic.SwitchPlayerTurn();
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
