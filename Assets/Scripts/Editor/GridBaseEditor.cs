using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridBase))]
public class GridBaseEditor : Editor
{
    private float _maxDisplayWidth => Mathf.Clamp(EditorGUIUtility.currentViewWidth - (2 * EditorGUIUtility.singleLineHeight), 0, float.MaxValue);

    private GridTileData _selectedTile;

    private readonly Dictionary<TileType, Color> _tileColor = new()
    {
        {TileType.Selected,Color.yellow},
        {TileType.Idle, Color.white},
        {TileType.AllySpawn, Color.cyan},
        {TileType.EnemySpawn, Color.red}
    };

    private const float WINDOW_PADDING = 10;
    private const float TILE_SIZE = 40;
    private const float TILE_SPACING = 5;
    private const int MIN_TILE_COUNT = 3;
    private const int MAX_TILE_COUNT = 7;

    private enum TileType { Idle = 0, Selected = 1, AllySpawn = 2, EnemySpawn = 3 }

    #region Editor

    public override void OnInspectorGUI()
    {
        GridBase gridObject = (GridBase)target;
        Validate(gridObject);

        DrawDimensionsControls(gridObject);

        DrawGridDisplayArea(gridObject);

        DrawTileDetails(gridObject, _selectedTile);

        // Save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(gridObject);
        }
    }

    #endregion

    #region Draw Helpers

    private void DrawDimensionsControls(GridBase gridBase)
    {
        GUILayout.Space(WINDOW_PADDING);

        EditorExtensions.DrawHeader("Dimensions");

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(_maxDisplayWidth));
        gridBase.Height = EditorGUILayout.IntSlider("Height", gridBase.Height, MIN_TILE_COUNT, MAX_TILE_COUNT);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(_maxDisplayWidth));
        gridBase.Width = EditorGUILayout.IntSlider("Width", gridBase.Width, MIN_TILE_COUNT, MAX_TILE_COUNT);
        EditorGUILayout.EndHorizontal();

    }

    private void DrawGridDisplayArea(GridBase gridBase)
    {
        GUILayout.Space(WINDOW_PADDING);

        EditorExtensions.DrawHeader("Grid Shape");

        // Get display area dimensions
        float width = (TILE_SIZE * gridBase.Width) + (TILE_SPACING * (gridBase.Width - 1)) + (WINDOW_PADDING * 2);
        float height = (TILE_SIZE * gridBase.Height) + (TILE_SPACING * (gridBase.Height - 1)) + (WINDOW_PADDING * 2);

        // Draw main rectangle
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(_maxDisplayWidth));

        Rect rawDisplayArea = EditorGUILayout.GetControlRect(false, height);
        Rect displayArea = new(rawDisplayArea.x + ((rawDisplayArea.width - width) * 0.5f), rawDisplayArea.y, width, height);
        EditorGUI.DrawRect(displayArea, Color.gray);
        DrawTiles(gridBase, displayArea);

        EditorGUILayout.EndHorizontal();
    }

    private void DrawTiles(GridBase gridBase, Rect displayArea)
    {
        foreach (var tile in gridBase.Tiles)
        {
            DrawTile(gridBase, tile.Value, displayArea);
        }
    }

    private void DrawTile(GridBase gridBase, GridTileData tile, Rect displayArea)
    {
        // Calculate the position based on tile coordinates
        float x = displayArea.x + (Mathf.Abs(tile.X) * (TILE_SIZE + TILE_SPACING)) + TILE_SPACING - TILE_SIZE;
        float y = WINDOW_PADDING + displayArea.y + ((gridBase.Height - tile.Y) * (TILE_SIZE + TILE_SPACING));

        // Create the button rect
        Rect buttonRect = new(x, y, TILE_SIZE, TILE_SIZE);

        // Draw the button
        Color originalColor = GUI.color;
        GUI.color = GetTileColor(tile);
        if (GUI.Button(buttonRect, $"{Mathf.Abs(tile.X)}:{tile.Y}"))
        {
            _selectedTile = _selectedTile == tile ? null : tile; // Toggle selection
        }
        GUI.color = originalColor;

    }

    private void DrawTileDetails(GridBase gridBase, GridTileData tile)
    {
        GUILayout.Space(WINDOW_PADDING);

        EditorExtensions.DrawHeader("Tile Details");

        if (tile == null || gridBase[tile.Coordinates] == null || gridBase[tile.Coordinates] != tile)
        {

            EditorGUILayout.LabelField("No tile selected");
            return;
        }

        // Display Basic Data
        EditorGUILayout.LabelField("Coordinates", $"X: {tile.X}, Y: {tile.Y}");
        tile.Tags = (GridTileData.TileTag)EditorGUILayout.EnumFlagsField("Tags", tile.Tags);
    }

    #endregion

    #region Other

    private bool Validate(GridBase gridObject)
    {
        if (gridObject.Tiles == null)
        {
            // Debug.LogWarning("Tile list is null! Creating new Tiles from GridBase!");
            gridObject.Initialize();
            return false;
        }

        if (gridObject.Tiles.Count != gridObject.Width * gridObject.Height)
        {
            // Debug.Log("Tile list's length is mismached! Creating new Tiles from GridBase!");
            gridObject.Initialize();
            return false;
        }

        if (gridObject[gridObject.Width, gridObject.Height] == null)
        {
            // Debug.Log("Tile list does not contain top-right corner tile within (probably due to the change in orientation)! Creating new Tiles from GridBase!");
            gridObject.Initialize();
            return false;
        }

        return true;
    }

    private Color GetTileColor(GridTileData tile)
    {
        if (tile == _selectedTile) return _tileColor[TileType.Selected];
        if (tile.HasTag(GridTileData.TileTag.AllySpawn)) return _tileColor[TileType.AllySpawn];
        if (tile.HasTag(GridTileData.TileTag.EnemySpawn)) return _tileColor[TileType.EnemySpawn];

        return _tileColor[TileType.Idle];
    }

    #endregion

}
