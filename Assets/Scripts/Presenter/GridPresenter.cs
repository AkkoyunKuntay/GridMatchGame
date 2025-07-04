using System.Collections.Generic;
using UnityEngine;

public class GridPresenter : MonoBehaviour
{
    [SerializeField] ScoreService scoreService; // TODO: will be injected later.

    NodeView[,] grid;
    int w, h;

    public void Initialize(NodeView[,] g)
    {
        grid = g;
        w = g.GetLength(0);
        h = g.GetLength(1);

        foreach (var n in grid) n.SetPresenter(this);
    }

    public void OnNodeClicked(NodeView node)
    {
        if (node.IsMarked) return;

        node.SetMarked(true);

        var stack = new Stack<NodeView>();
        var visited = new HashSet<NodeView>();
        stack.Push(node);

        while (stack.Count > 0)
        {
            var cur = stack.Pop();
            if (!visited.Add(cur)) continue;

            foreach (var nb in Neighbors(cur))
                if (nb.IsMarked && !visited.Contains(nb))
                    stack.Push(nb);
        }

        if (visited.Count >= 3)
        {
            foreach (var n in visited) n.SetMarked(false);
            Debug.Log($"Match cleared: {visited.Count} nodes");
            scoreService.RegisterMatch();
        }
    }

    IEnumerable<NodeView> Neighbors(NodeView n)
    {
        int x = n.X, y = n.Y;
        if (x > 0) yield return grid[x - 1, y];
        if (x < w - 1) yield return grid[x + 1, y];
        if (y > 0) yield return grid[x, y - 1];
        if (y < h - 1) yield return grid[x, y + 1];
    }
}
