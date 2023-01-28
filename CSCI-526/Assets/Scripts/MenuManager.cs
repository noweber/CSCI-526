using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    //[SerializeField] private GameObject _selectedHeroObject, _tileObject, _tileUnitObject, _turnInfoObject;
    [SerializeField] public GameObject  _turnInfoObject, _selectedUnitObject;

    public MenuManager()
    {
        Instance = this;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void ShowTurnInfo()
    {
        if (GameManager.Instance.GameState == GameState.White)
        {
            _turnInfoObject.GetComponentInChildren<Text>().text = "White Turn";
        } else
        {
            _turnInfoObject.GetComponentInChildren<Text>().text = "Black Turn";
        }
    }
    public void ShowUnitInfo(Piece piece)
    {
        if (piece == null)
        {
            _selectedUnitObject.SetActive(false);
            return;
        }
        _selectedUnitObject.GetComponentInChildren<Text>().text = piece.unitName;
        _selectedUnitObject.SetActive(true);
    }

    public void HideUnitInfo(Piece piece)
    {
        if (piece == null)
        {
            _selectedUnitObject.SetActive(false);
            return;
        }
        _selectedUnitObject.GetComponentInChildren<Text>().text = "UnitInfo";
        _selectedUnitObject.SetActive(false);
    }

    /*
    public void ShowTileInfo(Tile tile)
    {

        if (tile == null)
        {
            _tileObject.SetActive(false);
            _tileUnitObject.SetActive(false);
            return;
        }

        _tileObject.GetComponentInChildren<Text>().text = tile.TileName;
        _tileObject.SetActive(true);

        if (tile.OccupiedUnit)
        {
            _tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.UnitName;
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
    */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
