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

        /*Diamond moves like a knight*/
        var pos = transform.position;

        for (int i = 0; i < boardWidth; i++)
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

    public override void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
