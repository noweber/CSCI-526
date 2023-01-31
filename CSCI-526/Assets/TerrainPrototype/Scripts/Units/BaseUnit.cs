using Assets.TerrainPrototype.Source;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public string UnitName;
    public TerrainTile OccupiedTile;
    public Faction Faction;

    public string GetSummary()
    {
        return UnitName + "\nLife: " + GetLife() + ", Damage: " + GetDamage();
    }

    public int GetDamage(bool withTerrainBonuses = true)
    {
        if (withTerrainBonuses && OccupiedTile != null)
        {
            return damage + TerrainMappings.GetDamageBonusByTerrainType(OccupiedTile.TerrainType);
        }
        return damage;
    }

    public int GetLife(bool withTerrainBonuses = true)
    {
        if (withTerrainBonuses && OccupiedTile != null)
        {
            return life + TerrainMappings.GetLifeBonusByTerrainType(OccupiedTile.TerrainType);
        }
        return life;
    }

    private int damage;
    private int life;

    private void Awake()
    {
        damage = Random.Range(1, 3);
        life = Random.Range(1, 3);
    }
}
