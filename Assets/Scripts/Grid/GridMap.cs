using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using VInspector;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewGridMap", menuName = "Grid/Grid Map")]
public class GridMap : ScriptableObject
{
    [Header("Dimensions")]
    [SerializeField, Range(2, 20), Tooltip("Width of the grid")] private int _gridWidth = 5;
    public int Width => _gridWidth;

    [SerializeField, Range(1, 10), Tooltip("Height of the grid")] private int _gridHeight = 5;
    public int Height => _gridHeight;

    [SerializeField, HideInInspector] public List<GridTileData> Tiles = new();

    public GridTileData this[int x, int y] { get => this[new(x, y)]; }
    public GridTileData this[Vector2 coordinates] { get => Tiles.FirstOrDefault(tile => tile.Coordinates == coordinates); }

    #region Initialization

    public void InitializeGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2 index = new(x, y);

                // Check if tile already exists
                GridTileData existingTile = this[index];
                if (existingTile != null) continue;

                // Add a new tile if it doesn't exist
                GridTileData newTile = new(x, y);
                Tiles.Add(newTile);
            }
        }
    }

    public void OptimizeGrid()
    {
        // Remove unused GridTiles
        Tiles = Tiles.Where(tile => tile.X < Width && tile.Y < Height).ToList();
    }

    #endregion

    #region Pathfinding

    public bool FindPathBetween(GridEntity traveler, GridTileData start, GridTileData end, out IEnumerable<GridTileData> path)
    {
        // Validate
        foreach (var tile in new List<GridTileData>() { start, end })
        {
            if (tile.IsEnabled) continue;

            Debug.LogWarning($"Tile ({tile.X},{tile.Y}) needs to be enabled!", this);
            path = null;
            return false;
        }

        // Initialize data structures for BFS
        Queue<GridTileData> queue = new();
        Dictionary<GridTileData, GridTileData> cameFrom = new(); // Keeps track of the path

        queue.Enqueue(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            GridTileData current = queue.Dequeue();

            // Check if the end was reached
            if (current == end)
            {
                // Reconstruct the path
                List<GridTileData> shortestPath = new();
                for (GridTileData tile = end; tile != null; tile = cameFrom[tile])
                {
                    shortestPath.Add(tile);
                }
                shortestPath.Reverse();
                path = shortestPath;
                return true;
            }

            // Add neighbors to the queue
            foreach (GridTileData neighbour in current.Neighbours)
            {
                if (neighbour.IsEnabled && (!neighbour.IsOccupied || !traveler.OccupiesTheWholeTile) && !cameFrom.ContainsKey(neighbour)) // Ensure the neighbor is valid and not visited
                {
                    queue.Enqueue(neighbour);
                    cameFrom[neighbour] = current;
                }
            }
        }

        // No path found
        path = null;
        return false;
    }

    #endregion

}