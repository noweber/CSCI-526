using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;

    [SerializeField] private GameObject _selectedHeroObject,_selectedEnemyObject, _selectedTurnPhaseObject, _tileObject, _tileUnitObject;

    void Awake() {
        Instance = this;
        _selectedTurnPhaseObject.SetActive(true);
    }

    public void ShowTileInfo(Tile tile) {

        if (tile == null)
        {
            _tileObject.SetActive(false);
            _tileUnitObject.SetActive(false);
            return;
        }

        //Unsure why all grassland tiles are instantiated with (0,0) position
        _tileObject.GetComponentInChildren<Text>().text = tile.TileName; //+ "Pos:(" + tile._tilePosition.x + ", " + tile._tilePosition.y + ")";
        _tileObject.SetActive(true);

        if (tile.OccupiedUnit) {
            _tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.UnitName;
            _tileUnitObject.SetActive(true);
        }
    }

    public void ShowSelectedHero(BaseHero hero) {
        if (hero == null) {
            _selectedHeroObject.SetActive(false);
            return;
        }

        _selectedHeroObject.GetComponentInChildren<Text>().text = hero.UnitName;
        _selectedHeroObject.SetActive(true);
    }

    public void ShowSelectedEnemy(BaseEnemy enemy)
    {
        if (enemy == null)
        {
            _selectedEnemyObject.SetActive(false);
            return;
        }

        _selectedEnemyObject.GetComponentInChildren<Text>().text = enemy.UnitName;
        _selectedEnemyObject.SetActive(true);
    }

    //Hero is BLACK, Enemy is WHITE for now
    public void ShowPlayerBLACKPhase()
    {
        _selectedTurnPhaseObject.GetComponentInChildren<Text>().text = "Player BLACK's turn";
    }

    public void ShowPlayerWHITEPhase()
    {
        _selectedTurnPhaseObject.GetComponentInChildren<Text>().text = "Player WHITE's turn";
    }
}
