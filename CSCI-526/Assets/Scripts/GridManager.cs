using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


/* GridManager
 * Create and color the grid
 * Src: https://www.youtube.com/watch?v=kkAjpQAM-jE
 */
public class GridManager : MonoBehaviour
{
    private static GridManager instance;
    public static GridManager Instance { get { return instance; } }

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile ti ;
    [SerializeField] private Transform cam;


    public Tile[,] board;
    string selectedElement;

    // Interactive modes
    public enum MODE { PLUS, MINUS, X }
    public MODE currentMode = MODE.PLUS;
    // Create 5x5 grid
    private void MakeGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var _ti = Instantiate(ti, new Vector3(x, y), Quaternion.identity);
                _ti.name = $"Tile ({x}, {y})";
                board[x, y] = _ti;
                board[x, y].pos.x = x;
                board[x, y].pos.y = y;
                board[x, y].aspect("fire");
            }
        }
        cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public void SetElement(string ele)
    {
        selectedElement = ele;
    }
    // On button click set mode
    public void SetMode(int selection)      // 0 = PLUS, 1 = MINUS, 2 = X
    {
        switch(selection)
        {
            case 0:
                currentMode = MODE.PLUS;
                break;
            case 1:
                currentMode = MODE.MINUS;
                break;
            case 2:
                currentMode = MODE.X;
                break;
        }
    }

    public void ChangeElement()
    {
        foreach (Tile t in board)
        {
            if(t.mouseNear || t.mouseHere)      // If highlighted (selected)
            {
                t.aspect(selectedElement);
            }
        }
    }

    // Highlight tiles based on pattern
    public void MousedOver()
    {

        foreach (Tile t in board)
        {
            if (t.mouseHere)
            {
                // Set mouseNear for adjacent AFFECTED tiles
                switch (currentMode)
                {
                    case MODE.PLUS:     // (-1, 0), (1, 0), (0, -1), (0, 1)
                        if (t.pos.x - 1 >= 0)
                        {
                            board[t.pos.x - 1, t.pos.y].mouseNear = true;
                        }
                        if (t.pos.x + 1 < _width)
                        {
                            board[t.pos.x + 1, t.pos.y].mouseNear = true;
                        }
                        if (t.pos.y - 1 >= 0)
                        {
                            board[t.pos.x, t.pos.y - 1].mouseNear = true;
                        }
                        if (t.pos.y + 1 < _height)
                        {
                            board[t.pos.x, t.pos.y + 1].mouseNear = true;
                        }
                        break;
                    case MODE.MINUS:    // (-1, 0), (1, 0)
                        if (t.pos.x - 1 >= 0)
                        {
                            board[t.pos.x - 1, t.pos.y].mouseNear = true;
                        }
                        if (t.pos.x + 1 < _width)
                        {
                            board[t.pos.x + 1, t.pos.y].mouseNear = true;
                        }
                        break;
                    case MODE.X:        // (-1, -1), (-1, 1), (1, -1), (1, 1)
                        if (t.pos.x - 1 >= 0 && t.pos.y - 1 >= 0)
                        {
                            board[t.pos.x - 1, t.pos.y - 1].mouseNear = true;
                        }
                        if (t.pos.x - 1 >= 0 && t.pos.y + 1 < _height)
                        {
                            board[t.pos.x - 1, t.pos.y + 1].mouseNear = true;
                        }
                        if (t.pos.x + 1 < _width && t.pos.y - 1 >= 0)
                        {
                            board[t.pos.x + 1, t.pos.y - 1].mouseNear = true;
                        }
                        if (t.pos.x + 1 < _width && t.pos.y + 1 < _height)
                        {
                            board[t.pos.x + 1, t.pos.y + 1].mouseNear = true;
                        }
                        break;
                    default:        // Default Mode?
                        break;
                }
            }
        }
    }
    public void MouseExit()
    {
        // Disable "near" highlight on all first
        foreach (Tile t in board)
        {
            t.mouseHere = false;
            t.mouseNear = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        board = new Tile[_width, _height];
        selectedElement = "water";
        MakeGrid();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
