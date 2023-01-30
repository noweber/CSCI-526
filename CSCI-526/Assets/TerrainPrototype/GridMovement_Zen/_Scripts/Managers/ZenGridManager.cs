using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZenGridManager : MonoBehaviour {
    public static ZenGridManager Instance;
    [SerializeField] private int _width, _height;

    [SerializeField] private ZenTile _grassTile, _mountainTile;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, ZenTile> _tiles;

    void Awake() {
        Instance = this;
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, ZenTile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++) {
                var randomTile = Random.Range(0, 6) == 3 ? _mountainTile : _grassTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

              
                spawnedTile.Init(x,y);


                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

        ZenGameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public ZenTile GetHeroSpawnTile() {
        //Debug.Log(_tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value);
        return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public ZenTile GetEnemySpawnTile()
    {
        //Debug.Log(_tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value);
        return _tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public ZenTile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}