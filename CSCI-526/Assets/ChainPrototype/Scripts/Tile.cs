using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.XR;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _base, _offset;

    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] public GameObject _highlight;

    [SerializeField] public GameObject _legal;

    [SerializeField] public GameObject _fog, _redFog;

    [SerializeField] private GameObject closeEye, openEye, boot;
    
    private bool canPlayerSee;
    private bool canEnemySee;

    public bool isLegalMove = false;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _base : _offset;
    }

    public void ToggleEye(bool status)
    {
        openEye.SetActive(status);
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);

        // If legal move, turn on boot
        if(isLegalMove)
        {
            boot.SetActive(true);
            
            // IF adjacent ally is triangle, render triangle visible area as eyes
            foreach(Tuple<int,int> ally in AdjacentAllies())
            {
                var lvlmono = LevelMono.Instance;
                if(lvlmono.GetPiece(ally).IsTriangle())
                {
                    foreach(Tuple<int,int> visibleTile in lvlmono.GetPiece(ally).GetVisibleArea())
                    {
                        lvlmono.GetTile(visibleTile).ToggleEye(true);
                    }
                }
            }
        }
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        if(this.boot.activeInHierarchy)
        {
            boot.SetActive(false);
            // IF adjacent ally is triangle, render triangle visible area as eyes
            foreach (Tuple<int, int> ally in AdjacentAllies())
            {
                var lvlmono = LevelMono.Instance;
                if (lvlmono.GetPiece(ally).IsTriangle())
                {
                    foreach (Tuple<int, int> visibleTile in lvlmono.GetPiece(ally).GetVisibleArea())
                    {
                        lvlmono.GetTile(visibleTile).ToggleEye(false);
                    }
                }
            }
        }
    }

    public void ToggleFog()
    {
        if (canPlayerSee)
        {
            _fog.SetActive(false);
        }
        else
        {
            _fog.SetActive(true);
        }
    }
    
    // DEBUG ONLY
    public void ToggleEnemyFog()
    {
        if (canEnemySee)
        {
            _fog.SetActive(false);
        }
        else
        {
            _fog.SetActive(true);
        }
    }

    public void SetPlayerVisibility(bool v)
    {
        this.canPlayerSee = v;
    }
    
    public void SetEnemyVisibility(bool v)
    {
        this.canEnemySee = v;
    }
    
    public bool CanPlayerSee()
    {
        return this.canPlayerSee;
    }

    public bool CanEnemySee()
    {
        return this.canEnemySee;
    }
    
