using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _base, _offset;

    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] public GameObject _highlight;

    //[SerializeField] private GameObject _useAbility;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _base : _offset;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        if (LevelMono.Instance.storedPiece != null)
        {
            // TODO: reconfigure this with IPiece
            var highlightTiles = LevelMono.Instance.storedPiece.highlightedMoves;
            //Vector3 mousePos = Input.mousePosition;
            if (highlightTiles.Contains(new Tuple<int, int>((int)this.transform.position.x, (int)this.transform.position.y)))
            {
                //Debug.Log("IN THE ARRAY");
                _highlight.SetActive(true);
            }
            else
            {
                //Debug.Log("NOT IN THE ARRAY");
                _highlight.SetActive(false);
            }
            _highlight.SetActive(false);
        }
        else
        {
            _highlight.SetActive(false);
        }
        //_highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        var coord = new Tuple<int, int>((int)this.transform.position.x, (int)this.transform.position.y);
        var clickedPiece = LevelMono.Instance.GetPiece(coord);
        var turn = GameManagerChain.Instance.GameStateEnum == GameStateEnum.Human ? true : false;

        // Endgame logic - TODO: We can probably apply a reverse logic to clean the redundant inner IF-ELSE-IF loop

        /*
        if (LevelController.Instance.LevelModel.TryGetUnit(new Tuple<int, int>(3, 4)) == null)
        {
            if (LevelController.Instance.LevelModel.TryGetUnit(new Tuple<int, int>(3, 5)) == null) { Debug.Log("TUTORIAL_END)"); Analytics.Instance.Send(GameManagerChain.Instance.playTestID, GameManagerChain.Instance.TotalMoves); }
            else if (LevelController.Instance.LevelModel.TryGetUnit(new Tuple<int, int>(3, 5)).HasMoved() == true) { Debug.Log("TUTORIAL_END)"); Analytics.Instance.Send(GameManagerChain.Instance.playTestID, GameManagerChain.Instance.TotalMoves); }

        }
        else if (LevelController.Instance.LevelModel.TryGetUnit(new Tuple<int, int>(3, 4)).HasMoved() == true)

        {
            if (LevelController.Instance.LevelModel.TryGetUnit(new Tuple<int, int>(3, 5)) == null) { Debug.Log("TUTORIAL_END)"); Analytics.Instance.Send(GameManagerChain.Instance.playTestID, GameManagerChain.Instance.TotalMoves); }
            else if (LevelController.Instance.LevelModel.TryGetUnit(new Tuple<int, int>(3, 5)).HasMoved() == true) { Debug.Log("TUTORIAL_END)"); Analytics.Instance.Send(GameManagerChain.Instance.playTestID, GameManagerChain.Instance.TotalMoves); }
        }*/

        if (clickedPiece != null) // selected piece is correct turn's color
        {
            if (LevelMono.Instance.storedPiece == null && turn == clickedPiece.IsControlledByHuman() && clickedPiece.HasMoved() == false)
            {
                //Selects Piece
                Debug.Log(GameManagerChain.Instance.NumMoves);
                Debug.Log("TOTAL MOVES: " + GameManagerChain.Instance.TotalMoves);
                LevelMono.Instance.storedPiece = clickedPiece;
                LevelMono.Instance.storedCoord = coord;

                LevelMono.Instance.storedPiece.highlightedMoves = clickedPiece.GetLegalMoves(LevelMono.Instance.LevelModel.GetWidth(), LevelMono.Instance.LevelModel.GetHeight());

                if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1 && clickedPiece.Name() == "Circle")
                {
                    LevelMono.Instance.storedPiece.highlightedMoves.Add(new Tuple<int, int>(1, 1));
                    LevelMono.Instance.storedPiece.highlightedMoves.Add(new Tuple<int, int>(2, 2));
                }

                foreach (Tuple<int, int> tileCoords in LevelMono.Instance.storedPiece.highlightedMoves)
                {
                    LevelMono.Instance.tiles[tileCoords]._highlight.SetActive(true);
                    if (tileCoords.Item1 == 1 && tileCoords.Item2 == 1 && GameManagerChain.Instance.TotalMoves == 1) { LevelMono.Instance.tiles[tileCoords]._highlight.GetComponent<SpriteRenderer>().color = new Color32(200, 100, 70, 255); }
                    if (tileCoords.Item1 == 2 && tileCoords.Item2 == 2 && GameManagerChain.Instance.TotalMoves == 1) { LevelMono.Instance.tiles[tileCoords]._highlight.GetComponent<SpriteRenderer>().color = new Color32(200, 100, 70, 255); }
                }
                MenuManager.Instance.ShowUnitInfo(clickedPiece);
                GameManagerChain.Instance.MovedPieces.Add(clickedPiece);
            }
            else
            {
                if (LevelMono.Instance.storedPiece != null)
                {
                    // Possible to capture clickedPiece
                    if (LevelMono.Instance.storedPiece != clickedPiece && LevelMono.Instance.storedPiece.IsControlledByHuman() != clickedPiece.IsControlledByHuman())
                    {
                        if (LevelMono.Instance.MovePiece(coord, LevelMono.Instance.storedPiece))
                        {
                            //TODO: add LEVELMODEL update for capture
                            // Capturing Piece 
                            // Destroy(clickedPiece.gameObject);
                            GameManagerChain.Instance.NumMoves += 1;
                            GameManagerChain.Instance.TotalMoves += 1;


                            if (SceneManager.GetActiveScene().name == "TutorialLevel")
                            {
                                MenuManager.Instance.UpdateObjectiveContent();
                            }

                            MenuManager.Instance.ShowNumMovesInfo();
                            //If Unit that Captured a piece is Circle, gain another turn
                            if (LevelMono.Instance.storedPiece.Name() != "Circle")
                            {
                                Debug.Log("Piece that captured is NOT a circle");
                                LevelMono.Instance.storedPiece.SetMoveState(true);
                            }

                            //GridManager.Instance.storedPiece.hasMoved = true;
                        }
                    }
                    foreach (Tuple<int, int> tileCoords in LevelMono.Instance.storedPiece.highlightedMoves)
                    {
                        LevelMono.Instance.tiles[tileCoords]._highlight.SetActive(false);
                        LevelMono.Instance.tiles[tileCoords]._highlight.GetComponent<SpriteRenderer>().color =
                            new Color32(255, 255, 255, 100);
                    }
                    MenuManager.Instance.HideAbilityButton();
                    MenuManager.Instance.HideUnitInfo(LevelMono.Instance.storedPiece);
                    LevelMono.Instance.storedPiece = null;
                    LevelMono.Instance.storedCoord = new Tuple<int, int>(-1, -1);
                }
            }
        }
        else
        {
            Debug.Log(LevelMono.Instance.storedPiece);
            Debug.Log(clickedPiece);
            if (LevelMono.Instance.storedPiece != null)
            {
                //Move Piece
                if (LevelMono.Instance.MovePiece(coord, LevelMono.Instance.storedPiece))
                {
                    LevelMono.Instance.storedPiece.SetMoveState(true);
                    foreach (Tuple<int, int> tileCoords in LevelMono.Instance.storedPiece.highlightedMoves)
                    {
                        LevelMono.Instance.tiles[tileCoords]._highlight.SetActive(false);
                        LevelMono.Instance.tiles[tileCoords]._highlight.GetComponent<SpriteRenderer>().color =
                            new Color32(255, 255, 255, 100);
                        //fix hover unhighlight while selected
                    }
                    MenuManager.Instance.HideAbilityButton();
                    MenuManager.Instance.HideUnitInfo(LevelMono.Instance.storedPiece);
                    LevelMono.Instance.storedPiece = null;
                    LevelMono.Instance.storedCoord = new Tuple<int, int>(-1, -1);
                    GameManagerChain.Instance.NumMoves += 1;
                    GameManagerChain.Instance.TotalMoves += 1;
                    MenuManager.Instance.ShowNumMovesInfo();


                    if (SceneManager.GetActiveScene().name == "TutorialLevel")
                    {
                        MenuManager.Instance.UpdateObjectiveContent();
                    }
                    //unhighlight after move.
                }
            }
        }

        // turn logic
        if (GameManagerChain.Instance.NumMoves == 2)
        {
            GameManagerChain.Instance.NumMoves = 0;
            if (turn == true)
            {
                GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
            }
            else
                if (SceneManager.GetActiveScene().name == "TutorialLevel")
            {
                // Delayed switch back from black for tutorial
                GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
                StartCoroutine(DelayedChangeState());
            }
            else
            {
                GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
            }
        }
        else
        {
            GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
        }
        foreach (var piece in GameManagerChain.Instance.MovedPieces)
        {
            piece.SetMoveState(false);
        }
        GameManagerChain.Instance.UsedAbility = false;
        GameManagerChain.Instance.MovedPieces = new List<PieceMono>();
    }

    // "Slacking off" text for 3 seconds, then change state to white
    private IEnumerator DelayedChangeState()
    {
        MenuManager.Instance.SetSlackDialogue(true);
        yield return new WaitForSeconds(2);
        MenuManager.Instance.SetSlackDialogue(false);
        GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
        yield return null;
    }
}





