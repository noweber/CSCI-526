using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Diamond : Piece.Piece
    {
        public Diamond(Tuple<int, int> piecePosition, bool isControlledByHumanPlayer) : base(piecePosition, isControlledByHumanPlayer)
        {
        }

        public override string Name()
        {
            return UnitType.Diamond.ToString();
        }

        public override string Info()
        {
            return "If adjacent to circle, circle gains range.";
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
                if (lvlModel.TryGetUnit(coord).IsControlledByHuman() == base.IsControlledByHuman())
                {
                    adjAlly.Add(coord);
                }
            }
            if (adjAlly.Count == 0)
                return null;
            return adjAlly;
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

            var lvlModel = LevelMono.Instance.LevelModel;

            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    //get diagonals - those are ok
                    if (Mathf.Abs(i - Position.Item1) == 1 && Mathf.Abs(j - Position.Item2) == 1)
                    {
                        var availableMove = new Tuple<int, int>(i, j);
                        if (lvlModel.TryGetUnit(availableMove) != null)
                        {
                            if (lvlModel.TryGetUnit(availableMove).IsControlledByHuman() == this.IsControlledByHuman()) { continue; }
                        }
                        legalSpots.Add(availableMove);
                    }

                    //get +1 to cardinal directions
                    if (Mathf.Abs(i - Position.Item1) == 2 && Mathf.Abs(j - Position.Item2) == 0)
                    {
                        var availableMove = new Tuple<int, int>(i, j);
                        if (lvlModel.TryGetUnit(availableMove) != null)
                        {
                            if (lvlModel.TryGetUnit(availableMove).IsControlledByHuman()  == this.IsControlledByHuman()) { continue; }
                        }
                        legalSpots.Add(availableMove);
                    }
                    if (Mathf.Abs(i - Position.Item1) == 0 && Mathf.Abs(j - Position.Item2) == 2)
                    {
                        var availableMove = new Tuple<int, int>(i, j);
                        if (lvlModel.TryGetUnit(availableMove) != null)
                        {
                            if (lvlModel.TryGetUnit(availableMove).IsControlledByHuman() == this.IsControlledByHuman()) { continue; }
                        }
                        legalSpots.Add(availableMove);
                    }
                }
            }

            if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 0)
            {
                // first move of tutorial
                Debug.Log("FIRST MOVE");
                var availableMove = new Tuple<int, int>(1, 0);
                legalSpots.Clear();
                legalSpots.Add(availableMove);
            }
            else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1)
            {
                legalSpots.Clear();
            }

            return legalSpots;
        }
    }
}
