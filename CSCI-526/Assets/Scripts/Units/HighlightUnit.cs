using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to CanMove and CantMove game objects in each unit prefab
public class HighlightUnit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer highlight;

    // Update is called once per frame
    void Update()
    {
        highlight.color = new Color(highlight.color.r, highlight.color.g, highlight.color.b, GameManagerChain.Instance.movableAlpha);
    }
}
