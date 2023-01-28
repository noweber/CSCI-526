using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : Piece
{

    public override bool IsMoveLegal(Vector2 destination)
    {
        /* Diamond movoes like a knight:
            Two moves horizontal and one move vertical, OR
            Two moves vertical, one move horizontal */
        var pos = transform.position;
        if (Mathf.Abs(destination.x - pos.x) == 2 && Mathf.Abs(destination.y - pos.y) == 1)
        { 
            return true; 
        }
        else if (Mathf.Abs(destination.x - pos.x) == 1 && Mathf.Abs(destination.y - pos.y) == 2)
        {
            return true;
        }

        return false;
    }

    public override List<Vector2> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Vector2> legalSpots = new List<Vector2>();

        var pos = transform.position;

        for (int i = 0; i < boardHeight; i++)
        {
            for (int j = 0; j < boardWidth; j++)
            {
                if (Mathf.Abs(i - pos.x) == 2 && Mathf.Abs(j - pos.y) == 1)
                {
                    legalSpots.Add(new Vector2(i, j));
                }
                else if (Mathf.Abs(i - pos.x) == 1 && Mathf.Abs(j - pos.y) == 2)
                {
                    legalSpots.Add(new Vector2(i, j));

                }
            }
        }

        if (legalSpots.Count == 0)
            return null;

        return legalSpots;
    }

    public override List<Vector2> adjacentAllies(Vector2 unitPosition)
    {
        var adjacentList = new List<Vector2>();
        adjacentList.Add(new Vector2(unitPosition.x + 1, unitPosition.y));
        adjacentList.Add(new Vector2(unitPosition.x - 1, unitPosition.y));
        adjacentList.Add(new Vector2(unitPosition.x, unitPosition.y + 1));
        adjacentList.Add(new Vector2(unitPosition.x, unitPosition.y - 1));

        var adjAlly = new List<Vector2>();
        foreach (Vector2 coord in adjacentList)
        {
            if (GridManager.Instance._pieces.ContainsKey(coord))
            {
                if (GridManager.Instance._pieces[coord] != null && this.isWhite == GridManager.Instance._pieces[coord].isWhite)
                {
                    adjAlly.Add(coord);
                }
            }
        }
        if (adjAlly.Count == 0)
            return null;
        return adjAlly;
    }

    public override void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
