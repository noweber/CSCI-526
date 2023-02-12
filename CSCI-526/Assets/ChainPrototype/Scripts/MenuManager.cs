using Assets.Scripts.Units;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    [SerializeField] public GameObject _turnInfoObject, _selectedUnitInfo, _numTurnObject, _abilityUseObject, _endTurnObject, _objectiveObject, _objectiveContent, _slackObject, _pauseObject, _victoryObject, _pointerObject;
    [SerializeField] private TextMeshProUGUI unitInfo, unitAbility;     // Text components of Unit game object

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
        else
        {
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
        _endTurnObject.GetComponentInChildren<Button>().onClick.AddListener(() => EndTurnEvent());
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
            GameManagerChain.Instance.ChangeState(GameStateEnum.AI);
        }
        else
        {
            Debug.Log(GameManagerChain.Instance.GameStateEnum);
            GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
        }
        ShowTurnInfo();
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
                // 0
                // Click the diamond to select it.
                // Click the highlighted region to move the diamond to a legal position.

                // 1
                // The diamond increased the circle's movement ability. Move the circle to the triangle.

                // The circle is the main attacker for your team. Use it to capture the nearest enemy (red) unit.
                // 2
                // Capturing an enemy unit gave the circle another move. Use it to capture the final enemy unit.

                case 0:     // First move -- player must move diamond to the circle
                    //Position 3.25,-0.25, -2
                    tmpro.text = "Click the diamond to select it.";
                    _pointerObject.transform.position = new Vector3(3.25f, -0.25f, -2f);
                    if (LevelMono.Instance.selectedPiece != null && LevelMono.Instance.selectedPiece.IsDiamond())
                    {
                        _pointerObject.transform.position = new Vector3(1.25f, -0.25f, -2f);
                        tmpro.text = "Click the highlighted region to move the diamond to a legal position.";
                    }
                    break;
                case 1:     // Second move -- player must move circle next to triangle, directly in front of enemy
                    _pointerObject.transform.position = new Vector3(0.25f, -0.25f, -2f);
                    tmpro.text = "The diamond increased the circle's movement ability. Click the circle to select it.";
                    if (LevelMono.Instance.selectedPiece != null && LevelMono.Instance.selectedPiece.IsCircle())
                    {
                        _pointerObject.transform.position = new Vector3(3.25f, 2.75f, -2f);
                        tmpro.text = " Move the circle to the triangle.";
                    }
                    break;
                case 2:     // Free movement -- player freely maneuvers
                    _pointerObject.transform.position = new Vector3(3.25f, 3.75f, -2f);
                    tmpro.text = "Any unit that moves adjacent to a triangle may move infinitely. Use the circle again to capture the nearest enemy(red) unit.";
                    break;
                case 3:
                    _pointerObject.SetActive(false);
                    tmpro.text = "Capturing an enemy unit gave the circle another move. Use it to capture the final enemy unit.";
                    break;
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
        _turnInfoObject.SetActive(false);
        _selectedUnitInfo.SetActive(false);
        _numTurnObject.SetActive(false);
        _abilityUseObject.SetActive(false);
        _endTurnObject.SetActive(false);
        _objectiveContent.SetActive(false);
        _slackObject.SetActive(false);
        _pauseObject.SetActive(false);
        _objectiveObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            _objectiveContent.SetActive(true);
            _objectiveObject.SetActive(true);
            MenuManager.Instance.UpdateObjectiveContent();
        }
        else
        {
            _objectiveContent.SetActive(false);
            _objectiveObject.SetActive(false);
        }
        _turnInfoObject.SetActive(true);
        _selectedUnitInfo.SetActive(false);
        _numTurnObject.SetActive(true);
        _abilityUseObject.SetActive(false);
        _endTurnObject.SetActive(true);
        _slackObject.SetActive(false);
        _pauseObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        // TODO: REMOVE FROM UPDATE
        if (SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            UpdateObjectiveContent();
        }
    }
}