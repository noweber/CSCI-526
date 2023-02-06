using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    //[SerializeField] private GameObject _selectedHeroObject, _tileObject, _tileUnitObject, _turnInfoObject;
    [SerializeField] public GameObject _turnInfoObject, _selectedUnitObject, _selectUnitInfo, _numTurnObject, _abilityUseObject, _endTurnObject, _objectiveContent, _slackObject;

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
        if (GameManagerChain.Instance.GameStateEnum == GameStateEnum.White)
        {
            _turnInfoObject.GetComponentInChildren<Text>().text = "Red's Turn";
        } else
        {
            _turnInfoObject.GetComponentInChildren<Text>().text = "Blue's Turn";
        }
    }

    public void ShowNumMovesInfo()
    {
        
        _numTurnObject.GetComponentInChildren<Text>().text = "" + (2 - GameManagerChain.Instance.NumMoves) + " Moves Left";
        /*
        if ((2 - GameManagerChain.Instance.NumMoves) != 0)
        {
            _numTurnObject.GetComponentInChildren<Text>().text = "" + (2 - GameManagerChain.Instance.NumMoves) + " Moves Left";
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
        foreach (var piece in GameManagerChain.Instance.MovedPieces)
        {
            piece.hasMoved = false;
        }
        GameManagerChain.Instance.MovedPieces = new List<Piece>();
        GameManagerChain.Instance.NumMoves = 0;
        GameManagerChain.Instance.UsedAbility = false;
        var turn = GameManagerChain.Instance.GameStateEnum == GameStateEnum.White ? true : false;
        if (turn == true)
        {
            Debug.Log(GameManagerChain.Instance.GameStateEnum);
            GameManagerChain.Instance.ChangeState(GameStateEnum.Black);
        }
        else
        {
            Debug.Log(GameManagerChain.Instance.GameStateEnum);
            GameManagerChain.Instance.ChangeState(GameStateEnum.White);
        }
		if (GridManager.Instance.storedPiece != null && GridManager.Instance.storedPiece.highlightedMoves.Count > 0) 
		{
			foreach (Tuple<int, int> tileCoords in GridManager.Instance.storedPiece.highlightedMoves)
    		{
       			GridManager.Instance.tiles[tileCoords]._highlight.SetActive(false);
    		}
		}	
        if (GridManager.Instance.storedPiece != null)
        {
            GridManager.Instance.storedPiece.highlightedMoves.Clear();
            GridManager.Instance.storedPiece = null;
            GridManager.Instance.storedCoord = new Tuple<int, int>(-1, -1);
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
        if (GameManagerChain.Instance.UsedAbility == false)
        {
            GameManagerChain.Instance.NumMoves -= 1;
            GameManagerChain.Instance.UsedAbility = true;
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

    public void UpdateObjectiveContent()
    {
        TextMeshProUGUI tmpro = _objectiveContent.GetComponent<TextMeshProUGUI>();

        if(SceneManager.GetActiveScene().name == "TutorialLevel")
        switch(GameManagerChain.Instance.TotalMoves)
        {
                case 0:     // First move -- player must move diamond to the circle
                    tmpro.text = "<color=black>Let's move the diamond next to the circle.</color>";
                    break;
                case 1:     // Second move -- player must move circle next to triangle, directly in front of enemy
                    tmpro.text = "<color=black>The circle is the main attacker for your team, so let's move the circle closer to the enemy (blue) units.</color>";
                    break;
                case 2:     // Free movement -- player freely maneuvers
                    tmpro.text = "<color=black>Capture the enemy's pieces.</color>";
                    break;
        }
    }
    public void SetSlackDialogue(bool status)
    {
        _slackObject.SetActive(status);
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
        if(SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            MenuManager.Instance.UpdateObjectiveContent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
