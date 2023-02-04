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

    public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

        var pos = new Vector2Int ((int)transform.position.x, (int)transform.position.y);
        var lvlModel = GridManager.Instance.levelModel;

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                //get diagonals - those are ok
                if (Mathf.Abs(i-pos.x) == 1 && Mathf.Abs(j-pos.y) == 1)
                {
                    var availableMove = new Tuple<int, int>(i, j);
                    if (lvlModel.TryGetUnit(i, j) != null)
                    {
                        if (lvlModel.TryGetUnit(i, j)[0] == this.isWhite) {continue;}
                    }
                    // if (GridManager.Instance.GetPiece(availableMove) != null)
                    // {
                    //     if (GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) { continue; }
                    // }
                    legalSpots.Add(availableMove);
                }

                //get +1 to cardinal directions
                if (Mathf.Abs(i - pos.x) == 2 && Mathf.Abs(j - pos.y) == 0)
                {
                    var availableMove = new Tuple<int, int>(i, j);
                    if (lvlModel.TryGetUnit(i, j) != null)
                    {
                        if (lvlModel.TryGetUnit(i, j)[0] == this.isWhite) {continue;}
                    }
                    legalSpots.Add(availableMove);
                }
                if (Mathf.Abs(i - pos.x) == 0 && Mathf.Abs(j - pos.y) == 2)
                {
                    var availableMove = new Tuple<int, int>(i, j);
                    if (lvlModel.TryGetUnit(i, j) != null)
                    {
                        if (lvlModel.TryGetUnit(i, j)[0] == this.isWhite) {continue;}
                    }
                    legalSpots.Add(availableMove);
                }
            }
        }

        return legalSpots;
    }

    public override List<Tuple<int, int>> adjacentAllies(Tuple<int, int> unitPosition)
    {
        var adjacentList = new List<Tuple<int, int>>();
        adjacentList.Add(new Tuple<int, int>(unitPosition.x + 1, unitPosition.y));
        adjacentList.Add(new Tuple<int, int>(unitPosition.x - 1, unitPosition.y));
        adjacentList.Add(new Tuple<int, int>(unitPosition.x, unitPosition.y + 1));
        adjacentList.Add(new Tuple<int, int>(unitPosition.x, unitPosition.y - 1));

        var adjAlly = new List<Tuple<int, int>>();
        var lvlModel = GridManager.Instance.levelModel;
        foreach (Tuple<int, int> coord in adjacentList)
        {
            if (lvlModel.TryGetUnit(coord[0], coord[1])[0] == this.isWhite)
            {
                adjAlly.Add(coord);
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