/*    public void ShowVisibility()
    {
        if (SceneManager.GetActiveScene().name == "TutorialLevel") { return; }       // Not needed in first tutorial level -- no fog
        // switch (visibility)
        // {
        //     case VisibilityState.Player:
        //         openEye.SetActive(true);
        //         closeEye.SetActive(false);
        //         break;
        //     case VisibilityState.Enemy:
        //     case VisibilityState.Neutral:
        //         closeEye.SetActive(true);
        //         openEye.SetActive(false);
        //         break;
        // }
        if (this.canPlayerSee)
        {
            openEye.SetActive(true);
            closeEye.SetActive(false);
        }
        else
        {
            openEye.SetActive(false);
            closeEye.SetActive(true);
        }
    }

    public void HideVisibility()
    {
        openEye.SetActive(false);
        closeEye.SetActive(false);
    }
*/

	private List<Tuple<int, int>> AdjacentAllies()
	{
		var lvlMono = LevelMono.Instance;
		var pos = this.transform.position;
		int x = (int)pos.x;
		int y = (int)pos.y;

		if (!lvlMono.HasSelectedPiece()) 
		{
			Debug.Log("NO SELECTED PIECE. CANNOT FIND ADJACENT ALLIES");
			return null;
		}

		var selectedPiece = lvlMono.GetSelectedPiece();		

		var adjacentList = new List<Tuple<int, int>>();
        adjacentList.Add(new Tuple<int, int>(x + 1, y)); //right
        adjacentList.Add(new Tuple<int, int>(x - 1, y)); //left
        adjacentList.Add(new Tuple<int, int>(x, y + 1)); //up
        adjacentList.Add(new Tuple<int, int>(x, y - 1)); //down
        adjacentList.Add(new Tuple<int, int>(x + 1, y + 1)); //right up diag
        adjacentList.Add(new Tuple<int, int>(x - 1, y + 1)); //left  up diag
        adjacentList.Add(new Tuple<int, int>(x + 1, y - 1)); //right down diag
        adjacentList.Add(new Tuple<int, int>(x - 1, y - 1)); //left down diag

        var adjAlly = new List<Tuple<int, int>>();
		
        foreach (Tuple<int, int> coord in adjacentList)
        {
			if (lvlMono.GetPiece(coord) != null)
            {
            	adjAlly.Add(coord);
            }
        }    
        return adjAlly;
	}

    private void OnMouseDown()
    {
        var coord = new Tuple<int, int>((int)this.transform.position.x, (int)this.transform.position.y);
        var lvlMono = LevelMono.Instance;
        var clickedPiece = lvlMono.GetPiece(coord);
        var turn = GameManagerChain.Instance.GameStateEnum == GameStateEnum.Human ? true : false;

        if (clickedPiece != null && !GameManagerChain.Instance.switchingTurns) // selected piece is correct turn's color, prevents moving another unit while game is switching turns
        {
            if (!lvlMono.HasSelectedPiece() && turn == clickedPiece.IsHuman() && clickedPiece.CanMove())
            {
                // SELECTING CLICKED PIECE AS SELECTEDPIECE
                Debug.Log("TOTAL MOVES: " + GameManagerChain.Instance.TotalMoves);
                lvlMono.SelectPiece(clickedPiece, coord);
                lvlMono.HighlightMoves();

                MenuManager.Instance.ShowUnitInfo(clickedPiece);
                GameManagerChain.Instance.AddMovedPiece(clickedPiece, coord);
            }
            else
            {
                if (lvlMono.HasSelectedPiece())
                {
                    if (lvlMono.selectedPiece.IsEnemyOf(clickedPiece))
                    {
                        // LEVELMONO SELECTEDPIECE CAPTURING
                        if (lvlMono.MovePiece(coord))
                        {

                            GameManagerChain.Instance.TotalMoves += 1;
							GameManagerChain.Instance.IncrementMoves(1);
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
                // LEVELMONO SELECTEDPIECE MOVING
                if (lvlMono.MovePiece(coord))
                {
                    // UI/Analytics
                    MenuManager.Instance.HideAbilityButton();
                    MenuManager.Instance.HideUnitInfo(lvlMono.selectedPiece);
					GameManagerChain.Instance.TotalMoves += 1;
                    GameManagerChain.Instance.IncrementMoves(1);
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
        /*
        if (!LevelMono.Instance.DoesAiPlayerHaveUnitsRemaining())
        {
            StopAllCoroutines();
            GameManagerChain.Instance.ChangeState(GameStateEnum.Victory);
        }
		*/
        if (GameManagerChain.Instance.GetMovesMade() == 2)
        {
            if (turn == true)
            {
                if (SceneManager.GetActiveScene().name == "TutorialLevel")
                {
                    // GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
                    StartCoroutine(DelayedChangeState());
                }
                else if (SceneManager.GetActiveScene().name == "TutorialFogOfWar")
                {
                    // GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
                    StartCoroutine(DelayedChangeState());
                }
                else if (SceneManager.GetActiveScene().name == "Challenge_Circle")
                {
                    StartCoroutine(DelayedChangeState());
                }
                else if (SceneManager.GetActiveScene().name == "Challenge_Scout")
                {
                    StartCoroutine(GameManagerChain.Instance.StateToAI());
                }
                else
                {
                    StartCoroutine(GameManagerChain.Instance.StateToAI());
                    // GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
                }
            }
        }
    }

    // "Slacking off" text for 2 seconds, then change state to white
    private IEnumerator DelayedChangeState()
    {
        StartCoroutine(GameManagerChain.Instance.StateToAI());
        yield return new WaitForSeconds(2.0f);
        MenuManager.Instance.SetSlackDialogue(true);
        yield return new WaitForSeconds(1.5f);
        MenuManager.Instance.SetSlackDialogue(false);
        StartCoroutine(GameManagerChain.Instance.StateToHuman());
        yield return null;
    }

}



