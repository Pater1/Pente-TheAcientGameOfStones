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
        public static GameScreen ThisScreen { get; private set; }
        public MainWindow TheWindow { get; set; }
        public GameScreen()
        {
            InitializeComponent();
            ThisScreen = this;
        }
        public GameScreen(MainWindow window)
        {
            InitializeComponent();
            TheWindow = window;
            ThisScreen = this;
        }

        public static GameScreen ReturnGameScreen()
        {
            return ThisScreen;
        }
        public void CreatePenteBoard(int rows, int columns)
        {
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

        }
        
        private void StoneOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (sender is Stone stone)
            {
                switch (TheWindow.Logic.MoveCount)
                {
                    case 0:
                    {
                        int rows = Stones.GetLength(0);
                        int columns = Stones.GetLength(1);
                        int centerRow = rows - (rows / 2) - 1;
                        int centerColumn = columns - (columns / 2) - 1;
                        if (stone.Equals(Stones[centerRow, centerColumn]))
                        {
                            stone.CurrentState = StoneState.Black;
                            TheWindow.Logic.SwitchPlayerTurn();
                        }
                        break;
                    }
                    case 2:
                    {
                        int rows = Stones.GetLength(0);
                        int columns = Stones.GetLength(1);
                        int centerRow = rows - (rows / 2) - 1;
                        int centerColumn = columns - (columns / 2) - 1;
                        bool valid = true;
                        for (var i = centerRow - 3; i < centerRow + 4; i++)
                        {
                            for (var j = centerColumn - 3; j < centerColumn + 4; j++)
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
                        TheWindow.Logic.SwitchPlayerTurn();
                        break;
                }
            }
        }
    }
}
