using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;       // IEnumerator
using UnityEngine.Tilemaps;

public abstract class PieceMono : MonoBehaviour
{
	protected bool isHuman;
	
	protected bool hasMoved;

	protected string unitName;

    [SerializeField] private GameObject nSupport, wSupport, sSupport, eSupport, nwSupport, neSupport, swSupport, seSupport;     // Diamond support indicators

    [SerializeField] private GameObject buffParticles;      // Circle indicator when it is buffed by Diamond

    public GameObject canMoveObject, cantMoveObject;     // Highlight will be temporary -- change to particles after midterm

    public abstract string getUnitInfo();

	public void SetHuman(bool isHuman) { this.isHuman = isHuman; }

	public void SetName(string name) { this.unitName = name; }

	public void SetMoveState(bool state) { this.hasMoved = state; }

	public bool IsEnemyOf(PieceMono piece) { return this.IsHuman() != piece.IsHuman(); }

	public bool CanMove() { return !this.hasMoved; }

	public bool IsHuman() { return this.isHuman; }

	public bool IsCircle() { return this.unitName == "Circle"; }

	public bool IsTriangle() { return this.unitName == "Triangle"; }

	public bool IsDiamond() { return this.unitName == "Diamond"; }

	public bool IsScout() { return this.unitName == "Scout"; }

	public string GetName() { return this.unitName; }

