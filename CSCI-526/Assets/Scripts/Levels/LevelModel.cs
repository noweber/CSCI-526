using Assets.Scripts.Piece;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Levels
{
    public class LevelModel : ILevelModel
    {
        private int width;

        private int height;

        private Dictionary<Tuple<int, int>, IPiece> units;

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        /// <summary>
        /// Retrieves the unit at the given (x, y) position of the level.
        /// </summary>
        /// <param name="xPosition">The level grid x coordinate.</param>
        /// <param name="yPosition">The level grid y coordinate.</param>
        /// <returns>Returns a tuple. The bool is true when the piece is white, else false. The unit type is the second item of the typle.</returns>
        public IPiece TryGetUnit(Tuple<int, int> position)
        {
            // TODO: Validate inputs.
            if (units.ContainsKey(position))
            {
                return units[position];
            }
            return null;
        }

        public void PutUnit(Tuple<int, int> position, IPiece playerUnit)
        {
            // TODO: Validate inputs.
            if (units.ContainsKey(position))
            {
                units[position] = playerUnit;
            }
            else
            {
                units.Add(position, playerUnit);
            }
        }

        public void TryMoveUnit(Tuple<int, int> fromPosition, Tuple<int, int> toPosition)
        {
            // TODO: Validate inputs.
            UnityEngine.Debug.Log("From: " + fromPosition.Item1 + ", " + fromPosition.Item2);
            UnityEngine.Debug.Log("To: " + toPosition.Item1 + ", " + toPosition.Item2);

            if (units.ContainsKey(fromPosition) && !units.ContainsKey(toPosition))
            {
                units.Add(toPosition, units[fromPosition]);
                units.Remove(fromPosition);
            }
            else if (units.ContainsKey(fromPosition) && units.ContainsKey(toPosition))
            {
                // TODO: handle capture?
            }
        }

        public LevelModel(int gridWidth, int gridHeight, Dictionary<Tuple<int, int>, IPiece> playerUnits = null)
        {
            if (gridWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gridWidth));
            }
            width = gridWidth;

            if (gridHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gridWidth));
            }
            height = gridHeight;

            if (playerUnits == null)
            {
                units = new Dictionary<Tuple<int, int>, IPiece>();
            }
            else
            {
                // TODO: validate that each unit's position is valid within the level.
                units = playerUnits;
            }
        }
    }
}
