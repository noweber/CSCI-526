using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Units
{
    public class Scout : PieceMono
    {
        private Direction direction = Direction.Up;

        public void SetDirection(Tuple<int, int> destination)
        {
            var pos = this.transform.position;
            int x = (int)pos.x;
            int y = (int)pos.y;

            if (x - destination.Item1 > 0)
            {
                Debug.Log("GOING LEFT");
                this.direction = Direction.Left;
            } 
            else if (x - destination.Item1 < 0)
            {
                Debug.Log("GOING RIGHT");
                this.direction = Direction.Right;
            } 
            else if (y - destination.Item2 > 0)
            {
                Debug.Log("GOING DOWN");
                this.direction = Direction.Down;
            }
            else
            {
                Debug.Log("GOING UP");
                this.direction = Direction.Up;
            }
        }
        public override string getUnitInfo()
        {
            return "Grants directional vision.";
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

            //Circle moves like a king (delta(x) + delta(y) <= 2)
			var pos = this.transform.position;
			int x = (int)pos.x;
			int y = (int)pos.y;
            var lvlMono = LevelMono.Instance;

            //add else block
            var availableMoves = new List<Tuple<int, int>>();
            availableMoves.Add(new Tuple<int, int>(x - 1, y));
            availableMoves.Add(new Tuple<int, int>(x, y - 1));
            availableMoves.Add(new Tuple<int, int>(x, y + 1));
            availableMoves.Add(new Tuple<int, int>(x + 1, y));
            foreach (Tuple<int, int> move in availableMoves)
            {
                if (move.Item1 >= 0 && move.Item1 < boardWidth && move.Item2 >= 0 && move.Item2 < boardHeight)
                {
                    if (lvlMono.GetPiece(move) != null && (!this.IsEnemyOf(lvlMono.GetPiece(move)) || (lvlMono.GetPiece(move).IsTriangle())))
                    {
                        continue;
                    }

                    legalSpots.Add(move);
                }
            }
            
            if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 0)
            {
                // Diamond moves first, circle must not
                legalSpots.Clear();
            }
            else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1)
            {
                // Circle must move closer to enemies
                legalSpots.Clear();
                var availableMove = new Tuple<int, int>(3, 3);
                legalSpots.Add(availableMove);
            }
            else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 2)
            {
                // Circle must capture an enemy
                legalSpots.Clear();
                var availableMove = new Tuple<int, int>(3, 4);
                legalSpots.Add(availableMove);
            }
            else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 2)
            {
                // Free form movement for last capture.
            }
            
            return legalSpots;
        }

        public override List<Tuple<int, int>> GetVisibleArea(int range)
        {
            var lvlMono = LevelMono.Instance;
            List<Tuple<int, int>> visibleArea = new List<Tuple<int, int>>();
            var pos = this.transform.position;
            int x = (int)pos.x;
            int y = (int)pos.y;

            switch (this.direction)
            {
                case Direction.Up:
                    for (int j = 1; j <= range; j++)
                    {
                        for (int i = 1; i < j; i++)
                        {
                            if (lvlMono.CheckOutOfBounds(x-i, y+j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x - i, y + j));
                            }
                            if (lvlMono.CheckOutOfBounds(x+i, y+j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x + i, y + j));
                            }
                        }
                        if (lvlMono.CheckOutOfBounds(x, y+j))
                        {
                            visibleArea.Add(new Tuple<int, int>(x, y + j));
                        }
                    }
                    break;
                case Direction.Right:
                    for (int i = 1; i <= range; i++)
                    {
                        for (int j = 1; j < i; j++)
                        {
                            if (lvlMono.CheckOutOfBounds(x+i, y - j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x + i, y - j));
                            }
                            if (lvlMono.CheckOutOfBounds(x+i, y+j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x + i, y + j));
                            }
                        }
                        if (lvlMono.CheckOutOfBounds(x + i, y))
                        {
                            visibleArea.Add(new Tuple<int, int>(x + i, y));
                        }
                    }
                    
                    break;
                case Direction.Down:
                    for (int j = 1; j <= range; j++)
                    {
                        for (int i = 1; i < j; i++)
                        {
                            if (lvlMono.CheckOutOfBounds(x-i, y - j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x - i, y - j));
                            }
                            if (lvlMono.CheckOutOfBounds(x+i, y - j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x + i, y - j));
                            }
                        }
                        if (lvlMono.CheckOutOfBounds(x, y - j))
                        {
                            visibleArea.Add(new Tuple<int, int>(x, y - j));
                        }
                    }
                    break;
                case Direction.Left:
                    for (int i = 1; i <= range; i++)
                    {
                        for (int j = 1; j < i; j++)
                        {
                            if (lvlMono.CheckOutOfBounds(x - i, y - j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x - i, y - j));
                            }
                            if (lvlMono.CheckOutOfBounds(x - i, y + j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x - i, y + j));
                            }
                        }
                        if (lvlMono.CheckOutOfBounds(x - i, y))
                        {
                            visibleArea.Add(new Tuple<int, int>(x - i, y));
                        }
                    }
                    break;
            }
            return visibleArea;
        }
        
    }

    public enum Direction {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}