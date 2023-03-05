using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to CanMove and CantMove game objects in each unit prefab
public class HighlightTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tileHighlight;

    // Update is called once per frame
    void Update()
    {
        // Not as pronounced as the unit highlighting
        tileHighlight.color = new Color(tileHighlight.color.r, tileHighlight.color.g, tileHighlight.color.b, GameManagerChain.Instance.movableAlpha / 1.5f);
    }
}
