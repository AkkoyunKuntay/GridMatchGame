using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField, Min(1)] int width = 5;
    [SerializeField, Min(1)] int height = 5;

    [Header("Pool")]
    [SerializeField, Range(1, 100)] int prewarmCount = 15;
    readonly List<GameObject> pool = new();

    [Header("Grid References")]
    [SerializeField] Transform nodeContainer;
    [SerializeField] GameObject nodePrefab;
    [SerializeField] SpriteRenderer boardPrefab;
    SpriteRenderer board;

    void Awake()
    {
        if (nodeContainer == null)
        {
            nodeContainer = new GameObject("NodeContainer").transform;
            nodeContainer.SetParent(transform, false);
        }

        
        pool.Clear();
        foreach (Transform child in nodeContainer)
        {
            child.gameObject.SetActive(false);   
            pool.Add(child.gameObject);
        }
    }

    public void PrewarmPool()
    {
        while (pool.Count < prewarmCount)
        {
            var go = Instantiate(nodePrefab, nodeContainer);
            go.SetActive(false);
            pool.Add(go);
        }

        if (pool.Count > prewarmCount)
        {
            foreach (var c in pool)
                if (c.activeSelf)
                {
                    Debug.LogWarning("Pool shrink blocked: grid is active. ClearGrid first.");
                    return;
                }

            for (int i = pool.Count - 1; i >= prewarmCount; i--)
            {
                if (Application.isPlaying) Destroy(pool[i]);
                else DestroyImmediate(pool[i]);
                pool.RemoveAt(i);
            }
        }
    }

    public void GenerateGrid()
    {
        int needed = width * height;

        if (pool.Count < needed)
        {
            Debug.LogWarning($"Pool too small (need {needed}, have {pool.Count}). " +
                             "Instantiating and pooling missing nodes.");
            while (pool.Count < needed)
            {
                var go = Instantiate(nodePrefab, nodeContainer);
                go.SetActive(false);
                pool.Add(go);
            }
        }

        ClearGrid(false);

        int index = 0;
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++, index++)
            {
                var cell = pool[index];
                cell.transform.localPosition = new Vector3(x, y);
                cell.SetActive(true);
            }
        for (; index < pool.Count; index++)        
            pool[index].SetActive(false);

        Vector2 center = new(width * .5f - .5f, height * .5f - .5f);

        if (board == null)
            board = Instantiate(boardPrefab, transform);
        board.size = new Vector2(width, height);
        board.transform.position = center;
        board.gameObject.SetActive(true);

        var cam = Camera.main;
        if (cam) cam.transform.position = new Vector3(center.x, center.y, cam.transform.position.z);
    }

    public void ClearGrid(bool hideBoard = true)
    {
        if (pool.Count <= 0) return;
        foreach (var c in pool) c.SetActive(false);
        if (hideBoard && board) board.gameObject.SetActive(false);
    }

}
