using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Triangle : PieceMono
    {
        public override string getUnitInfo()
        {
            return "Cannot move. Allies can move without cost when in Range of Triangle.";
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();
            var pos = this.transform.position;
            if (this.TriangleAbilityCheck())
            {
                // MenuManager.Instance.ShowAbilityButton();
            }
            return legalSpots;
        }

        private bool TriangleAbilityCheck()
        {
			var pos = this.transform.position;
            var adjList = this.AdjacentAllies();
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