    public abstract List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight);
    
    public abstract List<Tuple<int, int>> GetVisibleArea(int range=-1);

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
	
	protected List<Tuple<int, int>> AdjacentEnemies()
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
			if (lvlMono.GetPiece(coord) != null && this.IsEnemyOf(lvlMono.GetPiece(coord)))
			{
				adjAlly.Add(coord);
			}
		}    
		return adjAlly;
	}

	// Checks if piece gameobject is near a triangle
	public Tuple<int, int> InTowerRange()
    {
		var lvlMono = LevelMono.Instance;
		var pos = this.transform.position;
		int x = (int)pos.x;
		int y = (int)pos.y;
		
		var adjacentList = this.AdjacentEnemies();

        foreach (Tuple<int, int> coord in adjacentList)
        {
			if (lvlMono.GetPiece(coord).IsTriangle())
            {	
            	return coord;
        	}
		}    
        return null;
	}

	public bool IsAdjacentToAllyCircle()
    {
		var list = this.AdjacentAllies();
		foreach (var ally in list)
        {
			if (LevelMono.Instance.GetPiece(ally).IsCircle())
            {
				return true;
			}
        }
		return false;
    }

	public bool IsAdjacentToAllyDiamond()
	{
		var list = this.AdjacentAllies();
		foreach (var ally in list)
		{
			if (LevelMono.Instance.GetPiece(ally).IsDiamond())
			{
				return true;
			}
		}
		return false;
	}

    private void UpdateSupportIndicators()        // FOR DIAMOND PIECE
    {
        if(!this.IsDiamond())
        {
            return;
        }

        // Adjacent list from PieceMono
        int x = (int)this.transform.position.x;
        int y = (int)this.transform.position.y;

        var adjacentList = new List<Tuple<int, int>>();
        adjacentList.Add(new Tuple<int, int>(x + 1, y)); // E
        adjacentList.Add(new Tuple<int, int>(x - 1, y)); // W
        adjacentList.Add(new Tuple<int, int>(x, y + 1)); // N
        adjacentList.Add(new Tuple<int, int>(x, y - 1)); // S
        adjacentList.Add(new Tuple<int, int>(x + 1, y + 1)); // NE
        adjacentList.Add(new Tuple<int, int>(x - 1, y + 1)); // NW
        adjacentList.Add(new Tuple<int, int>(x + 1, y - 1)); // SE
        adjacentList.Add(new Tuple<int, int>(x - 1, y - 1)); // SW


        foreach (Tuple<int, int> coord in adjacentList)
        {
            int ax = coord.Item1;
            int ay = coord.Item2;

            if (LevelMono.Instance.GetPiece(coord) != null && !this.IsEnemyOf(LevelMono.Instance.GetPiece(coord)))
            {
                // Adjacent ally is circle
                if (LevelMono.Instance.GetPiece(coord).IsCircle())
                {
                    if (ax == x + 1 && ay == y)      // East
                    {
                        if (!eSupport.activeInHierarchy) { eSupport.SetActive(true); }
                    }
                    if (ax == x - 1 && ay == y)      // West
                    {
                        if (!wSupport.activeInHierarchy) { wSupport.SetActive(true); }
                    }
                    if (ax == x && ay == y + 1)      // North
                    {
                        if (!nSupport.activeInHierarchy) { nSupport.SetActive(true); }
                    }
                    if (ax == x && ay == y - 1)      // South
                    {
                        if (!sSupport.activeInHierarchy) { sSupport.SetActive(true); }
                    }

                    if (ax == x + 1 && ay == y + 1)      // NorthEast
                    {
                        if (!neSupport.activeInHierarchy) { neSupport.SetActive(true); }
                    }
                    if (ax == x - 1 && ay == y + 1)      // NorthWest
                    {
                        if (!nwSupport.activeInHierarchy) { nwSupport.SetActive(true); }
                    }
                    if (ax == x - 1 && ay == y - 1)      // SouthWest
                    {
                        if (!swSupport.activeInHierarchy) { swSupport.SetActive(true); }
                    }
                    if (ax == x + 1 && ay == y - 1)      // SouthEast
                    {
                        if (!seSupport.activeInHierarchy) { seSupport.SetActive(true); }
                    }
                }
                // Adjacent ally is not circle
                else
                {
                    if (ax == x + 1 && ay == y)      // East
                    {
                        eSupport.SetActive(false);
                    }
                    if (ax == x - 1 && ay == y)      // West
                    {
                        wSupport.SetActive(false);
                    }
                    if (ax == x && ay == y + 1)      // North
                    {
                        nSupport.SetActive(false);
                    }
                    if (ax == x && ay == y - 1)      // South
                    {
                        sSupport.SetActive(false);
                    }

                    if (ax == x + 1 && ay == y + 1)      // NorthEast
                    {
                        neSupport.SetActive(false);
                    }
                    if (ax == x - 1 && ay == y + 1)      // NorthWest
                    {
                        nwSupport.SetActive(false);
                    }
                    if (ax == x - 1 && ay == y - 1)      // SouthWest
                    {
                        swSupport.SetActive(false);
                    }
                    if (ax == x + 1 && ay == y - 1)      // SouthEast
                    {
                        seSupport.SetActive(false);
                    }
                }
            }
            // Empty adjacent tile
            else
            {
                if (ax == x + 1 && ay == y)      // East
                {
                    eSupport.SetActive(false);
                }
                if (ax == x - 1 && ay == y)      // West
                {
                    wSupport.SetActive(false);
                }
                if (ax == x && ay == y + 1)      // North
                {
                    nSupport.SetActive(false);
                }
                if (ax == x && ay == y - 1)      // South
                {
                    sSupport.SetActive(false);
                }

                if (ax == x + 1 && ay == y + 1)      // NorthEast
                {
                    neSupport.SetActive(false);
                }
                if (ax == x - 1 && ay == y + 1)      // NorthWest
                {
                    nwSupport.SetActive(false);
                }
                if (ax == x - 1 && ay == y - 1)      // SouthWest
                {
                    swSupport.SetActive(false);
                }
                if (ax == x + 1 && ay == y - 1)      // SouthEast
                {
                    seSupport.SetActive(false);
                }
            }
        }
    }

    private void Update()
    {
        if(this.IsDiamond())
        {
            UpdateSupportIndicators();
        }

        if(this.IsCircle())
        {
            if(this.IsAdjacentToAllyDiamond())
            {
                buffParticles.SetActive(true);
            }
            else
            {
                buffParticles.SetActive(false);
            }
        }
    }
}