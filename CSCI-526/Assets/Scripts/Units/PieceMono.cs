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

	//public abstract string unitInfo;

    public abstract string getUnitInfo();
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

	// public abstract bool IsMoveLegal(Vector2 destination);

    public abstract List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight);

    public void UpdateLocation(Vector3 location) { this.transform.position = location; }

	protected List<Tuple<int, int>> AdjacentAllies()
	{
		var lvlMono = LevelMono.Instance;
		var pos = this.transform.position;
		int x = (int)pos.x;
		int y = (int)pos.y;

		var adjacentList = new List<Tuple<int, int>>();
        adjacentList.Add(new Tuple<int, int>(x + 1, y)); //right
        adjacentList.Add(new Tuple<int, int>(x - 1, y)); //left
        adjacentList.Add(new Tuple<int, int>(x, y + 1)); //up
        adjacentList.Add(new Tuple<int, int>(x, y - 1)); //down
        adjacentList.Add(new Tuple<int, int>(x + 1, y + 1)); //right up diag
        adjacentList.Add(new Tuple<int, int>(x - 1, y + 1)); //left  up diag
        adjacentList.Add(new Tuple<int, int>(x + 1, y - 1)); //right down diag
        adjacentList.Add(new Tuple<int, int>(x - 1, y - 1)); //left down diag

        var adjAlly = new List<Tuple<int, int>>();
		
        foreach (Tuple<int, int> coord in adjacentList)
        {
			if (lvlMono.GetPiece(coord) != null && !this.IsEnemyOf(lvlMono.GetPiece(coord)))
            {
            	adjAlly.Add(coord);
            }
        }    
        return adjAlly;
	}

	// Checks if piece gameobject is near a triangle
	public bool inTriangleRange()
    {
		var lvlMono = LevelMono.Instance;
		var pos = this.transform.position;
		int x = (int)pos.x;
		int y = (int)pos.y;
		
		var adjacentList = this.AdjacentAllies();

        foreach (Tuple<int, int> coord in adjacentList)
        {
			if (lvlMono.GetPiece(coord).IsTriangle())
            {	
            	return true;
        	}
		}    
        return false;
	}
}
