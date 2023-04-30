using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Units
{
    public class Scout : PieceMono
    {
        private Direction direction = Direction.Up;
        [SerializeField] public GameObject upArrow;
        [SerializeField] public GameObject downArrow;
        [SerializeField] public GameObject leftArrow;
        [SerializeField] public GameObject rightArrow;


        //TODO: Enemy Scout Unit set as Up instead of Down on Initialization
        public void SetInitialDirection(Direction d)
        {
            this.direction = d;
            if (this.direction == Direction.Up)
            {
                upArrow.SetActive(true);
                downArrow.SetActive(false);
                leftArrow.SetActive(false);
                rightArrow.SetActive(false);
            }

            if (this.direction == Direction.Down)
            {
                upArrow.SetActive(false);
                downArrow.SetActive(true);
                leftArrow.SetActive(false);
                rightArrow.SetActive(false);
            }

        }

        public void SetDirection(Tuple<int, int> destination)
        {
            var pos = this.transform.position;
            int x = (int)pos.x;
            int y = (int)pos.y;

            if (x - destination.Item1 > 0)
            {
                Debug.Log("GOING LEFT");
                this.direction = Direction.Left;
                upArrow.SetActive(false);
                downArrow.SetActive(false);
                leftArrow.SetActive(true);
                rightArrow.SetActive(false);
            }
            else if (x - destination.Item1 < 0)
            {
                Debug.Log("GOING RIGHT");
                this.direction = Direction.Right;
                upArrow.SetActive(false);
                downArrow.SetActive(false);
                leftArrow.SetActive(false);
                rightArrow.SetActive(true);
            }
            else if (y - destination.Item2 > 0)
            {
                Debug.Log("GOING DOWN");
                this.direction = Direction.Down;
                upArrow.SetActive(false);
                downArrow.SetActive(true);
                leftArrow.SetActive(false);
                rightArrow.SetActive(false);
            }
            else
            {
                Debug.Log("GOING UP");
                this.direction = Direction.Up;
                upArrow.SetActive(true);
                downArrow.SetActive(false);
                leftArrow.SetActive(false);
                rightArrow.SetActive(false);
            }
        }
        public override string getUnitInfo()
        {
            if (this.IsHuman())
                return "Capture Enemies: Yes \nAbility: Grants directional vision.";
            else
                return "Capture Player: Yes \nAbility: Grants directional vision.";
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
                    if (lvlMono.GetPiece(coord) != null && lvlMono.GetPiece(coord).IsDiamond())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();
            var pos = this.transform.position;
            int x = (int)pos.x;
            int y = (int)pos.y;
            var lvlMono = LevelMono.Instance;
            bool changeMovement = this.CircleMovementCheck();

            if (changeMovement)
            {
                int maxRange = 2;
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
                    else if (!lvlMono.GetTile(availableMove).gameObject.activeInHierarchy)
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
                    else if (!lvlMono.GetTile(availableMove).gameObject.activeInHierarchy)
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
                    else if (!lvlMono.GetTile(availableMove).gameObject.activeInHierarchy)
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
                    else if (!lvlMono.GetTile(availableMove).gameObject.activeInHierarchy)
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
                availableMoves.Add(new Tuple<int, int>(x - 1, y));
                availableMoves.Add(new Tuple<int, int>(x, y - 1));
                availableMoves.Add(new Tuple<int, int>(x, y + 1));
                availableMoves.Add(new Tuple<int, int>(x + 1, y));
                foreach (Tuple<int, int> move in availableMoves)
                {
                    if (move.Item1 >= 0 && move.Item1 < boardWidth && move.Item2 >= 0 && move.Item2 < boardHeight)
                    {
                        if (lvlMono.GetPiece(move) != null && (!this.IsEnemyOf(lvlMono.GetPiece(move)) ||
                                                               (lvlMono.GetPiece(move).IsTriangle())))
                        {
                            continue;
                        }
                        else if (!lvlMono.GetTile(move).gameObject.activeInHierarchy)
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
                    for (int j = 0; j < range; j++)
                    {
                        for (int i = 1; i < Math.Max(j + 1, 2); i++)
                        {
                            if (lvlMono.CheckOutOfBounds(x - i, y + j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x - i, y + j));
                            }
                            if (lvlMono.CheckOutOfBounds(x + i, y + j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x + i, y + j));
                            }
                        }
                        if (lvlMono.CheckOutOfBounds(x, y + j))
                        {
                            visibleArea.Add(new Tuple<int, int>(x, y + j));
                        }
                    }
                    break;
                case Direction.Right:
                    for (int i = 0; i < range; i++)
                    {
                        for (int j = 1; j < Math.Max(i + 1, 2); j++)
                        {
                            if (lvlMono.CheckOutOfBounds(x + i, y - j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x + i, y - j));
                            }
                            if (lvlMono.CheckOutOfBounds(x + i, y + j))
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
                    for (int j = 0; j < range; j++)
                    {
                        for (int i = 1; i < Math.Max(j + 1, 2); i++)
                        {
                            if (lvlMono.CheckOutOfBounds(x - i, y - j))
                            {
                                visibleArea.Add(new Tuple<int, int>(x - i, y - j));
                            }
                            if (lvlMono.CheckOutOfBounds(x + i, y - j))
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
                    for (int i = 0; i < range; i++)
                    {
                        for (int j = 1; j < Math.Max(i + 1, 2); j++)
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

            visibleArea.Add(new Tuple<int, int>(x, y));
            return visibleArea;
        }

    }

    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}