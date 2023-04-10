using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;       // IEnumerator
using UnityEngine.Tilemaps;

public abstract class PieceMono : MonoBehaviour
{
	protected bool isHuman;
	
	protected bool hasMoved;

	public string UnitName { get; protected set; }

    public const string Circle = "Fighter";

    public const string Diamond = "Support";

    public const string Triangle = "Satellite";

    public const string Scout = "Scout";

    public const string Base = "Planet";

    // [SerializeField] private GameObject nSupport, wSupport, sSupport, eSupport, nwSupport, neSupport, swSupport, seSupport;     // Diamond support indicators
    [SerializeField] private GameObject nPartPlayer, nPartEnemy, wPartPlayer, wPartEnemy, sPartPlayer, sPartEnemy, ePartPlayer, ePartEnemy,
                                        nePartPlayer, nePartEnemy, nwPartPlayer, nwPartEnemy, swPartPlayer, swPartEnemy, sePartPlayer, sePartEnemy;

    [SerializeField] private ParticleSystem buffParticles;      // Circle indicator when it is buffed by Diamond

    public GameObject canMoveObject, cantMoveObject;     // Highlight will be temporary -- change to particles after midterm

    [SerializeField] private GameObject enemySightIndicator;        // Display when the enemy is able to see the player unit
    public abstract string getUnitInfo();

	public void SetHuman(bool isHuman) { this.isHuman = isHuman; }

	public void SetName(string name) { this.UnitName = name; }

	public void SetMoveState(bool state) { this.hasMoved = state; }

	public bool IsEnemyOf(PieceMono piece) { return this.IsHuman() != piece.IsHuman(); }

	public bool CanMove() { return !this.hasMoved; }

	public bool IsHuman() { return this.isHuman; }

	public bool IsCircle() { return string.Equals(this.UnitName, Circle); }

	public bool IsTriangle() { return string.Equals(this.UnitName, Triangle); }

	public bool IsDiamond() { return string.Equals(this.UnitName, Diamond); }

	public bool IsScout() { return string.Equals(this.UnitName, Scout); }

    public bool IsBase() { return string.Equals(this.UnitName, Base); }

	public string GetName() { return this.UnitName; }

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

