using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArrowTranslator;

public class Overlay : MonoBehaviour
{
    public int G;
    public int H;
    public int F { get { return G + H; } }

    public bool isBlocked = false;

    public Overlay Previous;
    public Vector2Int grid2DLocation {get { return new Vector2Int(gridLocation.x, gridLocation.y); } }
    public Vector3Int gridLocation;
    public bool overlayOn = false;

    public List<Sprite> arrows;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    public void SetSprite(ArrowDirection d)
    {
        if (d == ArrowDirection.None)
            GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
        else
        {
            GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 1);
            GetComponentsInChildren<SpriteRenderer>()[1].sprite = arrows[(int)d];
            GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        }
    }
}
