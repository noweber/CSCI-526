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

        public static LoadLevelData TutorialCircle()
        {
            int _width = 7;
            int _height = 7;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var position = new Tuple<int, int>(x, y);

                    if (x == 0 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

                    if (x == 2 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 0 && y == 6)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

                    if (x == 5 && y == 5)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
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

        public static LoadLevelData TutorialCircleAbility()
        {
            int _width = 3;
            int _height = 7;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var position = new Tuple<int, int>(x, y);

                    if (x == 1 && y == 0)
                    {
                        //Add Player Base
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

                    if (x == 1 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x != 1 && y == 2)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Diamond));
                    }

                    if (y == 3)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Diamond));
                    }

                    if (x == 1 && y == 4)
                    {
                        //Add Enemy Base
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
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

        public static LoadLevelData TutorialDiamond()
        {
            int _width = 7;
            int _height = 7;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var position = new Tuple<int, int>(x, y);

                    if (x == 0 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

                    if (x == 2 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }

                    if (x == 3 && y == 6)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }

                    if (x == 5 && y == 1)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
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

        public static LoadLevelData TutorialDiamondAbility()
        {
            int _width = 7;
            int _height = 7;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var position = new Tuple<int, int>(x, y);

                    if (x == 0 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 5 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if (x == 2 && y == 0)
                    {
                        //Add Player Base
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

                    if (x == 3 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }

                    if (x == 2 && y == 3)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }

                    if (x == 5 && y == 5)
                    {
                        //Add Enemy Base
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
                    }

                    if (x == 6 && y == 6)
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

        public static LoadLevelData TutorialScoutAbility()
        {
            int _width = 7;
            int _height = 7;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var position = new Tuple<int, int>(x, y);

                    if (x == 1 && y == 5)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }

                    if (x == 3 && y == 4)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if (x == 6 && y == 6)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

                    if (x == 4 && y == 2)
                    {
                        //Add Enemy Base
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
                    }

                    if (x == 0 && y == 0)
                    {
                        //Add Player Base
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
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

        public static LoadLevelData TutorialTriangleAbility()
        {
            int _width = 5;
            int _height = 10;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var position = new Tuple<int, int>(x, y);

                    // Player pieces
                    if (x == 2 && y == 0)
                    {

                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

                    if (x == 3 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }

                    if (x == 1 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 0 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    // Enemy pieces
                    if ((x == 2 && y == 7) || (x == 2 && y == 2))
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Triangle));
                    }

                    if (x == 4 && y == 9)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

                    if (x == 2 && y == 9)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Diamond));
                    }

                    if (x == 4 && y == 7)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }

                    if (x == 0 && y == 9)
                    {
                        //Add Enemy Base
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
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

                    if (x == 0 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

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

                    if (x == 0 && y == 9)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
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

                    if (x == 1 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

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

                    if (x == 6 && y == 9)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
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

        public static LoadLevelData LevelThree()
        {
            int _width = 9;
            int _height = 9;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Tuple<int, int> position = new(x, y);

                    if (x == 4 && y == 3)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

                    if (x == 1 && y == 2 || x == 5 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if (x == 4 && y == 4)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Triangle));
                    }

                    if (x == 3 && y == 4 || x == 5 && y == 3)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 3 && y == 3)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }


                    // enemies
                    if (x == 0 && y == _height - 1 || x == _width - 1 && y == 0 || x == _width - 1 && y == 5)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

                    if (x == 3 && y == _height - 2)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Diamond));
                    }

                    if (x == 4 && y == _height - 1)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }

                    if (x == 7 && y == _height - 1)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
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
		

		public static LoadLevelData LevelFour()
        {
            int _width = 11;
            int _height = 15;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Tuple<int, int> position = new(x, y);

                    if (x == 0 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

                    if (x == 2 && y == 1 || x == 7 && y == 3)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if (x == 5 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Triangle));
                    }

                    if (x == 3 && y == 2 || x == 6 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 5 && y == 0 || x == 9 && y == 1)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }


                    // enemies
                    if (x == 2 && y == 12 || x == 4 && y == 8 || x == 6 && y == 9)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

					if (x == 5 && y == 7 || x == 5 && y == 12)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Triangle));
                    }

                    if (x == 4 && y == 14 || x == 8 && y == 12)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Diamond));
                    }

                    if (x == 3 && y == 10 || x == 5 && y == 10 || x == 7 && y == 10)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }

                    if (x == 2 && y == 14)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
                    }

					// background tiles
					if (x == 0 && (y >= 4 && y <= 10)) {
						units.Add(new PieceInfo(position, false, PieceMono.Background));
					}

					if (x == 1 && (y >= 5 && y <= 9)) {
						units.Add(new PieceInfo(position, false, PieceMono.Background));
					}

					if (x == 2 && (y >= 6 && y <= 8)) {
						units.Add(new PieceInfo(position, false, PieceMono.Background));
					}


					if (x == 10 && (y >= 4 && y <= 10)) {
						units.Add(new PieceInfo(position, false, PieceMono.Background));
					}

					if (x == 9 && (y >= 5 && y <= 9)) {
						units.Add(new PieceInfo(position, false, PieceMono.Background));
					}

					if (x == 8 && (y >= 6 && y <= 8)) {
						units.Add(new PieceInfo(position, false, PieceMono.Background));
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

        public static LoadLevelData LevelFive()
        {
            int _width = 15;
            int _height = 15;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Tuple<int, int> position = new (x, y);

                    if (x == 9 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

                    if (x == 5 && y == 5)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if (x == 9 && y == 5)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if ((x==6 || x == 8) && y == 4)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 7 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }

                    if (x == 5 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if (x == 7 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Triangle));
                    }

                    if (x == 9 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }




                    // enemies
                    if (x == 14 && y == 9)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
                    }

                    if ((x==5 || x==9) && y == 14)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Diamond));
                    }

                    if (x == 7 && y == 14)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

                    if (x == 0 && y == 9)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

                    if (x == 5 && y == 9)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }

                    if (x == 7 && y == 8)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }

                    if (x == 9 && y == 9)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }

                    if (x == 7 && y == 12)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Triangle));
                    }

                    if (x == 14 && y == 7)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

                    if (x == 2 && y == 7)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Triangle));
                    }

                    if (x == 12 && y == 7)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Triangle));
                    }

                    // background tiles
                    if ((x >= 0 && x <= 4) && (y <= 14 && y >= 10))
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Background));
                    }

                    if ((x >= 10 && x <= 14) && (y <= 14 && y >= 10))
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Background));
                    }

                    if ((x >= 0 && x <= 4) && (y >= 0 && y <= 4))
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Background));
                    }

                    if ((x >= 10 && x <= 14) && (y >= 0 && y <= 4))
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Background));
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

        public static LoadLevelData LevelSix()
        {
            int _width = 14;
            int _height = 10;
            List<PieceInfo> units = new List<PieceInfo>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Tuple<int, int> position = new(x, y);

                    if (x == 0 && y == 0)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Base));
                    }

                    if ( x == 5 && y == 1 || x == 1 && y == 9)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Circle));
                    }

                    if (x == 12 && y == 2)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Triangle));
                    }

                    if (x == 6 && y == 2 || x == 0 && y == 9 || x == 8 && y == 6)
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Scout));
                    }

                    if (x == 1 && y == 1 ||  x == 6 && y == 9 )
                    {
                        units.Add(new PieceInfo(position, true, PieceMono.Diamond));
                    }


                    // enemies
                    if (x == 2 && y == 4 || x == 0 && y == 4 || x == 10 && y == 7 || x == 12 && y == 6 || x == 12 && y == 0   )
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Circle));
                    }

                    if (x == 1 && y == 4 || x == 12 && y == 9 || x == 6 && y == 6 || x == 3 && y == 8 ||  x == 9 && y == 0)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Triangle));
                    }
                    if (x == 12 && y == 7 || x == 13 && y == 0)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Diamond));
                    }

                    if (x == 10 && y == 9  || x == 7 && y == 7 || x == 13 && y == 7 )
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Scout));
                    }

                    if (x == 12 && y == 8)
                    {
                        units.Add(new PieceInfo(position, false, PieceMono.Base));
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