using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Tile
 * Manage tile
 * Src: https://www.youtube.com/watch?v=kkAjpQAM-jE
 */
public class JoshNewTile : MonoBehaviour
{
    [SerializeField] private GameObject highlight;
    [SerializeField] private MeshRenderer elementOne, elementTwo;
    [SerializeField] private Material water, fire, earth, air;     // Water, Fire, Earth, Air
    private Material selectedElement;
    private string elementName;

    Dictionary<string, Color32> element = new Dictionary<string, Color32>()
    {
        { "water", new Color32(0, 0, 255, 120) },
        { "fire", new Color32(255, 0, 0, 120) },
        { "earth", new Color32(0, 255, 0, 120) },
        { "air", new Color32(255, 255, 0, 120) }
    };

    // Select element using button
    public void SetElement(string ele)
    {
        elementName = ele;
        switch(ele.ToLower())
        {
            case "water":
                selectedElement = water;
                break;
            case "fire":
                selectedElement = fire;
                break;
            case "earth":
                selectedElement = earth;
                break;
            case "air":
                selectedElement = air;
                break;
        }
        Debug.Log($"Selected {elementName}         ||      selectedElement {selectedElement.ToString()}");

    }

    // Aspect the tile to an element
    public void AspectOne(Material mat)
    {
        elementOne.material = mat;
    }
    public void AspectTwo(Material mat)
    {
        elementTwo.material = mat;
    }
    public void AspectBoth(Material mat)
    {
        AspectOne(mat);
        AspectTwo(mat);
        Debug.Log($"Painted {mat.ToString()}        ||          selectedElement {selectedElement.ToString()}");
    }

    // Highlight if mouse is hovering a tile
    public void OnMouseEnter()
    {
        highlight.SetActive(true);
    }
    public void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    // Change tile element if mouse clicked
    public void OnMouseDown()
    {
        AspectBoth(selectedElement);
    }

    // Start is called before the first frame update
    void Start()
    {
        highlight.SetActive(false);
        selectedElement = water;
        elementName = "water";
    }

    // Update is called once per frame
    void Update()
    {
        highlight.GetComponent<SpriteRenderer>().color = element[elementName];
    }
}
