#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridService))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var gridManager = (GridService)target;
        EditorGUILayout.Space(4);
        using (new EditorGUILayout.VerticalScope())
        {
            // Pool Ready
            if (GUILayout.Button("PreWarm Pool"))
            {
                gridManager.PrewarmPool(gridManager.prewarmCount);
                MarkSceneDirty();
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            // Build
            if (GUILayout.Button("Build Grid"))
            {
                gridManager.GenerateGrid();                        
                MarkSceneDirty();
            }

            // Clear
            if (GUILayout.Button("Clear All"))
            {
                gridManager.ClearGrid();                        
                MarkSceneDirty();
            }
        }
    }

    private void MarkSceneDirty()
    {
        if (!Application.isPlaying)
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
}
#endif
