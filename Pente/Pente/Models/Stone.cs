﻿using System;
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
                Background = currentState.GetBrush();
            }
        }

        public Stone()
        {
            CurrentState = StoneState.Open;
        }

    }
}
