using System;

namespace Pente.Models
{
    [Serializable]
    public class SaveFile
    {

        public StoneSaveTemplate[,] Stones { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player CurrentPlayer { get; set; }
        public int MoveCount { get; set; }
        public string GameStatusMessage { get; set; }

        public SaveFile(StoneSaveTemplate[,] stones, Player player1, Player player2, Player currentPlayer, int moveCount, string gameStatusMessage)
        {
            Stones = stones;
            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = currentPlayer;
            MoveCount = moveCount;
            GameStatusMessage = gameStatusMessage;
        }
    }
}