using static Assets.TerrainPrototype.Source.TerrainMappings;

namespace Assets.TerrainPrototype.Source
{
    public interface IWorldMap
    {
        int GetMapWidth();

        int GetMapHeight();

        Element LookupElementOfCell(int x, int y);

        TerrainType LookupTerrainOfCell(int x, int y);

        void SetElementAtCell(int x, int y, Element element);

        void GotoNextGeneration();
    }
}
