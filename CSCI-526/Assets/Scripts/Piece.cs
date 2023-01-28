using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
	public bool isWhite;

	public bool hasMoved;

    public string unitName;
    public string unitInfo;

    public List<Vector2> highlightedMoves;

    public abstract List<Vector2> adjacentAllies(Vector2 unitPosition);

    public abstract bool IsMoveLegal(Vector2 destination);

    public abstract List<Vector2> LegalMoves(int boardWidth, int boardHeight);

    public abstract void UpdateLocation(Vector3 location);

}
