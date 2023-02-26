using Assets.Scripts.Units;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }


    [SerializeField] private GameObject _turnInfoObject, _selectedUnitInfo, _numTurnObject, _abilityUseObject, _endTurnObject, _objectiveObject, _objectiveContent, _slackObject, _pauseObject, _victoryObject, _pointerObject;
   
    [SerializeField] private TextMeshProUGUI unitInfo, unitAbility;     // Text components of Unit game object

    public GameObject _playerTurnIndicator, _enemyTurnIndicator;

    public MenuManager()
    {
        Instance = this;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void ShowTurnInfo()
    {
        if (GameManagerChain.Instance.GameStateEnum == GameStateEnum.Human)
        {
            _turnInfoObject.GetComponentInChildren<Text>().text = "Blue Turn";
        }
        else if (GameManagerChain.Instance.GameStateEnum == GameStateEnum.AI)
        {
            Debug.Log("AI TURN");
            _turnInfoObject.GetComponentInChildren<Text>().text = "Red Turn";
        }
    }

    public void ShowNumMovesInfo()
    {
        _numTurnObject.GetComponentInChildren<Text>().text = "" + (2 - GameManagerChain.Instance.GetMovesMade()) + " Moves Left";
    }
    public void ShowEndTurnButton()
    {
        _endTurnObject.SetActive(true);
    }

    public void HideEndTurnButton()
    {
        _endTurnObject.SetActive(false);
    }

    public void EndTurnEvent()//(Piece triangle, Piece other)
    {
        Debug.Log("End Turn Event");
        var turn = GameManagerChain.Instance.GameStateEnum == GameStateEnum.Human ? true : false;
        if (turn == true)
        {
            Debug.Log(GameManagerChain.Instance.GameStateEnum);
            StartCoroutine(GameManagerChain.Instance.StateToAI());
            // GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
        }
        else
        {
            Debug.Log(GameManagerChain.Instance.GameStateEnum);
            StartCoroutine(GameManagerChain.Instance.StateToHuman());
            // GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
        }
        var lvlMono = LevelMono.Instance;
        lvlMono.RemoveHighlight();
        lvlMono.ResetPiece();
    }

    public void ShowAbilityButton()
    {
        Debug.Log("Ability Open time");
        _abilityUseObject.SetActive(true);

        //var triangle = GridManager.Instance.storedPiece;
        _abilityUseObject.GetComponentInChildren<Button>().onClick.AddListener(() => TriangleEvent());
    }

    public void HideAbilityButton()
    {
        Debug.Log("Ability Close time");
        _abilityUseObject.SetActive(false);
    }

    public void TriangleEvent()//(Piece triangle, Piece other)
    {
        Debug.Log("BUTTON WORKS");
        if (GameManagerChain.Instance.UsedAbility == false)
        {
            GameManagerChain.Instance.DecrementMoves(1);
            GameManagerChain.Instance.UsedAbility = true;
            this.ShowNumMovesInfo();
        }
        this.HideAbilityButton();
    }

    public void ShowUnitInfo(PieceMono piece)
    {
        if (piece == null)
        {
            _selectedUnitInfo.SetActive(false);
            return;
        }
        if (_selectedUnitInfo != null)
        {
            _selectedUnitInfo.SetActive(true);
            unitInfo.text = piece.GetName();
            unitAbility.text = piece.getUnitInfo();
        }
    }

    public void HideUnitInfo(PieceMono piece)
    {
        if (piece == null)
        {
            _selectedUnitInfo.SetActive(false);
            return;
        }
        // Necessary?
        unitInfo.text = "Unit Name";
        unitAbility.text = "Unit Ability";
        _selectedUnitInfo.SetActive(false);
    }

    public void UpdateObjectiveContent()
    {
        TextMeshProUGUI tmpro = _objectiveContent.GetComponent<TextMeshProUGUI>();

        if (SceneManager.GetActiveScene().name == "TutorialLevel")
            switch (GameManagerChain.Instance.TotalMoves)
            {
                case 0:     // First move -- player must move diamond to the circle
                    //Position 3.25,-0.25, -2
                    tmpro.text = "Click the diamond to select it.";
                    _pointerObject.gameObject.SetActive(true);

                    _pointerObject.transform.position = new Vector3(3.25f, -0.25f, -2f);
                    if (LevelMono.Instance.selectedPiece != null && LevelMono.Instance.selectedPiece.IsDiamond())
                    {
                        _pointerObject.transform.position = new Vector3(1.25f, -0.25f, -2f);
                        tmpro.text = "Click the highlighted region to move the diamond to a legal position. Each unit may only move once, without the help of an ability.";
                    }
                    break;
                case 1:     // Second move -- player must move circle next to triangle, directly in front of enemy
                    _pointerObject.transform.position = new Vector3(0.25f, -0.25f, -2f);
                    tmpro.text = "The diamond increased the circle's movement ability. Click the circle to select it.";
                    if (LevelMono.Instance.selectedPiece != null && LevelMono.Instance.selectedPiece.IsCircle())
                    {
                        _pointerObject.transform.position = new Vector3(3.25f, 2.75f, -2f);
                        tmpro.text = "The triangle is a resource, rather than a unit, that will be useful later on, and does not need to be captured to win. Move the circle to the triangle.";
                    }
                    break;
                case 2:     
                    _pointerObject.transform.position = new Vector3(3.25f, 2.75f, -2f);
                    tmpro.text = "Click the circle again to select it.";
                    if (LevelMono.Instance.selectedPiece != null && LevelMono.Instance.selectedPiece.IsCircle())
                    {
                        _pointerObject.transform.position = new Vector3(3.25f, 3.75f, -2f);
                        tmpro.text = "Use the circle again to capture the nearest enemy (red) unit.";
                    }

                    break;
                case 3: // Free movement -- player freely maneuvers
                    _pointerObject.SetActive(false);
                    tmpro.text = "Capturing an enemy unit with the circle allows the it to be used again, but consumes another one of your Moves. Use it to capture the final enemy unit.";
                    break;
            }
        if (SceneManager.GetActiveScene().name == "TutorialFogOfWar")
        {
            if (!LevelMono.Instance.capturedTower)
            {
                tmpro.text = "The fog of war hinders your vision. The triangle grants vision to the team that owns it. Move any of your units to a tile adjacent to the enemy triangle to capture it.";
            }
            else
            {
                tmpro.text = "Now that you own the triangle, you have vision of the enemy. Capture the enemy units.";
            }
        }
            
    }
    public void SetSlackDialogue(bool status)
    {
        _slackObject.SetActive(status);
    }

    public void SetVictoryScreen(bool status)
    {
        _victoryObject.SetActive(status);

        //Set every other UI elements to inactive
        if(status)
        {
            _turnInfoObject.SetActive(false);
            _selectedUnitInfo.SetActive(false);
            _numTurnObject.SetActive(false);
            _abilityUseObject.SetActive(false);
            _endTurnObject.SetActive(false);
            _slackObject.SetActive(false);
            _pauseObject.SetActive(false);

            if (SceneManager.GetActiveScene().name == "TutorialLevel" || SceneManager.GetActiveScene().name == "TutorialFogOfWar")
            {
                _objectiveObject.SetActive(false);
            }
        }

    }

    private IEnumerator FingerBlink()
    {
        while(_pointerObject.activeInHierarchy == true)
        {
         _pointerObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);      // Visible
         yield return new WaitForSeconds(0.5f);
         _pointerObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);      // Invisible
         yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);      // Right after start, set position to JUST in front of the grid
        this.transform.position = new Vector3(LevelMono.Instance.transform.position.x, LevelMono.Instance.transform.position.y, -10);

    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            _objectiveObject.SetActive(true);
            MenuManager.Instance.UpdateObjectiveContent();
            StartCoroutine(FingerBlink());
            _endTurnObject.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name == "TutorialFogOfWar")
        {
            _objectiveObject.SetActive(true);
            MenuManager.Instance.UpdateObjectiveContent();
            _endTurnObject.SetActive(false);
        }
        else
        {
            _objectiveObject.SetActive(false);
            _endTurnObject.SetActive(true);
        }
        SetVictoryScreen(false);
        _turnInfoObject.SetActive(true);
        _selectedUnitInfo.SetActive(false);
        _numTurnObject.SetActive(true);
        _abilityUseObject.SetActive(false);
        _slackObject.SetActive(false);
        _pauseObject.SetActive(true);
        _endTurnObject.GetComponentInChildren<Button>().onClick.AddListener(() => EndTurnEvent());

        StartCoroutine(LateStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "TutorialLevel" || SceneManager.GetActiveScene().name == "TutorialFogOfWar")
        {
            UpdateObjectiveContent();
        }
    }

    void OnDestroy()
    {
        _endTurnObject.GetComponentInChildren<Button>().onClick.RemoveListener(() => EndTurnEvent());
    }
}