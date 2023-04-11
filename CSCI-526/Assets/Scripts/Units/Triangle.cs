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
            return "Capture Enemies: No \nAbility: Grants a large amount of vision to its owner.";
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

        public override List<Tuple<int, int>> GetVisibleArea(int range)
        {
            var lvlMono = LevelMono.Instance;
            var w = lvlMono.GetWidth();
            var h = lvlMono.GetHeight();
            var pos = this.transform.position;
            int x = (int)pos.x;
            int y = (int)pos.y;

            var visibleArea = new List<Tuple<int, int>>();
            for (int i = x - range; i <= x + range; i++)
            {
                for (int j = y - range; j <= y + range; j++)
                {
                    if (i >= 0 && i < w && j >= 0 && j < h)
                    {
                        visibleArea.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return visibleArea;
        }

    }
}
