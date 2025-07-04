using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
   [SerializeField] private int _width;
   [SerializeField] private int _height;
   [SerializeField] private GameObject nodePrefab;
    [SerializeField] private SpriteRenderer boardPrefab;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject node = Instantiate(nodePrefab, pos, Quaternion.identity);
            }
        }

        Vector2 gridCenter = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);
        var board = Instantiate(boardPrefab, gridCenter, Quaternion.identity);
        board.size = new Vector2(_width, _height);

        Camera.main.transform.position = new Vector3(gridCenter.x, gridCenter.y, -10f);
    }
}