    private void UpdateDiamondIndicators()        // FOR DIAMOND PIECE
    {
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
                if(this.IsHuman())
                {
                    // Adjacent ally is circle or scout
                    if ((LevelMono.Instance.GetPiece(coord).IsCircle() || LevelMono.Instance.GetPiece(coord).IsScout())
                        && LevelMono.Instance.tiles[coord].CanPlayerSee())
                    {
                        if (ax == x + 1 && ay == y)      // East
                        {
                            if (!ePartPlayer.activeInHierarchy)
                            {
                                ePartPlayer.SetActive(true);
                            }
                        }
                        if (ax == x - 1 && ay == y)      // West
                        {
                            if (!wPartPlayer.activeInHierarchy)
                            {
                                wPartPlayer.SetActive(true);
                            }
                        }
                        if (ax == x && ay == y + 1)      // North
                        {
                            if (!nPartPlayer.activeInHierarchy)
                            {
                                nPartPlayer.SetActive(true);

                            }
                        }
                        if (ax == x && ay == y - 1)      // South
                        {
                            if (!sPartPlayer.activeInHierarchy)
                            {
                                sPartPlayer.SetActive(true);

                            }
                        }

                        if (ax == x + 1 && ay == y + 1)      // NorthEast
                        {
                            if (!nePartPlayer.activeInHierarchy)
                            {
                                nePartPlayer.SetActive(true);

                            }
                        }
                        if (ax == x - 1 && ay == y + 1)      // NorthWest
                        {
                            if (!nwPartPlayer.activeInHierarchy)
                            {
                                nwPartPlayer.SetActive(true);
                            }
                        }
                        if (ax == x - 1 && ay == y - 1)      // SouthWest
                        {
                            if (!swPartPlayer.activeInHierarchy)
                            {
                                swPartPlayer.SetActive(true);
                            }
                        }
                        if (ax == x + 1 && ay == y - 1)      // SouthEast
                        {
                            if (!sePartPlayer.activeInHierarchy)
                            {
                                sePartPlayer.SetActive(true);
                            }
                        }
                    }
                    // Adjacent ally is not circle or scout
                    else
                    {
                        if (ax == x + 1 && ay == y)      // East
                        {
                            ePartPlayer.SetActive(false);
                        }
                        if (ax == x - 1 && ay == y)      // West
                        {
                            wPartPlayer.SetActive(false);
                        }
                        if (ax == x && ay == y + 1)      // North
                        {
                            nPartPlayer.SetActive(false);
                        }
                        if (ax == x && ay == y - 1)      // South
                        {
                            sPartPlayer.SetActive(false);
                        }

                        if (ax == x + 1 && ay == y + 1)      // NorthEast
                        {
                            nePartPlayer.SetActive(false);
                        }
                        if (ax == x - 1 && ay == y + 1)      // NorthWest
                        {
                            nwPartPlayer.SetActive(false);
                        }
                        if (ax == x - 1 && ay == y - 1)      // SouthWest
                        {
                            swPartPlayer.SetActive(false);
                        }
                        if (ax == x + 1 && ay == y - 1)      // SouthEast
                        {
                            sePartPlayer.SetActive(false);
                        }
                    }
                }
                else
                {
                    // Adjacent ally is circle or scout
                    if ((LevelMono.Instance.GetPiece(coord).IsCircle() || LevelMono.Instance.GetPiece(coord).IsScout())
                        && LevelMono.Instance.tiles[coord].CanPlayerSee())
                    {
                        if (ax == x + 1 && ay == y)      // East
                        {
                            if (!ePartEnemy.activeInHierarchy)
                            {
                                ePartEnemy.SetActive(true);
                            }
                        }
                        if (ax == x - 1 && ay == y)      // West
                        {
                            if (!wPartEnemy.activeInHierarchy)
                            {
                                wPartEnemy.SetActive(true);
                            }
                        }
                        if (ax == x && ay == y + 1)      // North
                        {
                            if (!nPartEnemy.activeInHierarchy)
                            {
                                nPartEnemy.SetActive(true);
                            }
                        }
                        if (ax == x && ay == y - 1)      // South
                        {
                            if (!sPartEnemy.activeInHierarchy)
                            {
                                sPartEnemy.SetActive(true);
                            }
                        }

                        if (ax == x + 1 && ay == y + 1)      // NorthEast
                        {
                            if (!nePartEnemy.activeInHierarchy)
                            {
                                nePartEnemy.SetActive(true);

                            }
                        }
                        if (ax == x - 1 && ay == y + 1)      // NorthWest
                        {
                            if (!nwPartEnemy.activeInHierarchy)
                            {
                                nwPartEnemy.SetActive(true);
                            }
                        }
                        if (ax == x - 1 && ay == y - 1)      // SouthWest
                        {
                            if (!swPartEnemy.activeInHierarchy)
                            {
                                swPartEnemy.SetActive(true);
                            }
                        }
                        if (ax == x + 1 && ay == y - 1)      // SouthEast
                        {
                            if (!sePartEnemy.activeInHierarchy)
                            {
                                sePartEnemy.SetActive(true);
                            }
                        }
                    }
                    // Adjacent ally is not circle or scout
                    else
                    {
                        if (ax == x + 1 && ay == y)      // East
                        {
                            ePartEnemy.SetActive(false);
                        }
                        if (ax == x - 1 && ay == y)      // West
                        {
                            wPartEnemy.SetActive(false);
                        }
                        if (ax == x && ay == y + 1)      // North
                        {
                            nPartEnemy.SetActive(false);
                        }
                        if (ax == x && ay == y - 1)      // South
                        {
                            sPartEnemy.SetActive(false);
                        }

                        if (ax == x + 1 && ay == y + 1)      // NorthEast
                        {
                            nePartEnemy.SetActive(false);
                        }
                        if (ax == x - 1 && ay == y + 1)      // NorthWest
                        {
                            nwPartEnemy.SetActive(false);
                        }
                        if (ax == x - 1 && ay == y - 1)      // SouthWest
                        {
                            swPartEnemy.SetActive(false);
                        }
                        if (ax == x + 1 && ay == y - 1)      // SouthEast
                        {
                            sePartEnemy.SetActive(false);
                        }
                    }
                }
                
            }
            // Empty adjacent tile
            else
            {
                if (ax == x + 1 && ay == y)      // East
                {
                    ePartEnemy.SetActive(false); ;
                    ePartPlayer.SetActive(false);
                }
                if (ax == x - 1 && ay == y)      // West
                {
                    wPartEnemy.SetActive(false); ;
                    wPartPlayer.SetActive(false);
                }
                if (ax == x && ay == y + 1)      // North
                {
                    nPartEnemy.SetActive(false); ;
                    nPartPlayer.SetActive(false);
                }
                if (ax == x && ay == y - 1)      // South
                {
                    sPartEnemy.SetActive(false); ;
                    sPartPlayer.SetActive(false);
                }

                if (ax == x + 1 && ay == y + 1)      // NorthEast
                {
                    nePartEnemy.SetActive(false); ;
                    nePartPlayer.SetActive(false);
                }
                if (ax == x - 1 && ay == y + 1)      // NorthWest
                {
                    nwPartEnemy.SetActive(false); ;
                    nwPartPlayer.SetActive(false);
                }
                if (ax == x - 1 && ay == y - 1)      // SouthWest
                {
                    swPartEnemy.SetActive(false); ;
                    swPartPlayer.SetActive(false);
                }
                if (ax == x + 1 && ay == y - 1)      // SouthEast
                {
                    sePartEnemy.SetActive(false); ;
                    sePartPlayer.SetActive(false);
                }
            }
        }
    }

    private void UpdateBuffIndicator()
    {
        if(this.IsHuman())
        {
            if(this.IsAdjacentToAllyDiamond())
            {
                if(!buffParticles.isPlaying)
                {
                    buffParticles.Play();
                }
            }
            else
            {
                buffParticles.Stop();
            }
        }
        else        // ENEMY
        {
            if(this.IsAdjacentToAllyDiamond())
            {
                foreach(var ally in this.AdjacentAllies())
                {
                    if(LevelMono.Instance.GetPiece(ally).IsDiamond() && LevelMono.Instance.tiles[ally].CanPlayerSee())
                    {
                        if (!buffParticles.isPlaying)
                        {
                            buffParticles.Play();
                        }
                    }
                    else
                    {
                        buffParticles.Stop();
                    }
                }
            }
        }
    }

    private void Update()
    {
        if(this.IsDiamond())
        {
            UpdateDiamondIndicators();
        }

        if(this.IsCircle() || this.IsScout())
        {
            UpdateBuffIndicator();
        }
        if(!LevelMono.Instance.debug)       // Debug mode will display all units and could bug this
        {
            if (this.IsHuman())      // Red "eye" when player is in enemy vision
            {
                if (!this.IsTriangle() && !this.IsBase())
                {
                    if (LevelMono.Instance.tiles[new Tuple<int, int>((int)this.transform.position.x, (int)this.transform.position.y)].CanEnemySee())
                    {
                        enemySightIndicator.SetActive(true);
                    }
                    else
                    {
                        enemySightIndicator.SetActive(false);
                    }
                }
            }
        }

    }
}