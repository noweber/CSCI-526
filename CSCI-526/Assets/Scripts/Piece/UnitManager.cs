using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptablePiece> _pieces;

    public PieceController SelectedHero;

    public PieceController SelectedEnemy;

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
            var randomPrefab = GetRandomUnit<PieceController>(Alignment.Player);
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
            var randomPrefab = GetRandomUnit<PieceController>(Alignment.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = WorldMapController.Instance.GetRandomSpawnTile();
            //randomSpawnTile.SetUnitOccupant(spawnedEnemy);
        }
    }

    private T GetRandomUnit<T>(Alignment faction) where T : PieceController
    {
        return (T)_pieces.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedHero(PieceController hero)
    {
        SelectedHero = hero;
        //Prototype2MenuManager.Instance.ShowSelectedHero(hero);
    }

    public void SetSelectedEnemy(PieceController enemy)
    {
        SelectedEnemy = enemy;
        //Prototype2MenuManager.Instance.ShowSelectedEnemy(enemy);
    }

}
