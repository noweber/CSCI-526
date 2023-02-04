using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    //private List<ScriptableUnit> _units;

    //public BaseHero SelectedHero;

    //public BaseEnemy SelectedEnemy;

    void Awake()
    {
        Instance = this;
        //_units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnHeroes()
    {
        /*
        var heroCount = 3;

        for (int i = 0; i < heroCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = WorldMapController.Instance.GetRandomSpawnTile();
            randomSpawnTile.SetUnitOccupant(spawnedHero);
        }
        */
    }

    public void SpawnEnemies()
    {
        /*
        var enemyCount = 3;

        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = WorldMapController.Instance.GetRandomSpawnTile();
            randomSpawnTile.SetUnitOccupant(spawnedEnemy);
        }
        */
    }

    /*
    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedHero(BaseHero hero)
    {
        SelectedHero = hero;
        Prototype2MenuManager.Instance.ShowSelectedHero(hero);
    }

    public void SetSelectedEnemy(BaseEnemy enemy)
    {
        SelectedEnemy = enemy;
        Prototype2MenuManager.Instance.ShowSelectedEnemy(enemy);
    }
    */

}
