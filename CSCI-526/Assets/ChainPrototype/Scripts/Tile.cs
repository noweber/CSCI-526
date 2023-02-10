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

    [SerializeField] public GameObject _legal;

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
        _highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        var coord = new Tuple<int, int>((int)this.transform.position.x, (int)this.transform.position.y);
        var lvlMono = LevelMono.Instance;
        var clickedPiece = lvlMono.GetPiece(coord);
        var turn = GameManagerChain.Instance.GameStateEnum == GameStateEnum.Human ? true : false;

        if (clickedPiece != null) // selected piece is correct turn's color
        {
            if (!lvlMono.HasSelectedPiece() && turn == clickedPiece.IsHuman() && clickedPiece.CanMove())
            {
                //Selects Piece
                Debug.Log(GameManagerChain.Instance.NumMoves);
                Debug.Log("TOTAL MOVES: " + GameManagerChain.Instance.TotalMoves);
                lvlMono.SelectPiece(clickedPiece, coord);
                    
                if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1 && clickedPiece.IsCircle())
                {
                    lvlMono.highlightedMoves.Add(new Tuple<int, int>(1, 1));
                    lvlMono.highlightedMoves.Add(new Tuple<int, int>(2, 2));
                }

                lvlMono.HighlightMoves();    
                
                MenuManager.Instance.ShowUnitInfo(clickedPiece);
                GameManagerChain.Instance.MovedPieces.Add(clickedPiece);
            }
            else
            {
                if (lvlMono.HasSelectedPiece())
                {
                    // Possible to capture clickedPiece
                    if (lvlMono.selectedPiece.IsEnemyOf(clickedPiece))
                    {
                        if (lvlMono.MovePiece(coord))
                        {
                            
                            GameManagerChain.Instance.NumMoves += 1;
                            GameManagerChain.Instance.TotalMoves += 1;
                            if (this.inTriangleRange(coord))
                            {
                                GameManagerChain.Instance.NumMoves -= 1;
                                lvlMono.GetPiece(coord).SetMoveState(false);
                            }
                            MenuManager.Instance.ShowNumMovesInfo();
                            
                            if (SceneManager.GetActiveScene().name == "TutorialLevel")
                            {
                                MenuManager.Instance.UpdateObjectiveContent();
                            }
                            //GridManager.Instance.storedPiece.hasMoved = true;
                        }
                    }
                    else
                    {
                        Debug.Log("FAILED TO CAPTURE");
                        // lvlMono.ResetPiece();
                    }

                    lvlMono.RemoveHighlight();
                    lvlMono.ResetPiece();
                    
                    // UI/Analytics
                    MenuManager.Instance.HideAbilityButton();
                    MenuManager.Instance.HideUnitInfo(lvlMono.selectedPiece);
                }
            }
        }
        else
        {
            if (lvlMono.HasSelectedPiece())
            {
                //Move Piece
                if (lvlMono.MovePiece(coord))
                {
                    // UI/Analytics
                    MenuManager.Instance.HideAbilityButton();
                    MenuManager.Instance.HideUnitInfo(lvlMono.selectedPiece);
                    GameManagerChain.Instance.NumMoves += 1;
                    GameManagerChain.Instance.TotalMoves += 1;
                    if (this.inTriangleRange(coord))
                    {
                        GameManagerChain.Instance.NumMoves -= 1;
                        lvlMono.GetPiece(coord).SetMoveState(false);
                    }
                    MenuManager.Instance.ShowNumMovesInfo();

                    
                    if (SceneManager.GetActiveScene().name == "TutorialLevel")
                    {
                        MenuManager.Instance.UpdateObjectiveContent();
                    }
                }
                else
                {
                    
                    Debug.Log("FAILED TO MOVE");
                }
                lvlMono.RemoveHighlight();
                lvlMono.ResetPiece();
            }
        }

        // turn logic
        if (GameManagerChain.Instance.NumMoves == 2)
        {
            foreach (var piece in GameManagerChain.Instance.MovedPieces)
            {
                piece.SetMoveState(false);
            }
            GameManagerChain.Instance.UsedAbility = false;
            GameManagerChain.Instance.MovedPieces = new List<PieceMono>();
            GameManagerChain.Instance.NumMoves = 0;
            
            if (turn == true)
            {
                if (SceneManager.GetActiveScene().name == "TutorialLevel")
                {
                    GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
                    StartCoroutine(DelayedChangeState());
                }
                else
                {
                    GameManagerChain.Instance.ChangeState(GameStateEnum.AI);   
                }
            }
        }
        /*
        foreach (var piece in GameManagerChain.Instance.MovedPieces)
        {
            piece.SetMoveState(false);
        }
        GameManagerChain.Instance.UsedAbility = false;
        GameManagerChain.Instance.MovedPieces = new List<PieceMono>();
        */
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

    private bool inTriangleRange(Tuple<int, int> unitPosition)
    {
        var adjacentList = new List<Tuple<int, int>>();
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2)); //right
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2)); //left
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 + 1)); //up
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 - 1)); //down
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2 + 1)); //right up diag
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2 + 1)); //left  up diag
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2 - 1)); //right down diag
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2 - 1)); //left down diag
        var lvlMono = LevelMono.Instance;
        foreach (Tuple<int, int> coord in adjacentList)
        {
            var piece = lvlMono.GetPiece(coord);
            if (piece != null && piece.IsTriangle() && !lvlMono.GetPiece(unitPosition).IsEnemyOf(piece))
            {
                return true;
            }
        }
        return false;
    }
}





