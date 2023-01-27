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

    public override void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
