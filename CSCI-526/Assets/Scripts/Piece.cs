using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
	public bool isWhite;

	public bool hasMoved;

    public abstract bool IsMoveLegal(Vector2 destination);

    public abstract List<Vector2> LegalMoves(int boardWidth, int boardHeight);

    public abstract void UpdateLocation(Vector3 location);

}
