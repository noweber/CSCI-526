using System;
using static Assets.TerrainPrototype.Source.TerrainMappings;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace Assets.TerrainPrototype.Source
{
    // TODO: Comment all code. :)
    public class WorldMap : IWorldMap
    {
        private int width;

        public int GetMapWidth()
        {
            return width;
        }

        private int height;

        public int GetMapHeight()
        {
            return height;
        }

        private bool isElementalMapSeeded;

        private Element[,] elementalMap;

        private TerrainType[,] terrainMap;

        public WorldMap(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }
            this.width = width;
            this.height = height;
            elementalMap = new Element[width, height];
            terrainMap = new TerrainType[width, height];
            isElementalMapSeeded = false;
            GenerateMap();
        }

        public WorldMap(Element[,] elementsMap)
        {
            if (elementsMap == null || elementsMap.GetLength(0) == 0 || elementsMap.GetLength(1) == 0)
            {
                throw new ArgumentException(nameof(elementsMap));
            }
            width = elementsMap.GetLength(0);
            height = elementsMap.GetLength(1);
            elementalMap = elementsMap;
            terrainMap = new TerrainType[elementsMap.GetLength(0), elementsMap.GetLength(1)];
            isElementalMapSeeded = true;
            CalculateTerrain();
        }

        public Element LookupElementOfCell(int x, int y)
        {
            if (x < 0 || x >= elementalMap.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (y < 0 || y >= elementalMap.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            if (!isElementalMapSeeded)
            {
                GenerateMap();
            }

            return elementalMap[x, y];
        }

        public TerrainType LookupTerrainOfCell(int x, int y)
        {
            if (x < 0 || x >= terrainMap.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (y < 0 || y >= terrainMap.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            if (!isElementalMapSeeded)
            {
                GenerateMap();
            }

            return terrainMap[x, y];
        }

        public void SetElementAtCell(int x, int y, Element element)
        {
            ValidateCoordinatesInMaps(x, y);
            elementalMap[x, y] = element;
            // Note: No change to surrounding elements occurs until the next generation.
        }

        public void GotoNextGeneration()
        {
            ValidateDimensionsOfMaps();
            Element[,] nextGenerationMap = new Element[elementalMap.GetLength(0), elementalMap.GetLength(1)];

            for (int i = 0; i < elementalMap.GetLength(0); i++)
            {
                for (int j = 0; j < elementalMap.GetLength(1); j++)
                {
                    nextGenerationMap[i, j] = elementalMap[i, j];
                    Dictionary<Element, int> neighboringElements = GetEightNearestNeighboringElementCounts(i, j);

                    // If two of the neighboring cells are of the same element,
                    // then this tile's element base is stable:
                    Element currentTileElement = elementalMap[i, j];
                    bool elementIsStable = (neighboringElements.ContainsKey(currentTileElement) && neighboringElements[currentTileElement] >= 2);

                    foreach (KeyValuePair<Element, int> elementCountKvp in neighboringElements)
                    {

                        // If 3 or more of the neighboring elements are of a different element than the tile,
                        // then it is under pressure to convert to that element.
                        if (elementCountKvp.Value >= 3 && elementCountKvp.Key != currentTileElement)
                        {
                            // A tile with an unstable neighboring base will convert when under pressure:
                            if (!elementIsStable)
                            {
                                nextGenerationMap[i, j] = elementCountKvp.Key;
                            }
                        }
                    }
                }
            }

            // The elemental map needs to be updated to the next generation and the terrain then gets recalculated:
            elementalMap = nextGenerationMap;
            CalculateTerrain();
        }

        private void GenerateMap()
        {
            GenerateElementalMap();
            CalculateTerrain();
        }

        private void GenerateElementalMap()
        {
            elementalMap = new Element[width, height];

            for (int i = 0; i < elementalMap.GetLength(0); i++)
            {
                for (int j = 0; j < elementalMap.GetLength(1); j++)
                {
                    elementalMap[i, j] = (Element)Random.Range(0, Enum.GetValues(typeof(Element)).Length);
                }
            }
            isElementalMapSeeded = true;
        }

        private void ValidateMaps()
        {
            if (elementalMap == null || terrainMap == null)
            {
                throw new ApplicationException("Elemental map and/or terrain map is null.");
            }
        }

        private void ValidateCoordinatesInMaps(int x, int y)
        {
            if (!isElementalMapSeeded)
            {
                throw new ApplicationException("Elemental map is not initialized.");
            }

            ValidateMaps();

            if (x < 0 || x >= elementalMap.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x < 0 || y >= elementalMap.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            ValidateDimensionsOfMaps();
        }

        private void ValidateDimensionsOfMaps()
        {
            ValidateMaps();

            if (elementalMap.GetLength(0) != terrainMap.GetLength(0) || elementalMap.GetLength(1) != terrainMap.GetLength(1))
            {
                throw new ApplicationException("Elemental map dimensions do not match the terrain map dimensions.");
            }
        }

        private void CalculateTerrain()
        {
            ValidateDimensionsOfMaps();

            for (int i = 0; i < elementalMap.GetLength(0); i++)
            {
                for (int j = 0; j < elementalMap.GetLength(1); j++)
                {
                    terrainMap[i, j] = CalculateCellTerrainType(i, j);
                }
            }
        }

        private TerrainType CalculateCellTerrainType(int x, int y)
        {
            ValidateCoordinatesInMaps(x, y);

            Dictionary<Element, int> elementCounts = GetEightNearestNeighboringElementCounts(x, y);

            // TODO: Refactor this to sort the keys by count for this calculation.
            int highestElementCount = 0;
            List<Element> highestElementCountsOfNeighbors = new();
            foreach (KeyValuePair<Element, int> kvp in elementCounts)
            {
                if (kvp.Value > highestElementCount)
                {
                    highestElementCount = kvp.Value;
                    highestElementCountsOfNeighbors = new();
                    highestElementCountsOfNeighbors.Add(kvp.Key);
                }
                else if (kvp.Value == highestElementCount)
                {
                    highestElementCountsOfNeighbors.Add(kvp.Key);
                }
            }

            if (highestElementCountsOfNeighbors.Count == 1)
            {
                // If only one element has the highest count of the cells neighbors,
                // Then use that element alone when calculating the terrain:
                return TerrainMappings.GetTerrainByElements(highestElementCountsOfNeighbors[0], highestElementCountsOfNeighbors[0]);
            }
            else if (highestElementCountsOfNeighbors.Count == 2)
            {
                return TerrainMappings.GetTerrainByElements(highestElementCountsOfNeighbors[0], highestElementCountsOfNeighbors[1]);
            }
            else
            {
                // If there are more than two types of elements tied for being the highest count,
                // then just use the terrains element for both when calculating the terrain:
                return TerrainMappings.GetTerrainByElements(elementalMap[x, y], elementalMap[x, y]);
            }
        }

        private Dictionary<Element, int> GetEightNearestNeighboringElementCounts(int x, int y)
        {
            ValidateCoordinatesInMaps(x, y);

            List<Element> neighbors = new();

            if (x - 1 >= 0)
            {
                neighbors.Add(elementalMap[x - 1, y]);

                if (y - 1 >= 0)
                {
                    neighbors.Add(elementalMap[x - 1, y - 1]);
                }

                if (y + 1 < elementalMap.GetLength(1))
                {
                    neighbors.Add(elementalMap[x - 1, y + 1]);
                }
            }
            if (x + 1 < elementalMap.GetLength(0))
            {
                neighbors.Add(elementalMap[x + 1, y]);

                if (y - 1 >= 0)
                {
                    neighbors.Add(elementalMap[x + 1, y - 1]);
                }

                if (y + 1 < elementalMap.GetLength(1))
                {
                    neighbors.Add(elementalMap[x + 1, y + 1]);
                }
            }
            if (y - 1 >= 0)
            {
                neighbors.Add(elementalMap[x, y - 1]);
            }

            if (y + 1 < elementalMap.GetLength(1))
            {
                neighbors.Add(elementalMap[x, y + 1]);
            }

            Dictionary<Element, int> elementCounts = new();
            foreach (Element element in neighbors)
            {
                if (elementCounts.ContainsKey(element))
                {
                    elementCounts[element] += 1;
                }
                else
                {
                    elementCounts.TryAdd(element, 1);
                }
            }

            return elementCounts;
        }
    }
}
