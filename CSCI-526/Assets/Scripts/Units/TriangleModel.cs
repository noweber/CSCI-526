using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class TriangleModel : PieceModel
    {
        public TriangleModel(Tuple<int, int> piecePosition, bool isControlledByHumanPlayer) : base(piecePosition, isControlledByHumanPlayer)
        {
        }

        public override string Name()
        {
            return UnitType.Triangle.ToString();
        }

        public override string Info()
        {
            return "Cannot move/capture. If next to Circle, can grant player an extra move (works once per turn).";
        }

        public override List<Tuple<int, int>> AdjacentAllies(Tuple<int, int> unitPosition)
        {
            var adjacentList = new List<Tuple<int, int>>();
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2));
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2));
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 + 1));
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 - 1));

            var adjAlly = new List<Tuple<int, int>>();
            var lvlModel = LevelController.Instance.LevelModel;
            foreach (Tuple<int, int> coord in adjacentList)
            {
                if (coord.Item1 >= 0 && coord.Item1 < LevelController.Instance.LevelModel.GetWidth() && coord.Item2 >= 0 && coord.Item2 < LevelController.Instance.LevelModel.GetHeight())
                {
                    if (lvlModel.TryGetUnit(coord) != null && lvlModel.TryGetUnit(coord).IsControlledByHuman() == base.IsControlledByHuman())
                    {
                        adjAlly.Add(coord);
                    }
                }
            }
            if (adjAlly.Count == 0)
                return null;
            return adjAlly;
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();
            bool triangleAbilityCheck = this.TriangleAbilityCheck(Position);
            if (triangleAbilityCheck == true && GameManagerChain.Instance.UsedAbility != true)
            {
                //enable some ability option to add another movement turn
                //MenuManager Option pops up
                MenuManager.Instance.ShowAbilityButton();
                //if option is selected, ignore triangle legal moves + add 1 to NumMoves?
            }
            return legalSpots;
        }

        private bool TriangleAbilityCheck(Tuple<int, int> position)
        {
            var adjList = this.AdjacentAllies(new Tuple<int, int>(position.Item1, position.Item2));
            /*foreach (Tuple<int, int> coord in adjList)
            {
                Debug.Log("Triangle Allies: " + coord.Item1 + ", " + coord.Item2);
            }*/
            var lvlModel = LevelController.Instance.LevelModel;
            if (adjList != null)
            {
                foreach (Tuple<int, int> coord in adjList)
                {
                    if (lvlModel.TryGetUnit(coord) != null && string.Equals(lvlModel.TryGetUnit(coord).Name(), UnitType.Circle.ToString()))
                    {
                        Debug.Log("Set Triangle Ability True");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
