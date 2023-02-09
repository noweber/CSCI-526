using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Triangle : PieceMono
    {
        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();
            var pos = this.transform;
            if (this.TriangleAbilityCheck(new Tuple<int, int>((int)pos.x, (int)pos.y)))
            {
                MenuManager.Instance.ShowAbilityButton();
            }
            return legalSpots;
        }

        private bool TriangleAbilityCheck(Tuple<int, int> position)
        {
			var pos = this.transform;
            var adjList = this.AdjacentAllies(new Tuple<int, int>((int)pos.x, (int)pos.y));
            var lvlMono = LevelMono.Instance;
            if (adjList != null)
            {
                foreach (Tuple<int, int> coord in adjList)
                {
                    if (lvlMono.GetPiece(coord) != null && lvlMono.GetPiece(coord).IsCircle())// && string.Equals(lvlModel.TryGetUnit(coord).Name(), UnitType.Circle.ToString()))
                    {
                        Debug.Log("Set Triangle Ability True");
                        //return true;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
