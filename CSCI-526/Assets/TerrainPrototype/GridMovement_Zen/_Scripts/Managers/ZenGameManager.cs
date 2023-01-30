using System;
using System.Collections;
using System.Collections.Generic;
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
                ZenGridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnHeroes:
                ZenUnitManager.Instance.SpawnHeroes();
                break;
            case GameState.SpawnEnemies:
                ZenUnitManager.Instance.SpawnEnemies();
                break;
            case GameState.HeroesTurn:
                break;
            case GameState.EnemiesTurn:
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
    HeroesTurn = 3,
    EnemiesTurn = 4
}
