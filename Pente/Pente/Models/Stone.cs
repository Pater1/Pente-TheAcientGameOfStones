using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup.Localizer;
using System.Windows.Media;

namespace Pente.Models
{
    public class Stone : Label
    {

        private StoneStates currentState;

        public StoneStates CurrentState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                switch (currentState)
                {
                    case StoneStates.Open:
                        foreach (var resourcesKey in Application.Current.Resources.Keys)
                        {
                            if (resourcesKey.ToString().Equals("BoardColor"))
                            {
                                Object o = Application.Current.Resources[resourcesKey];
                                Background = o is SolidColorBrush ? o as SolidColorBrush : new SolidColorBrush(Colors.BlanchedAlmond) ;
                                break;
                            }
                        }
                        break;
                    case StoneStates.Black:
                        Background = new SolidColorBrush(Colors.Black);
                        break;
                    case StoneStates.White:
                        Background = new SolidColorBrush(Colors.White);
                        break;
                }
            }
        }

        public Stone()
        {
            CurrentState = StoneStates.Open;
        }
    }
}
