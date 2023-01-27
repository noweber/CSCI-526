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

    public override void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
