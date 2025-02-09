using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridMap))]
public class GridMapEditor : Editor
{
    // Private Field
    private GridTileData _selectedTile;

    // Private Readonly
    private readonly Color _gridDisplayLeftAreaColor = new(0.175f, 0.175f, 0.22f, 1f);
    private readonly Color _gridDisplayRightAreaColor = new(0.22f, 0.175f, 0.175f, 1f);

    private readonly Dictionary<TileType, Color> _tileColor = new()
    {
        {TileType.Selected,Color.yellow},
        {TileType.Empty, Color.white},
        {TileType.Disabled, Color.gray},
        {TileType.Occupied, Color.cyan},
        {TileType.Invalid, Color.red},
        {TileType.Missing, Color.black}
    };

    // Private Const
    private const float WINDOW_PADDING = 10;
    private const float DIVIDER_WIDTH = 2;

    private const float TILE_SIZE = 40;
    private const float TILE_SPACING = 5;
    private const int TILE_MAX_VERTICAL_COUNT = 5;
    private const int TILE_MAX_HORIZONTAL_COUNT = 5;

    // Enums
    private enum TileType { Selected, Empty, Disabled, Occupied, Invalid, Missing }

    public override void OnInspectorGUI()
    {
        GridMap gridObject = (GridMap)target;
        ValidateGrid(gridObject);

        GUILayout.Space(WINDOW_PADDING);

        DrawHeader("Settings");
        DrawGridSettings(gridObject);

        GUILayout.Space(WINDOW_PADDING);

        DrawHeader("Visualization");
        DrawGridDisplayArea(gridObject);

        GUILayout.Space(WINDOW_PADDING);

        DrawHeader("Tile Details");
        DrawTileDetails(_selectedTile);

        GUILayout.Space(WINDOW_PADDING);

        // Save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(gridObject);
        }
    }

    #region Functional Helpers

    private Color GetTileColor(GridTileData tile)
    {
        if (tile == _selectedTile) return _tileColor[TileType.Selected];
        if (!tile.IsEnabled) return _tileColor[TileType.Disabled];
        if (!tile.Occupants.Any()) return _tileColor[TileType.Empty];
        if (tile.Occupants.Where(occupant => occupant == null).Any()) return _tileColor[TileType.Invalid];
        return _tileColor[TileType.Occupied];
    }

    private void ValidateGrid(GridMap gridObject)
    {
        if (gridObject.Tiles == null || gridObject.Tiles.Count != (gridObject.WidthL + gridObject.WidthR) * gridObject.Height)
        {
            gridObject.InitializeGrid();
        }
    }

    #endregion

    #region Editor Helpers

    private void DrawHeader(string text)
    {
        GUIStyle headerStyle = new(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 20,
        };
        GUILayout.Label(text, headerStyle);
    }

    private void DrawGridDisplayArea(GridMap gridObject)
    {
        float maxWidth = Mathf.Clamp(EditorGUIUtility.currentViewWidth - (2 * EditorGUIUtility.singleLineHeight), 0, float.MaxValue);
        float height = (TILE_SIZE * gridObject.Height) + (TILE_SPACING * (gridObject.Height - 1)) + (WINDOW_PADDING * 2);

        // Draw the rectangles
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(maxWidth));

        Rect rectL = EditorGUILayout.GetControlRect(false, height);
        EditorGUI.DrawRect(rectL, _gridDisplayLeftAreaColor);
        DrawTiles(gridObject, rectL, false);

        GUILayout.Width(DIVIDER_WIDTH);

        Rect rectR = EditorGUILayout.GetControlRect(false, height);
        EditorGUI.DrawRect(rectR, _gridDisplayRightAreaColor);
        DrawTiles(gridObject, rectR, true);

        EditorGUILayout.EndHorizontal();
    }

    private void DrawTiles(GridMap gridObject, Rect rect, bool isRight)
    {
        var tilesToDraw = gridObject.Tiles
            .Where(isRight ? tile => tile.X < 0 && Mathf.Abs(tile.X) <= gridObject.WidthR : tile => tile.X > 0 && Mathf.Abs(tile.X) <= gridObject.WidthL)
            .Where(tile => tile.Y <= gridObject.Height && tile.Y > 0);

        foreach (var tile in tilesToDraw)
        {
            DrawTile(tile, rect, isRight);
        }
    }

    private void DrawTile(GridTileData tile, Rect rect, bool alignToLeft)
    {
        // Define the constant tile size
        float buttonWidth = TILE_SIZE;
        float buttonHeight = TILE_SIZE;

        // Calculate the position based on tile coordinates
        float x;
        if (alignToLeft)
        {
            x = rect.x + (Mathf.Abs(tile.X) * (TILE_SIZE + TILE_SPACING)) + TILE_SPACING - buttonWidth;
        }
        else
        {
            x = rect.x + rect.width - (Mathf.Abs(tile.X) * (TILE_SIZE + TILE_SPACING)) - TILE_SPACING;
        }

        // Calculate the vertical position
        float y = WINDOW_PADDING + rect.y + ((tile.Y - 1) * (TILE_SIZE + TILE_SPACING));

        // Prevent horizontal overflow
        if (x + buttonWidth > rect.x + rect.width) // Right
        {
            x = rect.x + rect.width - buttonWidth;
        }
        else if (x < rect.x) // Left
        {
            x = rect.x;
        }

        // Prevent vertical overflow
        if (y + buttonHeight > rect.y + rect.height) // Bottom
        {
            y = rect.y + rect.height - buttonHeight;
        }
        else if (y < rect.y) // Top
        {
            y = rect.y;
        }

        // Create the button rect
        Rect buttonRect = new(x, y, buttonWidth, buttonHeight);

        // Draw the button
        Color originalColor = GUI.color;
        GUI.color = GetTileColor(tile);
        if (GUI.Button(buttonRect, $"{Mathf.Abs(tile.X)}:{tile.Y}"))
        {
            _selectedTile = _selectedTile == tile ? null : tile; // Toggle selection
        }
        GUI.color = originalColor;
    }

    private void DrawGridSettings(GridMap gridObject)
    {
        gridObject.Height = EditorGUILayout.IntSlider("Grid Height", gridObject.Height, 1, TILE_MAX_VERTICAL_COUNT);
        gridObject.WidthL = EditorGUILayout.IntSlider("Grid Width (Left)", gridObject.WidthL, 1, TILE_MAX_HORIZONTAL_COUNT);
        gridObject.WidthR = EditorGUILayout.IntSlider("Grid Width (Right)", gridObject.WidthR, 1, TILE_MAX_HORIZONTAL_COUNT);
    }

    private void DrawTileDetails(GridTileData tile)
    {
        // Validate
        if (tile == null || !((GridMap)target).Tiles.Contains(tile))
        {
            EditorGUILayout.LabelField("No tile selected");
            return;
        }

        // Display Basic Data
        EditorGUILayout.LabelField("Coordinates", $"X: {tile.X}, Y: {tile.Y}");
        EditorGUILayout.LabelField("Type", tile.X > 0 ? "Ally Tile" : "Enemy Tile");
        tile.IsEnabled = EditorGUILayout.Toggle("Enabled", tile.IsEnabled);

        if (!tile.IsEnabled) return;

        EditorGUILayout.LabelField("Total Occupants", $"{tile.Occupants.Count}");

        GUILayout.Space(WINDOW_PADDING);

        // Display Advanced Data
        DrawOccupantsList(tile);
    }

    private void DrawOccupantsList(GridTileData tile)
    {
        // Occupants field with drag-and-drop support for multiple entities
        List<GridEntity> occupants = tile.Occupants;

        for (int i = 0; i < occupants.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // Validate occupant and get its name
            bool occupantExists = occupants[i] != null;
            string occupantName = occupantExists ? occupants[i].UserFriendlyName : "Missing occupant reference";

            // Mark red, if an occupant is missing
            Color previousColor = GUI.color;
            if (occupants[i] == null) GUI.color = Color.red;

            // Name label
            GUILayout.Label(occupantName, GUILayout.Width(250));

            // Revert color
            GUI.color = previousColor;

            // Create occupant field
            occupants[i] = (GridEntity)EditorGUILayout.ObjectField(occupants[i], typeof(GridEntity), false);

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

        tile.SetOccupants(occupants);
    }

    #endregion
}