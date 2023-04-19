using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Attached to CanMove game object in each unit prefab and EndTurn button in MenuManager
public class HighlightUnit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer highlight;      // For CanMove
    [SerializeField] private Image btn;     // For End Turn

    [SerializeField] private float c = 1;       // For End Turn r, g, b
    private bool increasingC = false;
    public bool moused = false;
    private IEnumerator FadeEndTurn()
    {
        while (true)
        {
            if (c >= 0.9f)
            {
                increasingC = false;
            }
            else if (c <= 0.7f)
            {
                increasingC = true;
            }
            if (increasingC)
            {
                c += 0.07f;
            }
            else
            {
                c -= 0.07f;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    private void OnMouseEnter()
    {
        moused = true;
    }
    private void OnMouseExit()
    {
        moused = false;
    }
    private void Awake()
    {
        if(btn != null)
        {
            StartCoroutine(FadeEndTurn());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(highlight != null)
        {
            highlight.color = new Color(highlight.color.r, highlight.color.g, highlight.color.b, GameManagerChain.Instance.movableAlpha);
        }
        if(btn != null && !moused)
        {
            btn.color = new Color(c, c, c, 1);
        }
    }
}
