using System;

namespace Assets.Scripts.Analytics
{
    public class PieceData
    {
        /// <summary>
        /// The player which owns the piece.
        /// </summary>
        public string Player;

        /// <summary>
        /// The type of piece.
        /// </summary>
        public string Piece;

        /// <summary>
        /// The (x, y) position the piece is located.
        /// </summary>
        public Tuple<int, int> Position;
    }
}
