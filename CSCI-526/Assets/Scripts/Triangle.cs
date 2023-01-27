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

    public override void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
