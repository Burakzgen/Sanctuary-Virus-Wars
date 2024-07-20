using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingPlacementGenerator))]
public class AdvancedBuildingPlacementEditor : Editor
{
    private BuildingPlacementGenerator generator;
    private bool editingPath = false;

    void OnEnable()
    {
        generator = (BuildingPlacementGenerator)target;
    }

    void OnSceneGUI()
    {
        if (editingPath)
        {
            for (int i = 0; i < generator.roadPoints.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPoint = Handles.PositionHandle(generator.roadPoints[i], Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(generator, "Move Road Point");
                    generator.roadPoints[i] = newPoint;
                    EditorUtility.SetDirty(generator);
                }
            }

            if (generator.roadPoints.Count > 1)
            {
                Handles.color = Color.red;
                for (int i = 0; i < generator.roadPoints.Count - 1; i++)
                {
                    Handles.DrawLine(generator.roadPoints[i], generator.roadPoints[i + 1]);
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button(editingPath ? "Stop Editing Path" : "Edit Path"))
        {
            editingPath = !editingPath;
            SceneView.RepaintAll();
        }

        if (editingPath)
        {
            if (GUILayout.Button("Add Road Point"))
            {
                Undo.RecordObject(generator, "Add Road Point");
                generator.roadPoints.Add(Vector3.zero);
                EditorUtility.SetDirty(generator);
            }

            if (generator.roadPoints.Count > 0 && GUILayout.Button("Remove Last Road Point"))
            {
                Undo.RecordObject(generator, "Remove Road Point");
                generator.roadPoints.RemoveAt(generator.roadPoints.Count - 1);
                EditorUtility.SetDirty(generator);
            }
        }
    }
}
