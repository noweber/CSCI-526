using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZenTile : MonoBehaviour {
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
        ZenMenuManager.Instance.ShowTileInfo(this);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        ZenMenuManager.Instance.ShowTileInfo(null);
    }

    void OnMouseDown() {
        //If it's enemy(team WHITE)'s turn
        if(ZenGameManager.Instance.GameState == GameState.EnemiesTurn)
        {
            if (OccupiedUnit != null)
            {
                if (OccupiedUnit.Faction == Faction.Enemy) ZenUnitManager.Instance.SetSelectedEnemy((BaseEnemy)OccupiedUnit);
                else
                {
                    if (ZenUnitManager.Instance.SelectedEnemy != null)
                    {
                        var hero = (BaseHero)OccupiedUnit;
                        SetUnit(ZenUnitManager.Instance.SelectedEnemy);
                        Destroy(hero.gameObject);
                        ZenUnitManager.Instance.SetSelectedEnemy(null);

                        //Change to enemy's turn
                        ZenMenuManager.Instance.ShowPlayerBLACKPhase();
                        ZenGameManager.Instance.ChangeState(GameState.HeroesTurn);
                    }
                }
            }
            else
            {
                //Check if the targeted tile is walkable
                if (_isWalkable)
                {
                    //Set the targeted tile to the selected hero's position and move prefab over
                    if (ZenUnitManager.Instance.SelectedEnemy != null)
                    {
                        SetUnit(ZenUnitManager.Instance.SelectedEnemy);
                        ZenUnitManager.Instance.SetSelectedEnemy(null);

                        //Change to enemy's turn
                        ZenMenuManager.Instance.ShowPlayerBLACKPhase();
                        ZenGameManager.Instance.ChangeState(GameState.HeroesTurn);
                    }
                }
            }
        }

        //If it's hero(team 2)'s turn
        if (ZenGameManager.Instance.GameState == GameState.HeroesTurn)
        {
            if (OccupiedUnit != null)
            {
                if (OccupiedUnit.Faction == Faction.Hero) ZenUnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
                else
                {
                    if (ZenUnitManager.Instance.SelectedHero != null)
                    {
                        var enemy = (BaseEnemy)OccupiedUnit;
                        SetUnit(ZenUnitManager.Instance.SelectedHero);
                        Destroy(enemy.gameObject);
                        ZenUnitManager.Instance.SetSelectedHero(null);

                        //Change to enemy's turn
                        ZenMenuManager.Instance.ShowPlayerWHITEPhase();
                        ZenGameManager.Instance.ChangeState(GameState.EnemiesTurn);
                    }
                }
            }
            else
            {
                //Check if the targeted tile is walkable
                if (_isWalkable)
                {
                    //Set the targeted tile to the selected hero's position and move prefab over
                    if (ZenUnitManager.Instance.SelectedHero != null)
                    {
                        SetUnit(ZenUnitManager.Instance.SelectedHero);
                        ZenUnitManager.Instance.SetSelectedHero(null);

                        //Change to enemy's turn
                        ZenMenuManager.Instance.ShowPlayerWHITEPhase();
                        ZenGameManager.Instance.ChangeState(GameState.EnemiesTurn);
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