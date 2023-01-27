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
        _highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        var clickedPiece = GridManager.Instance.GetPiece(new Vector2(this.transform.position.x, this.transform.position.y));
        var coord = new Vector2(this.transform.position.x, this.transform.position.y);
		var turn = GameManager.Instance.GameState == GameState.White ? true : false;
		Debug.Log(GameManager.Instance.GameState);
        if (clickedPiece != null && turn == clickedPiece.isWhite) // selected piece is correct turn's color
        {
            if (GridManager.Instance.storedPiece == null)
            {
                //Selects Piece
                Debug.Log("SELECTED PIECE: " + clickedPiece.gameObject.name);
                GridManager.Instance.storedPiece = clickedPiece;
                GridManager.Instance.storedCoord = coord;
            }
            else
            {
                Debug.Log("SELECTED PIECE AGAIN: " + clickedPiece.gameObject.name);
                //If piece has already been selected and selected again, cancel action.
                if (GridManager.Instance.storedPiece != clickedPiece && GridManager.Instance.storedPiece.isWhite != clickedPiece.isWhite)
                {
					Debug.Log("CAPTURED: " + clickedPiece.gameObject.name);
					// Capturing enemy piece
					Destroy(clickedPiece.gameObject);
					GridManager.Instance.MovePiece(coord, GridManager.Instance.storedPiece);
					GameManager.Instance.NumMoves += 1;
				}
					GridManager.Instance.storedPiece = null;
					GridManager.Instance.storedCoord = new Vector2(-1, -1);
            }
        }
        else
        {
            if (GridManager.Instance.storedPiece != null)
            {
                //Move Piece
                GridManager.Instance.MovePiece(coord, GridManager.Instance.storedPiece);
                //sets storedPiece as null here
                GridManager.Instance.storedPiece = null;
				GridManager.Instance.storedCoord = new Vector2(-1, -1);
				GameManager.Instance.NumMoves += 1;
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
