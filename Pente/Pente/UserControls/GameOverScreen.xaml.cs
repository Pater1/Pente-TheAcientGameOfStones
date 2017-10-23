using System;
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
using Pente.Logic;
using Pente.Models;

namespace Pente.UserControls
{
    /// <summary>
    /// Interaction logic for GameOverScreen.xaml
    /// </summary>
    public partial class GameOverScreen : UserControl, INotifyPropertyChanged
    {
        public MainWindow TheWindow { get; set; }
        public GameOverScreen()
        {
            InitializeComponent();
        }

        public GameOverScreen(MainWindow window)
        {
            InitializeComponent();
            TheWindow = window;
            DataContext = this;
        }

        private string winner;

        public string Winner
        {
            get { return winner; }
            set
            {
                winner = value;
                OnPropertyChanged();
            }
        }
        private string style;

        public string StyleOfWin
        {
            get { return style; }
            set
            {
                style = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            TheWindow.Logic = new MainGameLogic(TheWindow);
            TheWindow.MainGrid.Children.Clear();
            TheWindow.Height = 825;
            TheWindow.Width = 725;
            TheWindow.MainGrid.Children.Add(new MainMenu(TheWindow));
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            TheWindow.Close();
        }
    }
}
