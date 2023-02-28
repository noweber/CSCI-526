using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Levels
{
	public class PieceInfo
	{
		private Tuple<int, int> position;

		private bool isHuman;
		
		private string unitName;

		public PieceInfo(Tuple<int, int> position, bool isHuman, string unitName)
		{
			this.position = position;
			this.isHuman = isHuman;
			this.unitName = unitName;
		}
		
		public bool IsCircle() { return string.Equals(unitName, PieceMono.Circle); }

		public bool IsTriangle() { return string.Equals(unitName, PieceMono.Triangle); }
		public bool IsDiamond() { return string.Equals(unitName, PieceMono.Diamond); }
		public bool IsScout() { return string.Equals(unitName, PieceMono.Scout); }
		
		public bool IsHuman() { return this.isHuman; }

		public Tuple<int, int> GetPosition()
		{
			return this.position; 
		}
	}
}
