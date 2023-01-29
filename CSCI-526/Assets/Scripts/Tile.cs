using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Tile
 * Manage tile
 * Src: https://www.youtube.com/watch?v=kkAjpQAM-jE
 */
public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ren;
    [SerializeField] private GameObject highlight;

    public Vector2Int pos;     // Track own index location
    public bool mouseHere;
    public bool mouseNear;
    Dictionary<string, Color32> element = new Dictionary<string, Color32>()
    {
        { "water", new Color32(0, 0, 255, 255) },
        { "fire", new Color32(255, 0, 0, 255) },
        { "earth", new Color32(255, 255, 0, 255) },
        { "air", new Color32(161, 255, 230, 255) }
    };

    // Aspect the tile to an element
    public void aspect(string ele)
    {
        ren.color = element[ele];
    }

    public void HighlightOn()
    {
        highlight.SetActive(true);
    }
    public void HighlightOff()
    {
        highlight.SetActive(false);
    }

    // Tell the GridManager if a mouse is hovering a tile
    public void OnMouseEnter()
    {
        mouseHere = true;
        GridManager.Instance.MousedOver();
    }
    public void OnMouseExit()
    {
        mouseHere = true;
        GridManager.Instance.MouseExit();
    }

    // Change tile element if mouse clicked
    public void OnMouseDown()
    {
        GridManager.Instance.ChangeElement();
    }
    // Start is called before the first frame update
    void Start()
    {
        mouseHere = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch(GridManager.Instance.selectedElement)
        {
            case "water":
                highlight.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 255, 120);
                break;
            case "earth":
                highlight.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 0, 120);
                break;
            case "fire":
                highlight.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 120);
                break;
            case "air":
                highlight.GetComponent<SpriteRenderer>().color = new Color32(161, 255, 230, 120);
                break;
            default:
                break;
        }
        if(mouseNear)
        {
            HighlightOn();
        }
        else
        {
            HighlightOff();
        }
    }
}
