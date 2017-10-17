using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Pente.Models
{
    public enum StoneState
    {
        Open,
        White,
        Black
    }

    public static class StoneStateExtension
    {
        private static readonly Dictionary<StoneState,SolidColorBrush> StoneStateToColor = new Dictionary<StoneState, SolidColorBrush>()
        {
            [StoneState.Open] = ((Func<SolidColorBrush>)(() => (SolidColorBrush) Application.Current.Resources["BoardBrush"]))(),
            [StoneState.White] = ((Func<SolidColorBrush>)(() => (SolidColorBrush)Application.Current.Resources["StoneBrushWhite"]))(),
            [StoneState.Black] = ((Func<SolidColorBrush>)(() => (SolidColorBrush)Application.Current.Resources["StoneBrushBlack"]))()
        };
        public static SolidColorBrush GetBrush(this StoneState state)
        {
            return StoneStateToColor[state];
        }
    }
}