using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Levels
{
    public class Levels 
    { 
        
        /// <summary>
        /// A method representing the tutorial level's data.
        /// </summary>
        /// <returns>Returns the tutorial level's data.</returns>
        public static LoadLevelData TutorialLevel()
        {
            int _width = 5;
            int _height = 8;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var position = new Tuple<int, int>(x, y);
                    if (x == 2 && y == 3)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Triangle));
                    }

                    if (x == 0 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 3 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }

                    // enemies
                    if (x == 3 && y == _height - 4 || x == 3 && y == _height - 3)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }
                }
            }
            return new LoadLevelData()
            {
                Width = _width,
                Height = _height,
                Units = units
            };
        }

        public static LoadLevelData TutorialFogOfWarLevel()
        {
            int _width = 5;
            int _height = 8;       // Display the visibility of 2 triangles: 1 per team
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var position = new Tuple<int, int>(x, y);
                    if (x == 0 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }
                    if (x == 3 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }
                    if (x == 2 && y == 1)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    // Enemies
                    if (x == 3 && y == 4 || x == 1 && y == 6)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }
                    
                    if (x == 2 && y == 5)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Triangle));
                    }
                }
            }
            return new LoadLevelData()
            {
                Width = _width,
                Height = _height,
                Units = units
            };
        }

        public static LoadLevelData ChallengeCircle()
        {
            int _width = 5;
            int _height = 6;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Tuple<int, int> position = new(x, y);
                    if (x == 2 && y == 3)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Triangle));
                    }

                    if (x == 0 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 3 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }

                    if (x == 3 && y == 4)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }
                    if (x == 3 && y == 5)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }
                }
            }
            return new LoadLevelData()
            {
                Width = _width,
                Height = _height,
                Units = units
            };
        }

        public static LoadLevelData ChallengeScout()
        {
            int _width = 5;
            int _height = 7;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Tuple<int, int> position = new(x, y);
                    if (x == 2 && y == 3)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if (x == 1 && y == 3)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 3 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }

                    if (x == 0 && y == 0 || x == 4 && y == 6)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }
                }
            }
            return new LoadLevelData()
            {
                Width = _width,
                Height = _height,
                Units = units
            };
        }

        /// <summary>
        /// A method representing the first level's data.
        /// </summary>
        /// <returns>Returns the first level's data.</returns>
        public static LoadLevelData LevelOne()
        {
            int _width = 5;
            int _height = 10;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Tuple<int, int> position = new(x, y);

                    if (x == 3 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if (x == 2 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Triangle));
                    }

                    if (x == 2 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 3 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }

                    if (x == 2 && y == 7)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Triangle));

                    }

                    if (x == 2 && y == _height - 1)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

                    if (x == 3 && y == _height - 1)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Diamond));
                    }

                    if (x == 2 && y == 6)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }
                }
            }
            return new LoadLevelData()
            {
                Width = _width,
                Height = _height,
                Units = units
            };
        }

        public static LoadLevelData LevelTwo()
        {
            int _width = 8;
            int _height = 10;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Tuple<int, int> position = new(x, y);

                    if (x == 3 && y == 3)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if (x == 2 && y == 2 || x == _width - 3 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Triangle));
                    }

                    if (x == 0 && y == 0 || x == _width - 1 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 3 && y == 0 || x == 4 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }

                    if (x == 2 && y == _height - 3 || x == _width - 3 && y == _height - 3)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Triangle));

                    }

                    if (x == 0 && y == _height - 1 || x == _width - 1 && y == _height - 1)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

                    if (x == 3 && y == _height - 1 || x == 4 && y == _height - 1)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Diamond));
                    }

                    if (x == 3 && y == 7)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }
                }
            }
            return new LoadLevelData()
            {
                Width = _width,
                Height = _height,
                Units = units
            };
        }
    }
}