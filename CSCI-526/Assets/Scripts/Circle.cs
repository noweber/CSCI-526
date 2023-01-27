using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : Piece
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Circle's grid manager: " + GridManager.Instance.GetPiece(new Vector2(this.transform.position.x, this.transform.position.y)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
