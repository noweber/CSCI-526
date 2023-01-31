using System;
using System.Collections.Generic;

namespace Assets.TerrainPrototype.Source
{
    public static class TerrainMappings
    {
        public enum Element
        {
            Fire = 0,
            Water = 1,
            Earth = 2,
            Air = 3
        }

        public enum TerrainType
        {
            Desert = 0, // Fire + Fire
            Beach = 1, // Water + Fire
            Lake = 2, // Water + Water
            Jungle = 3, //Earth + Fire
            Marsh = 4, // Earth + Water
            Forest = 5, // Earth + Earth
            Rock = 6, // Air + Fire
            Shallows = 7, // Air + Water
            Grasslands = 9, // Air + Earth
            Plains = 10, // Air + Air
        }

        public static int GetLifeBonusByTerrainType(TerrainType terrain)
        {
            switch (terrain)
            {
                case TerrainType.Forest:
                    return 2;
                case TerrainType.Jungle:
                case TerrainType.Marsh:
                case TerrainType.Grasslands:
                    return 1;
                default:
                    return 0;
            }
        }

        public static int GetDamageBonusByTerrainType(TerrainType terrain)
        {
            switch (terrain)
            {
                case TerrainType.Desert:
                    return 2;
                case TerrainType.Beach:
                case TerrainType.Rock:
                case TerrainType.Plains:
                    return 1;
                default:
                    return 0;
            }
        }

        public static Tuple<Element, Element> GetElementsByTerrain(TerrainType terrain)
        {
            return terrain switch
            {
                TerrainType.Desert => new Tuple<Element, Element>(Element.Fire, Element.Fire),
                TerrainType.Beach => new Tuple<Element, Element>(Element.Water, Element.Fire),
                TerrainType.Lake => new Tuple<Element, Element>(Element.Water, Element.Water),
                TerrainType.Jungle => new Tuple<Element, Element>(Element.Earth, Element.Fire),
                TerrainType.Marsh => new Tuple<Element, Element>(Element.Earth, Element.Water),
                TerrainType.Forest => new Tuple<Element, Element>(Element.Earth, Element.Earth),
                TerrainType.Rock => new Tuple<Element, Element>(Element.Air, Element.Fire),
                TerrainType.Shallows => new Tuple<Element, Element>(Element.Air, Element.Water),
                TerrainType.Grasslands => new Tuple<Element, Element>(Element.Air, Element.Earth),
                _ => new Tuple<Element, Element>(Element.Air, Element.Air),
            };
        }

        public static TerrainType GetTerrainByElements(Element firstElement, Element secondElement)
        {
            return elementsToTerrainMapping[firstElement][secondElement];
        }

        private static readonly Dictionary<Element, Dictionary<Element, TerrainType>> elementsToTerrainMapping = new()
        {
            {
                Element.Fire,
                new Dictionary<Element, TerrainType>() {
                    { Element.Fire, TerrainType.Desert },
                    { Element.Water, TerrainType.Beach },
                    { Element.Earth, TerrainType.Jungle },
                    { Element.Air, TerrainType.Rock },
                }
            },
            {
                Element.Water,
                new Dictionary<Element, TerrainType>() {
                    { Element.Fire, TerrainType.Beach },
                    { Element.Water, TerrainType.Lake },
                    { Element.Earth, TerrainType.Marsh },
                    { Element.Air, TerrainType.Shallows },
                }
            },
            {
                Element.Earth,
                new Dictionary<Element, TerrainType>() {
                    { Element.Fire, TerrainType.Jungle },
                    { Element.Water, TerrainType.Marsh },
                    { Element.Earth, TerrainType.Forest },
                    { Element.Air, TerrainType.Grasslands },
                }
            },
            {
                Element.Air,
                new Dictionary<Element, TerrainType>() {
                    { Element.Fire, TerrainType.Rock },
                    { Element.Water, TerrainType.Shallows },
                    { Element.Earth, TerrainType.Grasslands },
                    { Element.Air, TerrainType.Plains },
                }
            }
        };
    }
}
