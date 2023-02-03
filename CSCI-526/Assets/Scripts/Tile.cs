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
        Debug.Log("Grid manager: " + GridManager.Instance.GetPiece(new Vector2(this.transform.position.x, this.transform.position.y)));
        var check = GridManager.Instance.GetPiece(new Vector2(this.transform.position.x, this.transform.position.y));
        var coord = new Vector2(this.transform.position.x, this.transform.position.y);
        Debug.Log("Tile: " + check);
        Debug.Log("Tile coord: " + coord);
        if (check != null)
        {
            if (GridManager.Instance.storedPiece == null)
            {
                //Selects Piece
                Debug.Log("Select Piece");
                GridManager.Instance.storedPiece = check;
                GridManager.Instance.storedCoord = coord;
            }
            else
            {
                Debug.Log("Piece Already Selected and new position has a different piece");
                //If piece has already been selected and selected again, cancel action.
                if (GridManager.Instance.storedPiece == check)
                {
                    GridManager.Instance.storedPiece = null;
                    GridManager.Instance.storedCoord = new Vector2(-1, -1);
                } else 
				{
					if (check != null) 
					{
						Destroy(check.gameObject);
						GridManager.Instance.MovePiece(coord, GridManager.Instance.storedPiece);
						GridManager.Instance.storedPiece = null;
						GridManager.Instance.storedCoord = new Vector2(-1, -1);
					}
				}
            }
        }
        else
        {
            if (GridManager.Instance.storedPiece != null)
            {
                Debug.Log("Piece should move to this empty position");
                Debug.Log("Selected Piece: " + GridManager.Instance.storedPiece);
                //Move Piece
                GridManager.Instance.MovePiece(coord, GridManager.Instance.storedPiece);
                //sets storedPiece as null here
                GridManager.Instance.storedPiece = null;
				GridManager.Instance.storedCoord = new Vector2(-1, -1);
                Debug.Log("Selected Piece change: " + GridManager.Instance.storedPiece);
            }
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
