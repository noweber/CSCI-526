using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(GridManager.Instance.storedPiece != null)
        {
            var highlightTiles = GridManager.Instance.storedPiece.highlightedMoves;
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
        }
        else
        {
            _highlight.SetActive(false);
        }
        //_highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        var clickedPiece = GridManager.Instance.GetPiece(new Tuple<int, int>((int)this.transform.position.x,(int)this.transform.position.y));
        Debug.Log(clickedPiece);
        var coord = new Tuple<int, int>((int)this.transform.position.x, (int)this.transform.position.y);
		var turn = GameManagerChain.Instance.GameStateEnum == GameStateEnum.White ? true : false;

        if (clickedPiece != null) // selected piece is correct turn's color
        {
            if (GridManager.Instance.storedPiece == null && turn == clickedPiece.isWhite && clickedPiece.hasMoved == false)
            {
                //Selects Piece
                Debug.Log(GameManagerChain.Instance.NumMoves);
                Debug.Log("TOTAL MOVES: " + GameManagerChain.Instance.TotalMoves);
                GridManager.Instance.storedPiece = clickedPiece;
                GridManager.Instance.storedCoord = coord;
 
                GridManager.Instance.storedPiece.highlightedMoves = clickedPiece.LegalMoves(GridManager.Instance._width, GridManager.Instance._height);
                
                if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1 && clickedPiece.unitName == "Circle") 
                {
                    GridManager.Instance.storedPiece.highlightedMoves.Add(new Tuple<int, int>(1, 1));
                    GridManager.Instance.storedPiece.highlightedMoves.Add(new Tuple<int, int>(2, 2));
                } 
                
                foreach (Tuple<int, int> tileCoords in GridManager.Instance.storedPiece.highlightedMoves)
                {
                    GridManager.Instance.tiles[tileCoords]._highlight.SetActive(true);
                    if (tileCoords.Item1 == 1 && tileCoords.Item2 == 1 && GameManagerChain.Instance.TotalMoves == 1 ) { GridManager.Instance.tiles[tileCoords]._highlight.GetComponent<SpriteRenderer>().color = new Color32(200, 100, 70, 255); }
                    if (tileCoords.Item1 == 2 && tileCoords.Item2 == 2 && GameManagerChain.Instance.TotalMoves == 1 ) { GridManager.Instance.tiles[tileCoords]._highlight.GetComponent<SpriteRenderer>().color = new Color32(200,100,70,255); }
                }
                MenuManager.Instance.ShowUnitInfo(clickedPiece);
                GameManagerChain.Instance.MovedPieces.Add(clickedPiece);
            }
            else
            {
                if (GridManager.Instance.storedPiece != null)
                {
                    // Possible to capture clickedPiece
                    if (GridManager.Instance.storedPiece != clickedPiece && GridManager.Instance.storedPiece.isWhite == clickedPiece.isWhite)
                    {

                    }
                    else if (GridManager.Instance.storedPiece != clickedPiece && GridManager.Instance.storedPiece.isWhite != clickedPiece.isWhite) 
					{
                    	if (GridManager.Instance.MovePiece(coord, GridManager.Instance.storedPiece))
                    	{
                            //TODO: add LEVELMODEL update for capture
							// Capturing Piece 
                        	Destroy(clickedPiece.gameObject);
                        	GameManagerChain.Instance.NumMoves += 1;
                            GameManagerChain.Instance.TotalMoves += 1;

                            if (SceneManager.GetActiveScene().name == "TutorialLevel")
                            {
                                MenuManager.Instance.UpdateObjectiveContent();
                            }

                            MenuManager.Instance.ShowNumMovesInfo();
                            //If Unit that Captured a piece is Circle, gain another turn
                            if (GridManager.Instance.storedPiece.unitName != "Circle")
                            {
                                Debug.Log("Piece that captured is NOT a circle");
                                GridManager.Instance.storedPiece.hasMoved = true;
                            }

                            //GridManager.Instance.storedPiece.hasMoved = true;
                        }
                    }
					foreach (Tuple<int, int> tileCoords in GridManager.Instance.storedPiece.highlightedMoves)
					{
						GridManager.Instance.tiles[tileCoords]._highlight.SetActive(false);
                        GridManager.Instance.tiles[tileCoords]._highlight.GetComponent<SpriteRenderer>().color =
                            new Color32(255, 255, 255, 100);
                    }
                    MenuManager.Instance.HideAbilityButton();
                    MenuManager.Instance.HideUnitInfo(GridManager.Instance.storedPiece);
                    GridManager.Instance.storedPiece = null;
                    GridManager.Instance.storedCoord = new Tuple<int, int>(-1, -1);
                }
			}
        }
        else
        {
            Debug.Log(GridManager.Instance.storedPiece);
            Debug.Log(clickedPiece);
            if (GridManager.Instance.storedPiece != null)
            {
                //Move Piece
                if (GridManager.Instance.MovePiece(coord, GridManager.Instance.storedPiece))
                {
                    GridManager.Instance.storedPiece.hasMoved = true;
                    foreach (Tuple<int, int> tileCoords in GridManager.Instance.storedPiece.highlightedMoves)
                    {
                        GridManager.Instance.tiles[tileCoords]._highlight.SetActive(false);
                        GridManager.Instance.tiles[tileCoords]._highlight.GetComponent<SpriteRenderer>().color =
                            new Color32(255, 255, 255, 100);
                        //fix hover unhighlight while selected
                    }
                    MenuManager.Instance.HideAbilityButton();
                    MenuManager.Instance.HideUnitInfo(GridManager.Instance.storedPiece);
                    GridManager.Instance.storedPiece = null;
                    GridManager.Instance.storedCoord = new Tuple<int, int>(-1, -1);
                    GameManagerChain.Instance.NumMoves += 1;
                    GameManagerChain.Instance.TotalMoves += 1;
                    MenuManager.Instance.ShowNumMovesInfo();

                    if(SceneManager.GetActiveScene().name == "TutorialLevel")
                    {
                        MenuManager.Instance.UpdateObjectiveContent();
                    }
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
                if(SceneManager.GetActiveScene().name == "TutorialLevel")
                {
                    // Delayed switch back from black for tutorial
                    GameManagerChain.Instance.ChangeState(GameStateEnum.Black);
                    StartCoroutine(DelayedChangeState());
                }
                else
                {
                    GameManagerChain.Instance.ChangeState(GameStateEnum.Black);
                }
            } else
			{
				GameManagerChain.Instance.ChangeState(GameStateEnum.White);
			}
			foreach (var piece in GameManagerChain.Instance.MovedPieces) 
			{
				piece.hasMoved = false;
			}
			GameManagerChain.Instance.UsedAbility = false;
			GameManagerChain.Instance.MovedPieces = new List<Piece>();
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

    // "Slacking off" text for 3 seconds, then change state to white
    private IEnumerator DelayedChangeState()
    {
        MenuManager.Instance.SetSlackDialogue(true);
        yield return new WaitForSeconds(2);
        MenuManager.Instance.SetSlackDialogue(false);
        GameManagerChain.Instance.ChangeState(GameStateEnum.White);
        yield return null;
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
