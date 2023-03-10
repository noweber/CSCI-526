using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Units
{
    public class Circle : PieceMono
    { 
        //public string unitInfo = "Upon Capture, Circle can move again.";
        public override string getUnitInfo()
        {
            return "Upon Capture, Circle can move again.";
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

            //Circle moves like a king (delta(x) + delta(y) <= 2)
			var pos = this.transform.position;
			int x = (int)pos.x;
			int y = (int)pos.y;
            bool changeMovement = this.CircleMovementCheck();
            var lvlMono = LevelMono.Instance;

            if (changeMovement == true)
            {
                int maxRange = 3;
                // left 
                var range = UnityEngine.Mathf.Min(x, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(x - i, y);
					var availablePiece = lvlMono.GetPiece(availableMove);
                    if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                    {
                        break;
                    }
                    else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // right
                range = UnityEngine.Mathf.Min(boardWidth - x - 1, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(x + i, y);
                    var availablePiece = lvlMono.GetPiece(availableMove);
                    if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                    {
                        break;
                    }
                    else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // up
                range = UnityEngine.Mathf.Min(boardHeight - y - 1, maxRange);
                for (int j = 1; j <= range; j++)
                {
                    var availableMove = new Tuple<int, int>(x, y + j);
                    var availablePiece = lvlMono.GetPiece(availableMove);
                    if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                    {
                        break;
                    }
                    else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // down
                range = UnityEngine.Mathf.Min(y, maxRange);
                for (int j = 1; j <= range; j++)
                {
                    var availableMove = new Tuple<int, int>(x, y - j);
                    var availablePiece = lvlMono.GetPiece(availableMove);
                    if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                    {
                        break;
                    }
                    else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // top left 
                var minIndex = UnityEngine.Mathf.Min(x, boardHeight - y - 1);
                range = UnityEngine.Mathf.Min(minIndex, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(x - i, y + i);
                    var availablePiece = lvlMono.GetPiece(availableMove);
                    if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                    {
                        break;
                    }
                    else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // top right 
                minIndex = UnityEngine.Mathf.Min(boardWidth - x - 1, boardHeight - y - 1);
                range = UnityEngine.Mathf.Min(minIndex, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(x + i, y + i);
                    var availablePiece = lvlMono.GetPiece(availableMove);
                    if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                    {
                        break;
                    }
                    else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // bottom left 
                minIndex = UnityEngine.Mathf.Min(x, y);
                range = UnityEngine.Mathf.Min(minIndex, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(x - i, y - i);
                    var availablePiece = lvlMono.GetPiece(availableMove);
                    if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                    {
                        break;
                    }
                    else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // bottom right 
                minIndex = UnityEngine.Mathf.Min(boardWidth - x - 1, y);
                range = UnityEngine.Mathf.Min(minIndex, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(x + i, y - i);
                    var availablePiece = lvlMono.GetPiece(availableMove);
                    if (availablePiece != null && (!this.IsEnemyOf(availablePiece) || availablePiece.IsTriangle()))
                    {
                        break;
                    }
                    else if (availablePiece != null && this.IsEnemyOf(availablePiece))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
					
                    else { legalSpots.Add(availableMove); }
                }

            }
            else
            {
                //add else block
                var availableMoves = new List<Tuple<int, int>>();
                availableMoves.Add(new Tuple<int, int>(x - 1, y - 1));
                availableMoves.Add(new Tuple<int, int>(x - 1, y));
                availableMoves.Add(new Tuple<int, int>(x - 1, y + 1));
                availableMoves.Add(new Tuple<int, int>(x, y - 1));
                availableMoves.Add(new Tuple<int, int>(x, y + 1));
                availableMoves.Add(new Tuple<int, int>(x + 1, y - 1));
                availableMoves.Add(new Tuple<int, int>(x + 1, y));
                availableMoves.Add(new Tuple<int, int>(x + 1, y + 1));
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
            
            // var triangleLegal = this.inTriangleRange();
            // if (triangleLegal)
            // {
            // }

            return legalSpots;
        }

        private bool CircleMovementCheck()
        {
			var pos = this.transform.position;
            var adjList = this.AdjacentAllies();
            var lvlMono = LevelMono.Instance;
            if (adjList != null)
            {
                foreach (Tuple<int, int> coord in adjList)
                {
                    //Debug.Log("" + coord.Item1 + " " + coord.Item2);
                    if (lvlMono.GetPiece(coord) != null && lvlMono.GetPiece(coord).IsDiamond())
                    {
                        // Debug.Log("Make Circle move like Queen");
                        return true;
                    }
                }
            }
            return false;
        }

		public override List<Tuple<int, int>> GetVisibleArea(int range)
        {
            /*
            var lvlMono = LevelMono.Instance;
            var pos = this.transform.position;
            int x = (int)pos.x;
            int y = (int)pos.y;
            List<Tuple<int, int>> visibleArea = new List<Tuple<int, int>>();
            var availableMoves = new List<Tuple<int, int>>();
            availableMoves.Add(new Tuple<int, int>(x, y));
            availableMoves.Add(new Tuple<int, int>(x - 1, y - 1));
            availableMoves.Add(new Tuple<int, int>(x - 1, y));
            availableMoves.Add(new Tuple<int, int>(x - 1, y + 1));
            availableMoves.Add(new Tuple<int, int>(x, y - 1));
            availableMoves.Add(new Tuple<int, int>(x, y + 1));
            availableMoves.Add(new Tuple<int, int>(x + 1, y - 1));
            availableMoves.Add(new Tuple<int, int>(x + 1, y));
            availableMoves.Add(new Tuple<int, int>(x + 1, y + 1));
            foreach (Tuple<int, int> move in availableMoves)
            {
                if (move.Item1 >= 0 && move.Item1 < lvlMono.GetWidth() && move.Item2 >= 0 && move.Item2 < lvlMono.GetHeight())
                {
                    if (lvlMono.GetPiece(move) != null && (!this.IsEnemyOf(lvlMono.GetPiece(move)) || (lvlMono.GetPiece(move).IsTriangle())))
                    {
                        continue;
                    }
                    visibleArea.Add(move);
                }
            }
            */
            List<Tuple<int, int>> visibleArea = new List<Tuple<int, int>>();
            return visibleArea;
        }
    }
}
