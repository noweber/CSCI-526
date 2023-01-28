using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : Piece
{

    public override bool IsMoveLegal(Vector2 destination)
    {
        //Circle moves like a king (if x and/or y change by 1)
        var pos = transform.position;
        if (Mathf.Abs(destination.x - pos.x) == 1 && Mathf.Abs(destination.y - pos.y) <= 1)
        {
            return true;
        }
        else if (Mathf.Abs(destination.x - pos.x) <= 1 && Mathf.Abs(destination.y - pos.y) == 1)
        {
            return true;
        }

        return false;
    }

    public override List<Vector2> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Vector2> legalSpots = new List<Vector2>();

        //Circle moves like a king (delta(x) + delta(y) <= 2)
        var pos = transform.position;
        bool changeMovement = this.CircleMovementCheck(pos);

        if (changeMovement == true)
        {
            Debug.Log("Circle should have queen movement");
            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    //Vertical Movement
                    if (pos.x == i)
                    {
                        var coord = new Vector2(i, j);
                        legalSpots.Add(coord);
                    }

                    //Diagonal Movement
                    else if (Mathf.Abs(i - pos.x) == Mathf.Abs(j - pos.y))
                    {
                        var coord = new Vector2(i, j);
                        legalSpots.Add(coord);
                    }
                }
            }
        }
        else
        {
            //add else block
            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    if (Mathf.Abs(i - pos.x) == 1 && Mathf.Abs(j - pos.y) <= 1)
                    {
                        legalSpots.Add(new Vector2(i, j));
                    }
                    else if (Mathf.Abs(i - pos.x) <= 1 && Mathf.Abs(j - pos.y) == 1)
                    {
                        legalSpots.Add(new Vector2(i, j));
                    }
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
                if (GridManager.Instance._pieces[coord] != null && this.isWhite == GridManager.Instance._pieces[coord].isWhite )
                {
                    adjAlly.Add(coord);
                }
            }
        }
        if (adjAlly.Count == 0)
            return null;
        return adjAlly;
    }

    private bool CircleMovementCheck(Vector2 pos)
    {
        var adjList = this.adjacentAllies(pos);
        if (adjList != null)
        {
            foreach (Vector2 coord in adjList)
            {
                if (GridManager.Instance.GetPiece(coord).unitName == "Diamond")
                {
                    Debug.Log("Make Circle move like Queen");
                    return true;
                }
            }
        }
        return false;
    }

    public override void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
