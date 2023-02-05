using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Piece : MonoBehaviour
{
	public bool isWhite;

	public bool hasMoved;

    public string unitName;
    public string unitInfo;
    //public bool hasAbility = false;

    public List<Tuple<int, int>> highlightedMoves;

    public abstract List<Tuple<int, int>> adjacentAllies(Tuple<int, int> unitPosition);

    public abstract List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight);

    public abstract void UpdateLocation(Vector3 location);

}
