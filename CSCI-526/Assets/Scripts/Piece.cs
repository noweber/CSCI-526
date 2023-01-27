using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public abstract bool IsMoveLegal(Vector2 destination);

    public abstract void UpdateLocation(Vector3 location);

}
