using UnityEngine;
using UnityEngine.UI;

public class Prototype2MenuManager : MonoBehaviour
{
    public static Prototype2MenuManager Instance;

    [SerializeField] private GameObject _selectedHeroObject, _selectedEnemyObject, _selectedTurnPhaseObject, _tileObject, _tileUnitObject;

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

        _tileObject.GetComponentInChildren<Text>().text = tile.TerrainType.ToString();
        _tileObject.SetActive(true);

        /* TODO
        if (tile.OccupiedUnit)
        {
            _tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.UnitName;
            _tileUnitObject.SetActive(true);
        }*/
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