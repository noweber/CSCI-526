using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Units;
using Assets.Scripts.Piece;

public abstract class PieceMono : MonoBehaviour
{
	protected bool isHuman;
	
	protected bool hasMoved;

	protected string unitName;

    protected string unitInfo;
    //public bool hasAbility = false;

	public void SetHuman(bool isHuman) { this.isHuman = isHuman; }

	public void SetName(string name) { this.unitName = name; }

	public void SetMoveState(bool state) { this.hasMoved = state; }

	public bool IsEnemyOf(PieceMono piece) { return this.IsHuman() != piece.IsHuman(); }

	public bool CanMove() { return !this.hasMoved; }

	public bool IsHuman() { return this.isHuman; }

	public bool IsCircle() { return this.unitName == "Circle"; }

	public bool IsTriangle() { return this.unitName == "Triangle"; }

	public bool IsDiamond() { return this.unitName == "Diamond"; }

	public string GetName() { return this.unitName; }

	public string GetInfo() { return this.unitInfo; }

	protected List<Tuple<int, int>> AdjacentAllies(Tuple<int, int> unitPosition)
	{
		var adjacentList = new List<Tuple<int, int>>();
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2));
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2));
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 + 1));
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 - 1));

        var adjAlly = new List<Tuple<int, int>>();
        var lvlMono = LevelMono.Instance;
        foreach (Tuple<int, int> coord in adjacentList)
        {
			if (lvlMono.GetPiece(coord) != null && !this.IsEnemyOf(lvlMono.GetPiece(coord)))
            {
            	adjAlly.Add(coord);
            }
        }    
        return adjAlly;
	}

    // public abstract bool IsMoveLegal(Vector2 destination);

    public abstract List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight);

    public void UpdateLocation(Vector3 location) { this.transform.position = location; }
}
