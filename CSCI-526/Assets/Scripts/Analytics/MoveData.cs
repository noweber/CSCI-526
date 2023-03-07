using System;

namespace Assets.Scripts.Analytics
{
    public class MoveData : PieceData
    {
        /// <summary>
        /// The (x, y) position the move went to.
        /// </summary>
        public Tuple<int, int> Destination;
    }
}
