using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ZenUnitManager : MonoBehaviour {
    public static ZenUnitManager Instance;

    private List<ScriptableUnit> _units;
    public BaseHero SelectedHero;
    public BaseEnemy SelectedEnemy;

    void Awake() {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();

    }

    public void SpawnHeroes() {
        var heroCount = 2;

        for (int i = 0; i < heroCount; i++) {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = ZenGridManager.Instance.GetHeroSpawnTile();

            randomSpawnTile.SetUnit(spawnedHero);
        }

        ZenGameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    public void SpawnEnemies()
    {
        var enemyCount = 2;

        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = ZenGridManager.Instance.GetEnemySpawnTile();

            randomSpawnTile.SetUnit(spawnedEnemy);
        }

        ZenGameManager.Instance.ChangeState(GameState.HeroesTurn);
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedHero(BaseHero hero) {
        SelectedHero = hero;
        ZenMenuManager.Instance.ShowSelectedHero(hero);
    }

    public void SetSelectedEnemy(BaseEnemy enemy)
    {
        SelectedEnemy = enemy;
        ZenMenuManager.Instance.ShowSelectedEnemy(enemy);
    }
}
