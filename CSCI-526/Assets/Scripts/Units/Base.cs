using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Units
{
    public class Base : PieceMono
    {
        public override string getUnitInfo()
        {
            if (this.IsHuman())
                return "Capture Enemies: N\\A \nAbility: Protect your planet from enemies or else you lose.";
            else
                return "Capture Player: N\\A \nAbility: Capture enemy planet to win.";
        }

        public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
        {
            List<Tuple<int, int>> legal = new List<Tuple<int, int>>();
            return legal;
        }

        public override List<Tuple<int, int>> GetVisibleArea(int range = -1)
        {
            List<Tuple<int, int>> visible = new List<Tuple<int, int>>();
            return visible;
        }
    }
}
