using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Pente.UserControls
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainWindow TheWindow { get; set; }
        public MainMenu()
        {
            InitializeComponent();
        }
        public MainMenu(MainWindow window)
        {
            InitializeComponent();
            TheWindow = window;
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Player1Name.Text) && !string.IsNullOrEmpty(Player2Name.Text))
            {

                if (int.TryParse(GridXSize.Text, out int x) && int.TryParse(GridYSize.Text, out int y))
                {
                    if (x > 8 && x < 40)
                    {
                        if (x % 2 == 1)
                        {
                            if (y > 8 && y < 40)
                            {
                                if (y % 2 == 1)
                                {
                                    TheWindow.Logic.ChangeScreenToGameScreen(x, y, Player1Name.Text, Player2Name.Text);
                                }
                                else
                                {
                                    if (MainGameLogic.ThrowErrorInsteadOfMessageBox)
                                    {
                                        throw new Exception("The height of the grid must be odd.");
                                    }
                                    MessageBox.Show("The height of the grid must be odd.");
                                }
                            }
                            else
                            {
                                if (MainGameLogic.ThrowErrorInsteadOfMessageBox)
                                {
                                    throw new Exception("The height of the grid must be at least 9 and at most 39.");
                                }
                                MessageBox.Show("The height of the grid must be at least 9 and at most 39.");
                            }
                        }
                        else
                        {
                            if (MainGameLogic.ThrowErrorInsteadOfMessageBox)
                            {
                                throw new Exception("The width of the grid must be odd.");
                            }
                            MessageBox.Show("The width of the grid must be odd.");
                        }
                    }
                    else
                    {
                        if (MainGameLogic.ThrowErrorInsteadOfMessageBox)
                        {
                            throw new Exception("The width of the grid must be at least 9 and at most 39.");
                        }
                        MessageBox.Show("The width of the grid must be at least 9 and at most 39.");
                    }
                }
                else
                {
                    TheWindow.Logic.ChangeScreenToGameScreen(19, 19, Player1Name.Text, Player2Name.Text);
                }
            }
            else
            {
                if (MainGameLogic.ThrowErrorInsteadOfMessageBox)
                {
                    throw new Exception("Both players need names.");
                }
                MessageBox.Show("Both players need names.");
            }
        }

        private void Rules_Click(object sender, RoutedEventArgs e)
        {
            string rules = "";
            rules += "Objective: \nWin by placing 5 or more stones in a row or by capturing 5 or more pairs\n\t of enemy stones.\n";
            rules += "How To Play: \nYou can get five in a row or capture a pair in any horizontal, vertical, or\n\t diagonal pattern.\n";
            rules += "You can only place stones on intersections that are not already controlled by\n\t the other player.\n";
            rules +=
                "The first player is alway black and must place his or her first stone on the\n\t center intersection. This players next turn must place their next\n\t stone at least 3 intersections away. White can place a stone\n\t wherever they wish.";
            rules += "Capturing:\nYou capture a pair of stones by surrounding them with your own,\n\t e.g. BWWB or WBBW (B = Black Stone; W = White Stone)\n";
            rules += "\t More than one capture can be made at once as long as the pair of\n\t opponent pieces are surrounded.\n";
            rules +=
                "When a player has 3 stones in a row that are unblocked, there will be a status\n\t label in the right side that will stay for 3 rounds.\n";
            rules += "If four stones have at least one side unblocked, there will also be an alert for 3\n\t turns.\n";
            if (MainGameLogic.ThrowErrorInsteadOfMessageBox)
            {
                throw new Exception(rules);
            }
            else
            {
                MessageBox.Show(rules);    
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            TheWindow.Close();
        }

        private void GridXSize_TextChanged(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void GridYSize_TextChanged(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
