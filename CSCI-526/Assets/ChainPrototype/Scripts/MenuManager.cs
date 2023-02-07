using System;
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
        if (GameManagerChain.Instance.GameStateEnum == GameStateEnum.Human)
        {
            _turnInfoObject.GetComponentInChildren<Text>().text = "Blue Turn";
        } else
        {
            _turnInfoObject.GetComponentInChildren<Text>().text = "Red Turn";
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
            piece.SetMoveState(false);
        }
        GameManagerChain.Instance.MovedPieces = new List<PieceController>();
        GameManagerChain.Instance.NumMoves = 0;
        GameManagerChain.Instance.UsedAbility = false;
        var turn = GameManagerChain.Instance.GameStateEnum == GameStateEnum.Human ? true : false;
        if (turn == true)
        {
            Debug.Log(GameManagerChain.Instance.GameStateEnum);
            GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
        }
        else
        {
            Debug.Log(GameManagerChain.Instance.GameStateEnum);
            GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
        }
		if (LevelController.Instance.storedPiece != null && LevelController.Instance.storedPiece.highlightedMoves.Count > 0) 
		{
			foreach (Tuple<int, int> tileCoords in LevelController.Instance.storedPiece.highlightedMoves)
    		{
       			LevelController.Instance.tiles[tileCoords]._highlight.SetActive(false);
    		}
		}	
        if (LevelController.Instance.storedPiece != null)
        {
            LevelController.Instance.storedPiece.highlightedMoves.Clear();
            LevelController.Instance.storedPiece = null;
            LevelController.Instance.storedCoord = new Tuple<int, int>(-1, -1);
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

    public void ShowUnitInfo(PieceController piece)
    {
        if (piece == null)
        {
            _selectedUnitObject.SetActive(false);
            _selectUnitInfo.SetActive(false);
            return;
        }
        _selectedUnitObject.GetComponentInChildren<Text>().text = piece.Name();
        _selectUnitInfo.GetComponentInChildren<Text>().text = piece.Summary();
        _selectedUnitObject.SetActive(true);
        _selectUnitInfo.SetActive(true);
    }

    public void HideUnitInfo(PieceController piece)
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
}
