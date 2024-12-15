using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewGridMap", menuName = "Grid/Grid Map")]
public class GridMap : ScriptableObject
{
    [Header("Dimensions")]
    [SerializeField, Range(2, 20), Tooltip("Width of the grid")] private int _gridWidth = 5;
    public int GridWidth => _gridWidth;

    [SerializeField, Range(1, 10), Tooltip("Height of the grid")] private int _gridHeight = 5;
    public int GridHeight => _gridHeight;

    public Dictionary<Vector2, GameObject> InitialOccupants { get; private set; } = new();

    [HideInInspector] public List<GridTile> Tiles;

    public GridTile GetTile(int x, int y) => GetTile(new(x, y));
    public GridTile GetTile(Vector2 coordinates)
    {
        return Tiles.FirstOrDefault(tile => tile.Coordinates == coordinates);
    }

    public void InitializeGrid()
    {
        if (Tiles == null) Tiles = new();

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Vector2 index = new(x, y);

                // Check if tile already exists
                if (Tiles.Any(tile => tile.Coordinates == index)) continue;

                // Add a new tile if it doesn't exist
                Tiles.Add(new GridTile(x, y));
            }
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(GridMap))]
public class GridMapEditor : Editor
{
    private const float TILE_SIZE = 40f;
    private Dictionary<string, Color> _tileColor = new()
    {
        {"Selected",Color.yellow},
        {"Empty", Color.white},
        {"Disabled", Color.gray},
        {"Occupied", Color.cyan},
        {"Invalid Occupants", Color.red},
        {"Missing", Color.black}
    };

    private GridTile _selectedTile; // Currently selected tile

    private Color GetTileColor(GridTile tile)
    {
        if (tile == _selectedTile) return _tileColor["Selected"];
        if (!tile.IsEnabled) return _tileColor["Disabled"];
        if (!tile.IsOccupied) return _tileColor["Empty"];
        if (tile.Occupants.Where(occupant => occupant == null).Any()) return _tileColor["Invalid Occupants"];
        return _tileColor["Occupied"];
    }

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
                GridTile currentTile = gridObject.GetTile(x, y);
                Color previousColor = GUI.color;
                GUI.color = GetTileColor(currentTile);

                // Draw button for the tile
                if (GUILayout.Button($"{x}:{y}", GUILayout.Width(TILE_SIZE), GUILayout.Height(TILE_SIZE)))
                {
                    if (_selectedTile == currentTile) _selectedTile = null; // Unselect tile if clicked again
                    else _selectedTile = currentTile; // Select this tile
                }

                GUI.color = previousColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        // Display selected tile information
        if (_selectedTile != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Selected Tile Info", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Coordinates", $"({_selectedTile.Coordinates.x}, {_selectedTile.Coordinates.y})");

            // Toggle for enabling/disabling the tile
            _selectedTile.IsEnabled = EditorGUILayout.Toggle("Is Enabled", _selectedTile.IsEnabled);

            // Disallow further edits if thile is not enabled
            if (_selectedTile.IsEnabled)
            {
                EditorGUILayout.Space();

                // Occupants field with drag-and-drop support for multiple entities
                List<GridEntity> occupants = _selectedTile.Occupants;
                if (occupants == null) occupants = new();

                for (int i = 0; i < occupants.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    string occupantName;
                    if (occupants[i] != null) occupantName = occupants[i].UserFriendlyName;
                    else occupantName = $"Occupant {i + 1}";

                    occupants[i] = (GridEntity)EditorGUILayout.ObjectField(occupantName, occupants[i], typeof(GridEntity), false);

                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        occupants.RemoveAt(i);
                        i--; // Adjust index to reflect removed item
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add Occupant"))
                {
                    occupants.Add(null);
                }

                _selectedTile.SetOccupants(occupants);
            }
        }

        // Save changes to ScriptableObject
        if (GUI.changed)
        {
            EditorUtility.SetDirty(gridObject);
        }
    }
}


#endif