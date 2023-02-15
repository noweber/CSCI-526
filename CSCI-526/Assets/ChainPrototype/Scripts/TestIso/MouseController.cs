using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    private PieceMono unit;
    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            Overlay overlayTile = hit.Value.collider.gameObject.GetComponent<Overlay>(); ;
            cursor.transform.position = overlayTile.transform.position;
            cursor.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            if (Input.GetMouseButtonDown(0))
            {
                overlayTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                if (unit != null && unit.standingOnTile.gridLocation == overlayTile.gridLocation)
                {
                    unit = null;
                    //isMoving = false;
                    overlayTile.HideTile();
                    foreach (var highlight in LevelMono.Instance.highlightedMoves)
                    {
                        var hlight = new Tuple<int, int>(highlight.Item2, highlight.Item1);
                        LevelMono.Instance.overlayTiles[hlight].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                    }
                }
                else if (LevelMono.Instance.playersLocations.Contains(overlayTile.gridLocation) && unit == null)
                {
                    foreach (var player in LevelMono.Instance.players)
                    {
                        //Debug.Log(player.gameObject.GetComponent<CharacterInfo>().standingOnTile.gridLocation);
                        if (player.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation == overlayTile.gridLocation)
                        {
                            unit = player;
                            break;
                        }
                    }
                    Debug.Log("Selected " + unit.GetName());
                    if (unit.GetName() != "Triangle")
                    {
                        var legal = unit.LegalMoves(LevelMono.Instance.GetHeight(), LevelMono.Instance.GetWidth());
                        var coord = new Tuple<int, int>(unit.standingOnTile.gridLocation.x, unit.standingOnTile.gridLocation.y);
                        LevelMono.Instance.SelectPiece(unit, coord);
                        Debug.Log("Selected Coord: " + LevelMono.Instance.selectedCoord);
                        foreach (var highlight in LevelMono.Instance.highlightedMoves)
                        {
                            Debug.Log("Should Highlight: " + highlight);
                            var hlight = new Tuple<int, int>(highlight.Item1, highlight.Item2);
                            LevelMono.Instance.overlayTiles[hlight].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                        }

                    }
                }
            }
            //else
            //{
            //    isMoving = true;
            //    overlayTile.HideTile();
            //}
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }
}
