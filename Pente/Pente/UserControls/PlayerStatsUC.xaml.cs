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

namespace Pente.UserControls
{
    /// <summary>
    /// Interaction logic for PlayerStatsUC.xaml
    /// </summary>
    public partial class PlayerStatsUC : UserControl, INotifyPropertyChanged
    {
        private int playerNumber;

        public int PlayerNumber
        {
            get { return playerNumber; }
            set
            {
                playerNumber = value;
                OnPropertyChanged();
            }
        }
        private int captures;

        public int Captures
        {
            get { return captures; }
            set
            {
                captures = value;
                OnPropertyChanged();
            }
        } 

        public PlayerStatsUC()
        {
            InitializeComponent();
            DataContext = this;
            Captures = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
