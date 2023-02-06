using Assets.Scripts.Units;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Levels
{
    public class LevelModel : ILevelModel
    {
        private int width;

        private int height;

        private Tuple<bool, UnitType>[,] units;

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
        public Tuple<bool, UnitType> TryGetUnit(int xPosition, int yPosition)
        {
            // TODO: Validate inputs.
            return units[xPosition, yPosition];
        }

        public void PutUnit(Tuple<int, int> position, bool isWhite, UnitType playerUnit)
        {
            // TODO: Validate inputs.
            units[position.Item1, position.Item2] = new Tuple<bool, UnitType>(isWhite, playerUnit);
        }

        public void TryMoveUnit(Tuple<int, int> fromPosition, Tuple<int, int> toPosition)
        {
            // TODO: Validate inputs.
            UnityEngine.Debug.Log("From: " + fromPosition.Item1 + ", " + fromPosition.Item2);
            UnityEngine.Debug.Log("To: " + toPosition.Item1 + ", " + toPosition.Item2);
            if (units[fromPosition.Item1, fromPosition.Item2] != null && units[toPosition.Item1, toPosition.Item2] == null)
            {
                units[toPosition.Item1, toPosition.Item2] = units[fromPosition.Item1, fromPosition.Item2];
            }
        }

        public LevelModel(int gridWidth, int gridHeight, Dictionary<Tuple<int, int>, Tuple<bool, UnitType>> playerUnits = null)
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

            units = new Tuple<bool, UnitType>[width, height];
            if (playerUnits != null)
            {
                // Place the units onto level:
                for (int x = 0; x < gridWidth; x++)
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        Tuple<int, int> positionKey = new(x, y);
                        if (playerUnits.ContainsKey(positionKey))
                        {
                            units[x, y] = playerUnits[positionKey];
                        }
                    }
                }
            }
        }
    }
}
