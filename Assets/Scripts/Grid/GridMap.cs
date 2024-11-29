using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGridMap", menuName = "Grid/Grid Map")]
public class GridMap : ScriptableObject
{
    [Header("Dimensions")]
    [SerializeField, Range(2, 20), Tooltip("Width of the grid")] private int _gridWidth = 5;
    public int GridWidth => _gridWidth;

    [SerializeField, Range(1, 10), Tooltip("Height of the grid")] private int _gridHeight = 5;
    public int GridHeight => _gridHeight;

    [HideInInspector] public List<GridTile> Tiles;

    public GridTile GetTile(int x, int y) => GetTile(new(x, y));
    public GridTile GetTile(Vector2 coordinates)
    {
        return Tiles.FirstOrDefault(tile => tile.Coordinates == coordinates);
    }

    public void InitializeGrid()
    {
        var tilesOld = Tiles != null ? Tiles : new();

        Tiles = new();

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Vector2 index = new(x, y);

                if (tilesOld.Where(tile => tile.Coordinates == index).Any())
                {
                    Tiles.Add(tilesOld.FirstOrDefault(tile => tile.Coordinates == index));
                }
                else
                {
                    Tiles.Add(new(x, y));
                }
            }
        }
    }
}

[CustomEditor(typeof(GridMap))]
public class GridMapEditor : Editor
{
    private const float TILE_SIZE = 40f;

    private Color _tileEnabledColor = Color.white;
    private Color _tileDisabledColor = Color.gray;

    public override void OnInspectorGUI()
    {
        GridMap gridObject = (GridMap)target;

        // Draw default Inspector for other fields
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Visualization", EditorStyles.boldLabel);

        // Ensure the grid is initialized
        if (gridObject.Tiles == null || gridObject.Tiles.Count != gridObject.GridWidth * gridObject.GridHeight)
        {
            gridObject.InitializeGrid();
        }

        // Draw the grid
        for (int y = gridObject.GridHeight - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < gridObject.GridWidth; x++)
            {
                // Tile visualization
                Color previousColor = GUI.color;
                GUI.color = gridObject.GetTile(x, y).IsEnabled ? _tileEnabledColor : _tileDisabledColor;

                // Draw button for the tile
                if (GUILayout.Button($"{x}:{y}", GUILayout.Width(TILE_SIZE), GUILayout.Height(TILE_SIZE)))
                {
                    // Toggle on/off
                    gridObject.GetTile(x, y).IsEnabled = !gridObject.GetTile(x, y).IsEnabled;
                }

                GUI.color = previousColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        // Save changes to ScriptableObject
        if (GUI.changed)
        {
            EditorUtility.SetDirty(gridObject);
        }
    }
}
