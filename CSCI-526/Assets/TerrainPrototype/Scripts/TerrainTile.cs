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

    [SerializeField]
    private TerrainType terrainType;

    [SerializeField]
    private Element firstElement;

    [SerializeField]
    private Element secondElement;

    [SerializeField]
    private Vector2Int cellCoordinates;

    private IWorldMap worldMapData;

    private bool isTileDataSet;

    private bool isMaterialDataSet;

    private void Awake()
    {
        isTileDataSet = false;
        isMaterialDataSet = false;
    }

    public void SetTileData(int x, int y, ref IWorldMap mapData)
    {
        // TODO: validate inputs
        worldMapData = mapData;
        terrainType = mapData.LookupTerrainOfCell(x, y);
        var elements = TerrainMappings.GetElementsByTerrain(terrainType);
        firstElement = elements.Item1;
        secondElement = elements.Item2;
        cellCoordinates = new Vector2Int(x, y);
        isTileDataSet = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // TODO: for now, poll for any changes of the terrain in the map data.
        // TODO: later, use an observable to see the terrain changes.

        if (isTileDataSet && !isMaterialDataSet)
        {
            if (elementMaterials != null)
            {
                if (ElementOneRenderer != null && ElementTwoRenderer != null)
                {
                    // TODO: Check that the list of materials is the same as the number of types of elements, else log an error.
                    if (elementMaterials.Count == Enum.GetValues(typeof(Element)).Length)
                    {
                        ElementOneRenderer.material = elementMaterials[(int)firstElement];
                        ElementTwoRenderer.material = elementMaterials[(int)secondElement];
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
            isMaterialDataSet = true;
        }
    }
}
