using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : Piece
{

    public override bool IsMoveLegal(Vector2 destination)
    {
        /*triangle moves like a bishop (diagonal)
         *  |srcx - destx| == |srcy- desty|
        */
        var pos = transform.position;
        if (Mathf.Abs(destination.x - pos.x) == Mathf.Abs(destination.y - pos.y))
        {
            return true;
        }
        return false;
    }

    public override List<Vector2> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Vector2> legalSpots = new List<Vector2>();

        /*triangle moves like a bishop (diagonal)
         *  delta(x) = delta(y)
        */
        var pos = transform.position;

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardWidth; j++)
            {
                if (Mathf.Abs(i - pos.x) == Mathf.Abs(j - pos.y))
                {
                    var coord = new Vector2(i, j);

                    if (!GridManager.Instance._pieces.ContainsKey(coord))
                    {
                        legalSpots.Add(coord);
                    }
                    else
                    {
                        if (GridManager.Instance._pieces[coord] == null)
                        {
                            legalSpots.Add(coord);
                        }
                    }
                    //legalSpots.Add(coord);
                    
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
        return adjAlly;
    }

    public override void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
