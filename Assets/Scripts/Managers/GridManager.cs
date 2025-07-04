// Assets/Scripts/Grid2D/GridManager.cs
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [Min(1)] public int width = 5;
    [Min(1)] public int height = 5;

    [Header("Pool Settings")]
    public int prewarmCount = 25;
    readonly List<GameObject> pool = new();

    [Header("References")]
    [SerializeField] GameObject nodePrefab;
    [SerializeField] SpriteRenderer boardPrefab;
    [SerializeField] Transform nodeContainer;

    [Header("Scene References")]
  
    [SerializeField] SpriteRenderer board;

    void Awake()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        if (nodeContainer != null)
        {
            for (int i = nodeContainer.childCount - 1; i >= 0; i--)
                Destroy(nodeContainer.GetChild(i).gameObject);
        }

        if (board != null)
        {
            Destroy(board.gameObject);
            board = null;
        }

        pool.Clear();

        if (nodeContainer != null) Destroy(nodeContainer.gameObject);
        nodeContainer = new GameObject("NodeContainer").transform;
        nodeContainer.SetParent(transform, false);

        while (pool.Count < prewarmCount)
        {
            var g = Instantiate(nodePrefab, nodeContainer);
            g.SetActive(false);
            pool.Add(g);
        }

        GenerateGrid();
        return;
    }
    public void PrewarmPool(int size)
    {
        prewarmCount = Mathf.Max(0, size);
        while (pool.Count < prewarmCount)
        {
            var g = Instantiate(nodePrefab, nodeContainer);
            g.SetActive(false);
            pool.Add(g);
        }
        foreach (var v in pool)
            if (v.activeSelf) { Debug.LogWarning("Call 'ClearGrid' first."); return; }
        while (pool.Count > prewarmCount)
        {
            int i = pool.Count - 1;
            DestroyImmediate(pool[i]);
            pool.RemoveAt(i);
        }
    }
    public void GenerateGrid()
    {
        int need = width * height;
        while (pool.Count < need)
        {
            var g = Instantiate(nodePrefab, nodeContainer);
            g.SetActive(false);
            pool.Add(g);
        }
        foreach (var g in pool) g.SetActive(false);

        var grid = new NodeView[width, height];
        var presenter = GetComponent<GridPresenter>() ?? gameObject.AddComponent<GridPresenter>();

        int p = 0;
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++, p++)
            {
                var root = pool[p];
                root.transform.localPosition = new Vector3(x, y);
                root.SetActive(true);

                var view = root.GetComponentInChildren<NodeView>(true);
                view.SetMarked(false);
                view.SetPresenter(presenter);
                grid[x, y] = view;
            }
        for (; p < pool.Count; p++)
            pool[p].SetActive(false);

        presenter.Initialize(grid);

        Vector2 center = new(width * .5f - .5f, height * .5f - .5f);
        if (board == null) board = Instantiate(boardPrefab, transform);
        board.size = new Vector2(width, height);
        board.transform.position = center;
        board.gameObject.SetActive(true);

        if (Application.isPlaying && Camera.main != null)
        {
            var cam = Camera.main.transform;
            cam.position = new Vector3(center.x, center.y, cam.position.z);
        }
    }
    public void ClearGrid()
    {
        foreach (var g in pool) g.SetActive(false);
        if (board) board.gameObject.SetActive(false);
    }
}
