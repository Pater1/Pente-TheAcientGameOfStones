using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;

namespace Pente.Models
{
    public class Stone : Label
    {

        private StoneState currentState;

        public StoneState CurrentState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                if (currentState != StoneState.Open)
                {
                    BorderThickness = new Thickness(1);
                    BorderBrush = new SolidColorBrush(Colors.Black);
                }
                Background = currentState.GetBrush();
            }
        }

        public Stone()
        {
            CurrentState = StoneState.Open;
            Foreground = new SolidColorBrush(Colors.Transparent);
        }

    }
}
