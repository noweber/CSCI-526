using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptablePiece> _pieces;

    public PieceMono SelectedHero;

    public PieceMono SelectedEnemy;

    void Awake()
    {
        Instance = this;
        _pieces = Resources.LoadAll<ScriptablePiece>("Scriptables").ToList();
    }

    public void SpawnHeroes()
    {
        var heroCount = 3;

        for (int i = 0; i < heroCount; i++)
        {
            var randomPrefab = GetRandomUnit<PieceMono>(Alignment.Player);
            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = WorldMapController.Instance.GetRandomSpawnTile();
            //randomSpawnTile.SetUnitOccupant(spawnedHero);
        }
    }

    public void SpawnEnemies()
    {
        var enemyCount = 3;

        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<PieceMono>(Alignment.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = WorldMapController.Instance.GetRandomSpawnTile();
            //randomSpawnTile.SetUnitOccupant(spawnedEnemy);
        }
    }

    private T GetRandomUnit<T>(Alignment faction) where T : PieceMono
    {
        return (T)_pieces.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedHero(PieceMono hero)
    {
        SelectedHero = hero;
        //Prototype2MenuManager.Instance.ShowSelectedHero(hero);
    }

    public void SetSelectedEnemy(PieceMono enemy)
    {
        SelectedEnemy = enemy;
        //Prototype2MenuManager.Instance.ShowSelectedEnemy(enemy);
    }

}
