using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Pente.Models
{
    [Serializable]
    public enum StoneState
    {
        Open,
        White,
        Black
    }

    public static class StoneStateExtension
    {
        private static readonly Dictionary<StoneState,Brush> StoneStateToColor = new Dictionary<StoneState, Brush>()
        {
            [StoneState.Open] = ((Func<Brush>)(() => (Brush) Application.Current.Resources["BoardBrush"]))(),
            [StoneState.White] = ((Func<SolidColorBrush>)(() => (SolidColorBrush)Application.Current.Resources["StoneBrushWhite"]))(),
            [StoneState.Black] = ((Func<SolidColorBrush>)(() => (SolidColorBrush)Application.Current.Resources["StoneBrushBlack"]))()
        };
        public static Brush GetBrush(this StoneState state)
        {
            return StoneStateToColor[state];
        }
    }
}