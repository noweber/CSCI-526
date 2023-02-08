using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Triangle : Piece.Piece
    {
        public Triangle(Tuple<int, int> piecePosition, bool isControlledByHumanPlayer) : base(piecePosition, isControlledByHumanPlayer)
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
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2)); //right
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2)); //left
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 + 1)); //up
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 - 1)); //down
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2 + 1)); //right up diag
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2 + 1)); //left  up diag
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2 - 1)); //right down diag
            adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2 - 1)); //left down diag

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
                return null;
            return adjAlly;
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();
            PieceMono triangleAbilityCheck = this.TriangleAbilityCheck(Position);
            if (triangleAbilityCheck != null)// && GameManagerChain.Instance.UsedAbility != true)
            {
                //MenuManager.Instance.ShowAbilityButton();
            }
            return legalSpots;
        }

        private PieceMono TriangleAbilityCheck(Tuple<int, int> position)
        {
            var adjList = this.AdjacentAllies(new Tuple<int, int>(position.Item1, position.Item2));
            var lvlModel = LevelMono.Instance.LevelModel;
            if (adjList != null)
            {
                foreach (Tuple<int, int> coord in adjList)
                {
                    if (lvlModel.TryGetUnit(coord) != null)// && string.Equals(lvlModel.TryGetUnit(coord).Name(), UnitType.Circle.ToString()))
                    {
                        Debug.Log("Set Triangle Ability True");
                        //return true;
                        return LevelMono.Instance._pieces[coord];
                    }
                }
            }
            return null;
        }
    }
}
