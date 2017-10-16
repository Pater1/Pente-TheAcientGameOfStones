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

namespace Pente.UserControls
{
    /// <summary>
    /// Interaction logic for PlayerStatsUC.xaml
    /// </summary>
    public partial class PlayerStatsUC : UserControl
    {
        private int playerNumber;

        public int PlayerNumber
        {
            get { return playerNumber; }
            set
            {
                playerNumber = value;
                MainLabel.Content = $"Player {playerNumber} Stats:";
            }
        }

        public PlayerStatsUC()
        {
            InitializeComponent();
        }
    }
}
