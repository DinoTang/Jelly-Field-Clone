using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JellyLevelSO))]
public class JellyLevelSOEditor : Editor
{
    private int width;
    private int height;

    private const float CellSize = 30f;
    private const float LabelWidth = 25f;

    private void OnEnable()
    {
        JellyLevelSO board = (JellyLevelSO)target;

        width = board.Width;
        height = board.Height;
    }

    public override void OnInspectorGUI()
    {
        JellyLevelSO board = (JellyLevelSO)target;

        // =====================================================
        // SIZE
        // =====================================================

        EditorGUILayout.LabelField(
            "Board Size",
            EditorStyles.boldLabel
        );

        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);

        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);

        if (GUILayout.Button("Apply Size"))
        {
            Undo.RecordObject(board, "Resize Board");

            board.SetSize(width, height);

            EditorUtility.SetDirty(board);
            AssetDatabase.SaveAssets();

            // Đồng bộ lại giá trị sau khi Apply
            width = board.Width;
            height = board.Height;
        }

        EditorGUILayout.Space(10);

        // =====================================================
        // BOARD LAYOUT
        // =====================================================

        EditorGUILayout.LabelField(
            "Board Layout",
            EditorStyles.boldLabel
        );

        bool[] cells = board.GetValidCells();

        if (cells == null ||
            cells.Length != board.Width * board.Height)
        {
            EditorGUILayout.HelpBox(
                "The board layout has not been initialized. Please click Apply Size.",
                MessageType.Warning
            );
        }
        else
        {
            // =================================================
            // X COORDINATES
            // =================================================

            EditorGUILayout.BeginHorizontal();

            // Chừa chỗ cho Y
            GUILayout.Space(LabelWidth);

            for (int x = 0; x < board.Width; x++)
            {
                GUIStyle xLabelStyle = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleCenter
                };

                GUILayout.Label(
                    x.ToString(),
                    xLabelStyle,
                    GUILayout.Width(CellSize),
                    GUILayout.Height(20)
                );
            }

            EditorGUILayout.EndHorizontal();

            // =================================================
            // GRID + Y COORDINATES
            // =================================================

            for (int y = 0; y < board.Height; y++)
            {
                EditorGUILayout.BeginHorizontal();

                // Y coordinate
                GUIStyle yLabelStyle = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleCenter
                };

                GUILayout.Label(
                    y.ToString(),
                    yLabelStyle,
                    GUILayout.Width(LabelWidth),
                    GUILayout.Height(CellSize)
                );

                // Grid cells
                for (int x = 0; x < board.Width; x++)
                {
                    int index = y * board.Width + x;

                    GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
                    {
                        fontSize = 18,
                        alignment = TextAnchor.MiddleCenter
                    };

                    string symbol = cells[index]
                        ? "■"
                        : "□";

                    if (GUILayout.Button(
                        symbol,
                        buttonStyle,
                        GUILayout.Width(CellSize),
                        GUILayout.Height(CellSize)))
                    {
                        Undo.RecordObject(
                            board,
                            "Change Board Cell"
                        );

                        cells[index] = !cells[index];

                        EditorUtility.SetDirty(board);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        // =====================================================
        // JELLY CELLS
        // =====================================================

        EditorGUILayout.Space(15);

        SerializedProperty jellyCellsProperty =
            serializedObject.FindProperty("jellyCells");

        EditorGUILayout.PropertyField(
            jellyCellsProperty,
            true
        );

        serializedObject.ApplyModifiedProperties();
    }
}