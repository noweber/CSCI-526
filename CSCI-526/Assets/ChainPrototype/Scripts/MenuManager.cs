using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    //[SerializeField] private GameObject _selectedHeroObject, _tileObject, _tileUnitObject, _turnInfoObject;
    [SerializeField] public GameObject _turnInfoObject, _selectedUnitObject, _selectUnitInfo, _numTurnObject, _abilityUseObject, _endTurnObject;

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
            _turnInfoObject.GetComponentInChildren<Text>().text = "Red Turn";
        } else
        {
            _turnInfoObject.GetComponentInChildren<Text>().text = "Blue Turn";
        }
    }

    public void ShowNumMovesInfo()
    {
        
        _numTurnObject.GetComponentInChildren<Text>().text = "" + (2 - GameManager.Instance.NumMoves) + " Moves Left";
        /*
        if ((2 - GameManager.Instance.NumMoves) != 0)
        {
            _numTurnObject.GetComponentInChildren<Text>().text = "" + (2 - GameManager.Instance.NumMoves) + " Moves Left";
        }
        else
        {
            _numTurnObject.GetComponentInChildren<Text>().text = "" + (2) + " Moves Left";
        }
        */

    }
    public void ShowEndTurnButton()
    {
        _endTurnObject.GetComponentInChildren<Button>().onClick.AddListener(() => EndTurnEvent());
    }

    public void EndTurnEvent()//(Piece triangle, Piece other)
    {
        Debug.Log("End Turn Event");
        foreach (var piece in GameManager.Instance.MovedPieces)
        {
            piece.hasMoved = false;
        }
        GameManager.Instance.MovedPieces = new List<Piece>();
        GameManager.Instance.NumMoves = 0;
        GameManager.Instance.UsedAbility = false;
        var turn = GameManager.Instance.GameState == GameState.White ? true : false;
        if (turn == true)
        {
            Debug.Log(GameManager.Instance.GameState);
            GameManager.Instance.ChangeState(GameState.Black);
        }
        else
        {
            Debug.Log(GameManager.Instance.GameState);
            GameManager.Instance.ChangeState(GameState.White);
        }
    }

    public void ShowAbilityButton()
    {
        Debug.Log("Ability Open time");
        _abilityUseObject.SetActive(true);

        //var triangle = GridManager.Instance.storedPiece;
        _abilityUseObject.GetComponentInChildren<Button>().onClick.AddListener(() => TriangleEvent());
    }

    public void HideAbilityButton()
    {
        Debug.Log("Ability Close time");
        _abilityUseObject.SetActive(false);
    }

    public void TriangleEvent()//(Piece triangle, Piece other)
    {
        Debug.Log("BUTTON WORKS");
        if (GameManager.Instance.UsedAbility == false)
        {
            GameManager.Instance.NumMoves -= 1;
            GameManager.Instance.UsedAbility = true;
            this.ShowNumMovesInfo();
        }
        this.HideAbilityButton();
    }

    public void ShowUnitInfo(Piece piece)
    {
        if (piece == null)
        {
            _selectedUnitObject.SetActive(false);
            _selectUnitInfo.SetActive(false);
            return;
        }
        _selectedUnitObject.GetComponentInChildren<Text>().text = piece.unitName;
        _selectUnitInfo.GetComponentInChildren<Text>().text = piece.unitInfo;
        _selectedUnitObject.SetActive(true);
        _selectUnitInfo.SetActive(true);
    }

    public void HideUnitInfo(Piece piece)
    {
        if (piece == null)
        {
            _selectedUnitObject.SetActive(false);
            _selectUnitInfo.SetActive(false);
            return;
        }
        _selectedUnitObject.GetComponentInChildren<Text>().text = "Unit Name";
        _selectUnitInfo.GetComponentInChildren<Text>().text = "Unit Info";
        _selectedUnitObject.SetActive(false);
        _selectUnitInfo.SetActive(false);
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
