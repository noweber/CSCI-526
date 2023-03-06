using System;
using System.Collections.Generic;

namespace Assets.Scripts.Analytics
{
    /// <summary>
    /// This class stores all of the data needed for replaying the game (used for analytics).
    /// </summary>
    public class LevelReplayData
    {
        /// <summary>
        /// This field tracks the (x, y) positions of pieces on the game board at the start of the level.
        /// </summary>
        public HashSet<PieceData> InitialPiecePositions { get; private set; }

        /// <summary>
        /// This field is the set of all moves made during the this level of the game.
        /// </summary>
        public List<MoveData> MovesMade { get; private set; }

        public string VictoriousPlayerName;

        public LevelReplayData()
        {
            InitialPiecePositions = new HashSet<PieceData>();
            MovesMade = new List<MoveData>();
        }
    }
}
