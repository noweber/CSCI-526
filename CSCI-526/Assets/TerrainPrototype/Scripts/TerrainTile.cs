using Assets.TerrainPrototype.Source;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.TerrainPrototype.Source.TerrainMappings;
using TerrainMappings = Assets.TerrainPrototype.Source.TerrainMappings;

public class TerrainTile : MonoBehaviour
{
    public List<Material> elementMaterials;

    public MeshRenderer ElementOneRenderer;

    public MeshRenderer ElementTwoRenderer;

    public BaseUnit Occupant { get; private set; }

    public TerrainType TerrainType { get; private set; }

    public bool IsPassable { get; private set; }

    public Vector2Int MapCoordinates { get; private set; }

    public Element FirstElement { get; private set; }

    public Element SecondElement { get; private set; }

    private IWorldMap worldMapData;

    private bool isTileDataSet;

    [SerializeField]
    private GameObject mouseoverHighlight;


    private void Awake()
    {
        isTileDataSet = false;
        IsPassable = true;
    }

    public void SetTileData(int x, int y, ref IWorldMap mapData, bool isPassable = true)
    {
        // TODO: validate inputs
        worldMapData = mapData;
        MapCoordinates = new Vector2Int(x, y);
        isTileDataSet = true;
        IsPassable = isPassable;
        PollMapData();
    }

    public void SetUnitOccupant(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.Occupant = null;
        unit.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        Occupant = unit;
        unit.OccupiedTile = this;
    }

    public void PollMapData()
    {
        TerrainType = worldMapData.LookupTerrainOfCell(MapCoordinates.x, MapCoordinates.y);
        var elements = TerrainMappings.GetElementsByTerrain(TerrainType);
        FirstElement = elements.Item1;
        SecondElement = elements.Item2;

        if (isTileDataSet)
        {
            if (elementMaterials != null)
            {
                if (ElementOneRenderer != null && ElementTwoRenderer != null)
                {
                    // TODO: Check that the list of materials is the same as the number of types of elements, else log an error.
                    if (elementMaterials.Count == Enum.GetValues(typeof(Element)).Length)
                    {
                        ElementOneRenderer.material = elementMaterials[(int)FirstElement];
                        ElementTwoRenderer.material = elementMaterials[(int)SecondElement];
                    }
                    else
                    {
                        Debug.LogError("The set of materials for each element is of a different size than the number of elements. Fix this in the inspector.");
                    }
                }
                else
                {
                    Debug.LogError("A required reference to the render of the tile's meshes is null. Set this in the inspector.");
                }
            }
            else
            {
                Debug.LogError("The map of elements to materials is null. The tile's materials cannot be set. Add these in the inspector.");
            }
        }
    }

    void OnMouseEnter()
    {
        if (mouseoverHighlight != null)
        {
            mouseoverHighlight.SetActive(true);
        }
        else
        {
            Debug.LogError("Cannot set the mouseover highlight on the tile because the reference is null. Fix this in the inspector.");
        }
        Prototype2MenuManager.Instance.ShowTileInfo(this);
    }

    void OnMouseExit()
    {
        if (mouseoverHighlight != null)
        {
            mouseoverHighlight.SetActive(false);
        }
        else
        {
            Debug.LogError("The mouseover highlight is null when trying to deactivate it. Fix this in the inspector.");
        }
        Prototype2MenuManager.Instance.ShowTileInfo(null);
    }

    void OnMouseDown()
    {
        if (SpellManager.Instance.SpellSelected && (ZenGameManager.Instance.GameState == GameState.HeroesSpell || ZenGameManager.Instance.GameState == GameState.EnemiesSpell))
        {
            WorldMapController.Instance.ChangeCellElement(MapCoordinates.x, MapCoordinates.y, SpellManager.Instance.SelectedElement);
            PollMapData();
            Prototype2MenuManager.Instance.HideSpellUi();

            // Transition to the turn phase
            ZenGameManager.Instance.ChangeState(ZenGameManager.Instance.GameState + 1);
        }

        //If it's enemy(team WHITE)'s turn
        if (ZenGameManager.Instance.GameState == GameState.EnemiesTurn)
        {
            if (Occupant != null)
            {
                if (Occupant.Faction == Faction.Enemy) ZenUnitManager.Instance.SetSelectedEnemy((BaseEnemy)Occupant);
                else
                {
                    if (ZenUnitManager.Instance.SelectedEnemy != null)
                    {
                        var hero = (BaseHero)Occupant;
                        SetUnitOccupant(ZenUnitManager.Instance.SelectedEnemy);
                        Destroy(hero.gameObject);
                        ZenUnitManager.Instance.SetSelectedEnemy(null);

                        //Change to enemy's turn
                        Prototype2MenuManager.Instance.ShowPlayerBLACKPhase();
                        Prototype2MenuManager.Instance.HideMoveUi();
                        ZenGameManager.Instance.ChangeState(GameState.HeroesSpell);
                    }
                }
            }
            else
            {
                //Check if the targeted tile is walkable
                if (IsPassable)
                {
                    //Set the targeted tile to the selected hero's position and move prefab over
                    if (ZenUnitManager.Instance.SelectedEnemy != null)
                    {
                        SetUnitOccupant(ZenUnitManager.Instance.SelectedEnemy);
                        ZenUnitManager.Instance.SetSelectedEnemy(null);

                        //Change to enemy's turn
                        Prototype2MenuManager.Instance.ShowPlayerBLACKPhase();
                        Prototype2MenuManager.Instance.HideMoveUi();
                        ZenGameManager.Instance.ChangeState(GameState.HeroesSpell);
                    }
                }
            }
        }

        //If it's hero(team 2)'s turn
        if (ZenGameManager.Instance.GameState == GameState.HeroesTurn)
        {
            if (Occupant != null)
            {
                if (Occupant.Faction == Faction.Hero) ZenUnitManager.Instance.SetSelectedHero((BaseHero)Occupant);
                else
                {
                    if (ZenUnitManager.Instance.SelectedHero != null)
                    {
                        var enemy = (BaseEnemy)Occupant;
                        SetUnitOccupant(ZenUnitManager.Instance.SelectedHero);
                        Destroy(enemy.gameObject);
                        ZenUnitManager.Instance.SetSelectedHero(null);

                        //Change to enemy's turn
                        Prototype2MenuManager.Instance.ShowPlayerWHITEPhase();
                        Prototype2MenuManager.Instance.HideMoveUi();
                        ZenGameManager.Instance.ChangeState(GameState.EnemiesSpell);
                    }
                }
            }
            else
            {
                //Check if the targeted tile is walkable
                if (IsPassable)
                {
                    //Set the targeted tile to the selected hero's position and move prefab over
                    if (ZenUnitManager.Instance.SelectedHero != null)
                    {
                        SetUnitOccupant(ZenUnitManager.Instance.SelectedHero);
                        ZenUnitManager.Instance.SetSelectedHero(null);

                        //Change to enemy's turn
                        Prototype2MenuManager.Instance.ShowPlayerWHITEPhase();
                        Prototype2MenuManager.Instance.HideMoveUi();
                        ZenGameManager.Instance.ChangeState(GameState.EnemiesSpell);
                    }
                }
            }
        }
    }
}
