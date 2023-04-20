using System;
using System.Collections.Generic;

namespace Assets.Scripts.Units
{
    public class Triangle : PieceMono
    {
        public override string getUnitInfo()
        {
            if (this.IsHuman())
                return "Capture Enemies: No \nAbility: Grants a large amount of vision to its owner.";
            else
                return "Capture Player: No \nAbility: Grants a large amount of vision to its owner.";
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            return new List<Tuple<int, int>>();
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
