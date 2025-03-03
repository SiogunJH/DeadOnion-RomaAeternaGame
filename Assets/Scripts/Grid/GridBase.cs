using System.Collections.Generic;
using UnityEngine;
using VInspector;

[CreateAssetMenu(fileName = "NewGridBase", menuName = "Grid/Grid Base")]
public class GridBase : ScriptableObject
{
    [field: Header("Dimensions")]
    [field: SerializeField, Range(3, 7)] public int Width { get; set; } = 5;
    [field: SerializeField, Range(3, 7)] public int Height { get; set; } = 5;

    public SerializedDictionary<Vector2, GridTileData> Tiles = new();

    #region Indexer

    public GridTileData this[int x, int y]
    {
        get => this[new(x, y)];
        private set => this[new(x, y)] = value;
    }
    public GridTileData this[Vector2Int coordinates]
    {
        get => Tiles.TryGetValue(coordinates, out GridTileData tile) ? tile : null;
        private set => Tiles.Add(coordinates, value);
    }

    #endregion

    #region Initialization

    public void Initialize()
    {
        SerializedDictionary<Vector2, GridTileData> newTiles = new();

        for (int y = 1; y <= Height; y++)
        {
            for (int x = 1; x <= Width; x++)
            {
                Vector2Int index = new(x, y);
                newTiles[index] = GenerateTile(index);
            }
        }

        Tiles = newTiles;
    }

    private GridTileData GenerateTile(Vector2Int index)
    {
        GridTileData newTile = new(index.x, index.y);

        // Check if tile already exists
        GridTileData existingTile = this[index];
        if (existingTile != null)
        {
            // Migrate old data to new tile

            // NO DATA TO MIGRATE, AS OF NOW
        }

        // Add a new tile to Tiles pool
        return newTile;
    }

    #endregion
}
