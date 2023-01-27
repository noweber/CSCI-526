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
        if (GridManager.Instance.GetPiece(new Vector2(this.transform.position.x, this.transform.position.y)) != null)
        {
            if (GridManager.Instance.storedPiece == null)
            {
                //Selects Piece
                Debug.Log("Select Piece");
                GridManager.Instance.storedPiece = GridManager.Instance.GetPiece(new Vector2(this.transform.position.x, this.transform.position.y));
            }
            else
            {
                Debug.Log("Piece Already Selected and new position has a different piece");
            }
        }
        else
        {
            if (GridManager.Instance.storedPiece != null)
            {
                Debug.Log("Piece should move to this empty position");
                Debug.Log("Selected Piece: " + GridManager.Instance.storedPiece);
                //Move Piece
                //sets storedPiece as null here
                GridManager.Instance.storedPiece = null;
                Debug.Log("Selected Piece change: " + GridManager.Instance.storedPiece);
            }
        }
        //Finds valid piece
        //Calls GridManager/sends to GridManager --> initialize some function to prepare for movement/store the piece
        //Piece in GridManager--> var selectedPiece = piece or null --> set as True if itself, then set null/cancel action

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
