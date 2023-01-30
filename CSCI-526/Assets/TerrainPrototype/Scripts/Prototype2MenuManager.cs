using UnityEngine;
using UnityEngine.UI;

public class Prototype2MenuManager : MonoBehaviour
{
    public static Prototype2MenuManager Instance;

    [SerializeField] private GameObject _selectedHeroObject, _selectedEnemyObject, _selectedTurnPhaseObject, _tileObject, _tileUnitObject, _spellCastObject, _moveUnitNotificationObject;

    void Awake()
    {
        Instance = this;
        _selectedTurnPhaseObject.SetActive(true);
    }

    public void ShowTileInfo(TerrainTile tile)
    {

        if (tile == null)
        {
            _tileObject.SetActive(false);
            _tileUnitObject.SetActive(false);
            return;
        }

        _tileObject.GetComponentInChildren<Text>().text = "Terrain: " + tile.TerrainType.ToString() + "\nElements: " + tile.FirstElement.ToString() + " + " + tile.SecondElement.ToString();
        _tileObject.SetActive(true);

        if (tile.Occupant)
        {
            _tileUnitObject.GetComponentInChildren<Text>().text = tile.Occupant.UnitName;
            _tileUnitObject.SetActive(true);
        }
    }
    public void ShowSelectedHero(BaseHero hero)
    {
        if (hero == null)
        {
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

    public void ShowSpellUi()
    {
        _spellCastObject.SetActive(true);
    }

    public void HideSpellUi()
    {
        _spellCastObject.SetActive(false);
    }

    public void ShowMoveUi()
    {
        _moveUnitNotificationObject.SetActive(true);
    }

    public void HideMoveUi()
    {
        _moveUnitNotificationObject.SetActive(false);
    }
}