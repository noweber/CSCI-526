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
		
		public bool IsCircle() { return unitName == "Circle"; }

		public bool IsTriangle() { return unitName == "Triangle"; }

		public bool IsDiamond() { return unitName == "Diamond"; }
		
		public bool IsHuman() { return this.isHuman; }

		public Tuple<int, int> GetPosition()
		{
			return this.position; 
		}
	}
}
