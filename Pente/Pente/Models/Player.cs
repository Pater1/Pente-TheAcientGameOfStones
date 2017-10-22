using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pente.Models
{
    [Serializable]
    public class Player : INotifyPropertyChanged
    {
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

        public Player()
        {
            Captures = 0;
        }

        public string PlayerName { get; set; }





        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}