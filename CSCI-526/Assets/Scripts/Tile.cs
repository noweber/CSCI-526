using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _base, _offset;

    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private GameObject _highlight;

    
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
            if (highlightTiles.Contains(new Vector2(this.transform.position.x, this.transform.position.y)))
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
        var clickedPiece = GridManager.Instance.GetPiece(new Vector2(this.transform.position.x, this.transform.position.y));
        var coord = new Vector2(this.transform.position.x, this.transform.position.y);
		var turn = GameManager.Instance.GameState == GameState.White ? true : false;
        if (clickedPiece != null) // selected piece is correct turn's color
        {
            if (GridManager.Instance.storedPiece == null && turn == clickedPiece.isWhite && clickedPiece.hasMoved == false)
            {
                //Selects Piece
                Debug.Log(GameManager.Instance.NumMoves);
                //Debug.Log("SELECTED PIECE: " + clickedPiece.gameObject.name);
                GridManager.Instance.storedPiece = clickedPiece;
                GridManager.Instance.storedCoord = coord;
 
                GridManager.Instance.storedPiece.highlightedMoves = clickedPiece.LegalMoves(6, 6);
                foreach (Vector2 tileCoords in GridManager.Instance.storedPiece.highlightedMoves)
                {
                    GridManager.Instance.tiles[tileCoords]._highlight.SetActive(true);
                    //fix hover unhighlight while selected
                }
                GameManager.Instance.MovedPieces.Add(clickedPiece);
            }
            else
            {
                if (GridManager.Instance.storedPiece != null)
                {
					// Possible to capture clickedPiece
					if (GridManager.Instance.storedPiece != clickedPiece && GridManager.Instance.storedPiece.isWhite != clickedPiece.isWhite) 
					{
                    	if (GridManager.Instance.MovePiece(coord, GridManager.Instance.storedPiece))
                    	{
							// Capturing Piece
                        	Destroy(clickedPiece.gameObject);
                        	GameManager.Instance.NumMoves += 1;
                        	GridManager.Instance.storedPiece.hasMoved = true;
                    	}
					}
					foreach (Vector2 tileCoords in GridManager.Instance.storedPiece.highlightedMoves)
					{
						GridManager.Instance.tiles[tileCoords]._highlight.SetActive(false);
                    }
                    GridManager.Instance.storedPiece = null;
                    GridManager.Instance.storedCoord = new Vector2(-1, -1);
				}
			}
        }
        else
        {
            if (GridManager.Instance.storedPiece != null)
            {
                //Move Piece
                if (GridManager.Instance.MovePiece(coord, GridManager.Instance.storedPiece))
                {
                    GridManager.Instance.storedPiece.hasMoved = true;
                    foreach (Vector2 tileCoords in GridManager.Instance.storedPiece.highlightedMoves)
                    {
                        GridManager.Instance.tiles[tileCoords]._highlight.SetActive(false);
                        //fix hover unhighlight while selected
                    }
                    GridManager.Instance.storedPiece = null;
                    GridManager.Instance.storedCoord = new Vector2(-1, -1);
                    GameManager.Instance.NumMoves += 1;
                    //unhighlight after move.
                }


            }
        }

		// turn logic
		if (GameManager.Instance.NumMoves == 2) 
		{
			if (turn == true) 
			{
				GameManager.Instance.ChangeState(GameState.Black);
			} else
			{
				GameManager.Instance.ChangeState(GameState.White);
			}
			foreach (var piece in GameManager.Instance.MovedPieces) 
			{
				piece.hasMoved = false;
			}
			GameManager.Instance.MovedPieces = new List<Piece>();
			GameManager.Instance.NumMoves = 0;
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
