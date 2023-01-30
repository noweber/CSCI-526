using System;
using UnityEngine;

public class ZenGameManager : MonoBehaviour
{
    public static ZenGameManager Instance;
    public GameState GameState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                WorldMapController.Instance.CreateWorldMap(WorldMapController.Instance.MapSize);
                ChangeState(GameState.SpawnHeroes);
                break;
            case GameState.SpawnHeroes:
                ZenUnitManager.Instance.SpawnHeroes();
                ChangeState(GameState.SpawnEnemies);
                break;
            case GameState.SpawnEnemies:
                ZenUnitManager.Instance.SpawnEnemies();
                ChangeState(GameState.HeroesSpell);
                break;
            case GameState.HeroesSpell:
                Prototype2MenuManager.Instance.ShowSpellUi();
                break;
            case GameState.HeroesTurn:
                Prototype2MenuManager.Instance.ShowMoveUi();
                break;
            case GameState.EnemiesSpell:
                Prototype2MenuManager.Instance.ShowSpellUi();
                break;
            case GameState.EnemiesTurn:
                Prototype2MenuManager.Instance.ShowMoveUi();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    HeroesSpell = 3,
    HeroesTurn = 4,
    EnemiesSpell = 5,
    EnemiesTurn = 6
}
