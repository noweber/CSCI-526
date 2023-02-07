using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _base, _offset;

    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] public GameObject _highlight;

    //[SerializeField] private GameObject _useAbility;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _base : _offset;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }
    
    void OnMouseExit()
    {
        if(LevelController.Instance.storedPiece != null)
        {
            // TODO: reconfigure this with IPiece
            var highlightTiles = LevelController.Instance.storedPiece.highlightedMoves;
            //Vector3 mousePos = Input.mousePosition;
            if (highlightTiles.Contains(new Tuple<int, int>((int)this.transform.position.x, (int)this.transform.position.y)))
            {
                //Debug.Log("IN THE ARRAY");
                _highlight.SetActive(true);
            }
            else
            {
                //Debug.Log("NOT IN THE ARRAY");
                _highlight.SetActive(false);
            }
            _highlight.SetActive(false);
        }
        else
        {
            _highlight.SetActive(false);
        }
        //_highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        var coord = new Tuple<int, int>((int)this.transform.position.x, (int)this.transform.position.y);
        var clickedPiece = LevelController.Instance.GetPiece(coord);
		var turn = GameManagerChain.Instance.GameStateEnum == GameStateEnum.Human ? true : false;
        if (clickedPiece != null) // selected piece is correct turn's color
        {
            if (LevelController.Instance.storedPiece == null && turn == clickedPiece.IsControlledByHuman() && clickedPiece.HasMoved() == false)
            {
                //Selects Piece
                LevelController.Instance.storedPiece = clickedPiece;
                LevelController.Instance.storedCoord = coord;
                LevelController.Instance.storedPiece.highlightedMoves = clickedPiece.GetLegalMoves(LevelController.Instance.LevelModel.GetWidth(), LevelController.Instance.LevelModel.GetHeight());
                foreach (Tuple<int, int> tileCoords in LevelController.Instance.storedPiece.highlightedMoves)
                {
                    LevelController.Instance.tiles[tileCoords]._highlight.SetActive(true);
                }
                MenuManager.Instance.ShowUnitInfo(clickedPiece);
                GameManagerChain.Instance.MovedPieces.Add(clickedPiece);
            }
            else
            {
                if (LevelController.Instance.storedPiece != null)
                {
                    // Possible to capture clickedPiece
                    if (LevelController.Instance.storedPiece != clickedPiece && LevelController.Instance.storedPiece.IsControlledByHuman() != clickedPiece.IsControlledByHuman()) 
					{
                    	if (LevelController.Instance.MovePiece(coord, LevelController.Instance.storedPiece))
                    	{
                            //TODO: add LEVELMODEL update for capture
							// Capturing Piece 
                        	// Destroy(clickedPiece.gameObject);
                        	GameManagerChain.Instance.NumMoves += 1;
                            MenuManager.Instance.ShowNumMovesInfo();
                            //If Unit that Captured a piece is Circle, gain another turn
                            if (LevelController.Instance.storedPiece.Name() != "Circle")
                            {
                                Debug.Log("Piece that captured is NOT a circle");
                                LevelController.Instance.storedPiece.SetMoveState(true);
                            }

                            //GridManager.Instance.storedPiece.hasMoved = true;
                        }
                    }
					foreach (Tuple<int, int> tileCoords in LevelController.Instance.storedPiece.highlightedMoves)
					{
						LevelController.Instance.tiles[tileCoords]._highlight.SetActive(false);
                    }
                    MenuManager.Instance.HideAbilityButton();
                    MenuManager.Instance.HideUnitInfo(LevelController.Instance.storedPiece);
                    LevelController.Instance.storedPiece = null;
                    LevelController.Instance.storedCoord = new Tuple<int, int>(-1, -1);
                }
			}
        }
        else
        {
            Debug.Log(LevelController.Instance.storedPiece);
            Debug.Log(clickedPiece);
            if (LevelController.Instance.storedPiece != null)
            {
                //Move Piece
                if (LevelController.Instance.MovePiece(coord, LevelController.Instance.storedPiece))
                {
                    LevelController.Instance.storedPiece.SetMoveState(true);
                    foreach (Tuple<int, int> tileCoords in LevelController.Instance.storedPiece.highlightedMoves)
                    {
                        LevelController.Instance.tiles[tileCoords]._highlight.SetActive(false);
                        //fix hover unhighlight while selected
                    }
                    MenuManager.Instance.HideAbilityButton();
                    MenuManager.Instance.HideUnitInfo(LevelController.Instance.storedPiece);
                    LevelController.Instance.storedPiece = null;
                    LevelController.Instance.storedCoord = new Tuple<int, int>(-1, -1);
                    GameManagerChain.Instance.NumMoves += 1;
                    MenuManager.Instance.ShowNumMovesInfo();
                    //unhighlight after move.
                }
            }
        }

		// turn logic
		if (GameManagerChain.Instance.NumMoves == 2) 
		{
			
			GameManagerChain.Instance.NumMoves = 0;
			if (turn == true) 
			{
				GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
			} else
			{
				GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
			}
			foreach (var piece in GameManagerChain.Instance.MovedPieces) 
			{
				piece.SetMoveState(false);
			}
			GameManagerChain.Instance.UsedAbility = false;
			GameManagerChain.Instance.MovedPieces = new List<PieceController>();
        }
        //Finds valid piece (done)
        //Calls GridManager/sends to GridManager --> initialize some function to prepare for movement/store the piece
        //Piece in GridManager--> var selectedPiece = piece or null --> set as True. if itself, then set null/cancel action

        //next click = move? --> if selectedPiece == true, means we can move
        //update GridManager Dictionaries
        //update stored piece = null


        //extra: highlight valid spots to move for specific piece
        //_highlight.SetActive(true)?
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
