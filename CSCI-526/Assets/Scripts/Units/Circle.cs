using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Units
{
    public class Circle : Piece.Piece
    {
        public Circle(Tuple<int, int> piecePosition, bool isControlledByHumanPlayer) : base(piecePosition, isControlledByHumanPlayer)
        {
        }

        public override string Name()
        {
            return UnitType.Circle.ToString();
        }

        public override string Info()
        {
            return "Upon Capture, get another turn.";
        }

        public override List<Tuple<int, int>> AdjacentAllies(Tuple<int, int> unitPosition)
        {
            var adjacentList = new List<Tuple<int, int>>();
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2));
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2));
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 + 1));
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 - 1));

            var adjAlly = new List<Tuple<int, int>>();
            var lvlModel = LevelMono.Instance.LevelModel;
            foreach (Tuple<int, int> coord in adjacentList)
            {
                if (coord.Item1 >= 0 && coord.Item1 < LevelMono.Instance.LevelModel.GetWidth() && coord.Item2 >= 0 && coord.Item2 < LevelMono.Instance.LevelModel.GetHeight())
                {
                    if (lvlModel.TryGetUnit(coord) != null && lvlModel.TryGetUnit(coord).IsControlledByHuman() == base.IsControlledByHuman())
                    {
                        adjAlly.Add(coord);
                    }
                }
            }
            if (adjAlly.Count == 0)
            {
                return null;
            }
            return adjAlly;
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {

            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

            //Circle moves like a king (delta(x) + delta(y) <= 2)
            bool changeMovement = CircleMovementCheck(Position);
            var lvlModel = LevelMono.Instance.LevelModel;

            if (changeMovement == true)
            {
                int maxRange = 3;
                // left 
                var range = UnityEngine.Mathf.Min(Position.Item1, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(Position.Item1 - i, Position.Item2);
                    if (lvlModel.TryGetUnit(availableMove) != null && (lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || lvlModel.TryGetUnit(availableMove).IsControlledByHuman() == base.IsControlledByHuman()))
                    {
                        break;
                    }
                    else if (lvlModel.TryGetUnit(availableMove) != null && (!lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || (!base.IsControlledByHuman() && lvlModel.TryGetUnit(availableMove).IsControlledByHuman())))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // right
                range = UnityEngine.Mathf.Min(boardWidth - Position.Item1 - 1, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(Position.Item1 + i, Position.Item2);
                    if (lvlModel.TryGetUnit(availableMove) != null && (lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || lvlModel.TryGetUnit(availableMove).IsControlledByHuman() == base.IsControlledByHuman()))
                    {
                        break;
                    }
                    else if (lvlModel.TryGetUnit(availableMove) != null && (!lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || (!base.IsControlledByHuman() && lvlModel.TryGetUnit(availableMove).IsControlledByHuman())))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // up
                range = UnityEngine.Mathf.Min(boardHeight - Position.Item2 - 1, maxRange);
                for (int j = 1; j <= range; j++)
                {
                    var availableMove = new Tuple<int, int>(Position.Item1, Position.Item2 + j);
                    if (lvlModel.TryGetUnit(availableMove) != null && (lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || lvlModel.TryGetUnit(availableMove).IsControlledByHuman() == base.IsControlledByHuman()))
                    {
                        break;
                    }
                    else if (lvlModel.TryGetUnit(availableMove) != null && (!lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || (!base.IsControlledByHuman() && lvlModel.TryGetUnit(availableMove).IsControlledByHuman())))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // down
                range = UnityEngine.Mathf.Min(Position.Item2, maxRange);
                for (int j = 1; j <= range; j++)
                {
                    var availableMove = new Tuple<int, int>(Position.Item1, Position.Item2 - j);
                    if (lvlModel.TryGetUnit(availableMove) != null && (lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || lvlModel.TryGetUnit(availableMove).IsControlledByHuman() == base.IsControlledByHuman()))
                    {
                        break;
                    }
                    else if (lvlModel.TryGetUnit(availableMove) != null && (!lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || (!base.IsControlledByHuman() && lvlModel.TryGetUnit(availableMove).IsControlledByHuman())))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // top left 
                var minIndex = UnityEngine.Mathf.Min(Position.Item1, boardHeight - Position.Item2 - 1);
                range = UnityEngine.Mathf.Min(minIndex, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(Position.Item1 - i, Position.Item2 + i);
                    if (lvlModel.TryGetUnit(availableMove) != null && (lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || lvlModel.TryGetUnit(availableMove).IsControlledByHuman() == base.IsControlledByHuman()))
                    {
                        break;
                    }
                    else if (lvlModel.TryGetUnit(availableMove) != null && (!lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || (!base.IsControlledByHuman() && lvlModel.TryGetUnit(availableMove).IsControlledByHuman())))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // top right 
                minIndex = UnityEngine.Mathf.Min(boardWidth - Position.Item1 - 1, boardHeight - Position.Item2 - 1);
                range = UnityEngine.Mathf.Min(minIndex, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(Position.Item1 + i, Position.Item2 + i);
                    if (lvlModel.TryGetUnit(availableMove) != null && (lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || lvlModel.TryGetUnit(availableMove).IsControlledByHuman() == base.IsControlledByHuman()))
                    {
                        break;
                    }
                    else if (lvlModel.TryGetUnit(availableMove) != null && (!lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || (!base.IsControlledByHuman() && lvlModel.TryGetUnit(availableMove).IsControlledByHuman())))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // bottom left 
                minIndex = UnityEngine.Mathf.Min(Position.Item1, Position.Item2);
                range = UnityEngine.Mathf.Min(minIndex, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(Position.Item1 - i, Position.Item2 - i);
                    if (lvlModel.TryGetUnit(availableMove) != null && (lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || lvlModel.TryGetUnit(availableMove).IsControlledByHuman() == base.IsControlledByHuman()))
                    {
                        break;
                    }
                    else if (lvlModel.TryGetUnit(availableMove) != null && (!lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || (!base.IsControlledByHuman() && lvlModel.TryGetUnit(availableMove).IsControlledByHuman())))
                    {
                        legalSpots.Add(availableMove);
                        break;
                    }
                    else { legalSpots.Add(availableMove); }
                }

                // bottom right 
                minIndex = UnityEngine.Mathf.Min(boardWidth - Position.Item1 - 1, Position.Item2);
                range = UnityEngine.Mathf.Min(minIndex, maxRange);
                for (int i = 1; i <= range; i++)
                {
                    var availableMove = new Tuple<int, int>(Position.Item1 + i, Position.Item2 - i);
                    if (lvlModel.TryGetUnit(availableMove) != null && (lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || lvlModel.TryGetUnit(availableMove).IsControlledByHuman() == base.IsControlledByHuman()))
                    {
                        break;
                    }
                    else if (lvlModel.TryGetUnit(availableMove) != null && (!lvlModel.TryGetUnit(availableMove).IsControlledByHuman() || (!base.IsControlledByHuman() && lvlModel.TryGetUnit(availableMove).IsControlledByHuman())))
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
                availableMoves.Add(new Tuple<int, int>(Position.Item1 - 1, Position.Item2 - 1));
                availableMoves.Add(new Tuple<int, int>(Position.Item1 - 1, Position.Item2));
                availableMoves.Add(new Tuple<int, int>(Position.Item1 - 1, Position.Item2 + 1));
                availableMoves.Add(new Tuple<int, int>(Position.Item1, Position.Item2 - 1));
                availableMoves.Add(new Tuple<int, int>(Position.Item1, Position.Item2 + 1));
                availableMoves.Add(new Tuple<int, int>(Position.Item1 + 1, Position.Item2 - 1));
                availableMoves.Add(new Tuple<int, int>(Position.Item1 + 1, Position.Item2));
                availableMoves.Add(new Tuple<int, int>(Position.Item1 + 1, Position.Item2 + 1));
                foreach (Tuple<int, int> move in availableMoves)
                {
                    if (move.Item1 >= 0 && move.Item1 < boardWidth && move.Item2 >= 0 && move.Item2 < boardHeight)
                    {
                        if (lvlModel.TryGetUnit(move) != null && lvlModel.TryGetUnit(move).IsControlledByHuman())
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
            var trianglePosition = inTriangleRange(Position);
            if (trianglePosition != null)
            {
                UnityEngine.Debug.Log("Unlimited movement around triangle");
                var adjacentList = new List<Tuple<int, int>>();
                adjacentList.Add(new Tuple<int, int>(trianglePosition.Item1 + 1, trianglePosition.Item2)); //right
                adjacentList.Add(new Tuple<int, int>(trianglePosition.Item1 - 1, trianglePosition.Item2)); //left
                adjacentList.Add(new Tuple<int, int>(trianglePosition.Item1, trianglePosition.Item2 + 1)); //up
                adjacentList.Add(new Tuple<int, int>(trianglePosition.Item1, trianglePosition.Item2 - 1)); //down
                adjacentList.Add(new Tuple<int, int>(trianglePosition.Item1 + 1, trianglePosition.Item2 + 1)); //right up diag
                adjacentList.Add(new Tuple<int, int>(trianglePosition.Item1 - 1, trianglePosition.Item2 + 1)); //left  up diag
                adjacentList.Add(new Tuple<int, int>(trianglePosition.Item1 + 1, trianglePosition.Item2 - 1)); //right down diag
                adjacentList.Add(new Tuple<int, int>(trianglePosition.Item1 - 1, trianglePosition.Item2 - 1)); //left down diag

                foreach (var adj in adjacentList)
                {
                    legalSpots.Add(adj);
                }
            }

            return legalSpots;
        }

        private bool CircleMovementCheck(Tuple<int, int> pos)
        {
            var adjList = this.AdjacentAllies(new Tuple<int, int>(pos.Item1, pos.Item2));
            var lvlModel = LevelMono.Instance.LevelModel;
            if (adjList != null)
            {
                foreach (Tuple<int, int> coord in adjList)
                {
                    //Debug.Log("" + coord.Item1 + " " + coord.Item2);
                    if (lvlModel.TryGetUnit(coord) != null && string.Equals(lvlModel.TryGetUnit(coord).Name(), UnitType.Diamond.ToString()))
                    {
                        //Debug.Log("Make Circle move like Queen");
                        return true;
                    }
                }
            }
            return false;
        }

        private Tuple<int, int> inTriangleRange(Tuple<int,int> unitPosition)
        {
            var adjacentList = new List<Tuple<int, int>>();
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2)); //right
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2)); //left
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 + 1)); //up
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 - 1)); //down
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2 + 1)); //right up diag
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2 + 1)); //left  up diag
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2 - 1)); //right down diag
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2 - 1)); //left down diag

            var lvlModel = LevelMono.Instance.LevelModel;
            foreach (Tuple<int, int> coord in adjacentList)
            {
                if (coord.Item1 >= 0 && coord.Item1 < LevelMono.Instance.LevelModel.GetWidth() && coord.Item2 >= 0 && coord.Item2 < LevelMono.Instance.LevelModel.GetHeight())
                {
                    if (lvlModel.TryGetUnit(coord) != null && lvlModel.TryGetUnit(coord).IsControlledByHuman() == base.IsControlledByHuman() && string.Equals(lvlModel.TryGetUnit(coord).Name(), UnitType.Triangle.ToString()))
                    {
                        return coord;
                    }
                }
            }
            return null;
        }
    }
}
