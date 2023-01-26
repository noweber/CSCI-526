using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width = 6, _height = 6;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Triangle _trianglePrefab;
    [SerializeField] private Diamond _diamondPrefab;
    [SerializeField] private Circle _circlePrefab;

    [SerializeField] private Transform _camera;
    void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                tile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                tile.Init(isOffset);

                if (x == 0 && y == 0 || x == 1 && y == 0)
                {
                    var triangle = Instantiate(_trianglePrefab, new Vector3(x, y, -1), Quaternion.identity);
                }
                
                if (x == 2 && y == 0 || x == 3 && y == 0)
                {
                    var circle = Instantiate(_circlePrefab, new Vector3(x, y, -1), Quaternion.identity);
                }
                
                if (x == 4 && y == 0 || x == 5 && y == 0)
                {
                    var diamond = Instantiate(_diamondPrefab, new Vector3(x, y, -1), _diamondPrefab.transform.rotation);
                    
                }
                

            }
        }

        _camera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
