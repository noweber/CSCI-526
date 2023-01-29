using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour {
    public string TileName;
    public Vector2 _tilePosition;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;
    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;


    public virtual void Init(int x, int y)
    {
        _tilePosition.x = x;
        _tilePosition.y = y;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    void OnMouseDown() {
        //If it's enemy(team WHITE)'s turn
        if(GameManager.Instance.GameState == GameState.EnemiesTurn)
        {
            if (OccupiedUnit != null)
            {
                if (OccupiedUnit.Faction == Faction.Enemy) UnitManager.Instance.SetSelectedEnemy((BaseEnemy)OccupiedUnit);
                else
                {
                    if (UnitManager.Instance.SelectedEnemy != null)
                    {
                        var hero = (BaseHero)OccupiedUnit;
                        SetUnit(UnitManager.Instance.SelectedEnemy);
                        Destroy(hero.gameObject);
                        UnitManager.Instance.SetSelectedEnemy(null);

                        //Change to enemy's turn
                        MenuManager.Instance.ShowPlayerBLACKPhase();
                        GameManager.Instance.ChangeState(GameState.HeroesTurn);
                    }
                }
            }
            else
            {
                //Check if the targeted tile is walkable
                if (_isWalkable)
                {
                    //Set the targeted tile to the selected hero's position and move prefab over
                    if (UnitManager.Instance.SelectedEnemy != null)
                    {
                        SetUnit(UnitManager.Instance.SelectedEnemy);
                        UnitManager.Instance.SetSelectedEnemy(null);

                        //Change to enemy's turn
                        MenuManager.Instance.ShowPlayerBLACKPhase();
                        GameManager.Instance.ChangeState(GameState.HeroesTurn);
                    }
                }
            }
        }

        //If it's hero(team 2)'s turn
        if (GameManager.Instance.GameState == GameState.HeroesTurn)
        {
            if (OccupiedUnit != null)
            {
                if (OccupiedUnit.Faction == Faction.Hero) UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
                else
                {
                    if (UnitManager.Instance.SelectedHero != null)
                    {
                        var enemy = (BaseEnemy)OccupiedUnit;
                        SetUnit(UnitManager.Instance.SelectedHero);
                        Destroy(enemy.gameObject);
                        UnitManager.Instance.SetSelectedHero(null);

                        //Change to enemy's turn
                        MenuManager.Instance.ShowPlayerWHITEPhase();
                        GameManager.Instance.ChangeState(GameState.EnemiesTurn);
                    }
                }
            }
            else
            {
                //Check if the targeted tile is walkable
                if (_isWalkable)
                {
                    //Set the targeted tile to the selected hero's position and move prefab over
                    if (UnitManager.Instance.SelectedHero != null)
                    {
                        SetUnit(UnitManager.Instance.SelectedHero);
                        UnitManager.Instance.SetSelectedHero(null);

                        //Change to enemy's turn
                        MenuManager.Instance.ShowPlayerWHITEPhase();
                        GameManager.Instance.ChangeState(GameState.EnemiesTurn);
                    }
                }
            }
        }
    }

    public void SetUnit(BaseUnit unit) {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
}