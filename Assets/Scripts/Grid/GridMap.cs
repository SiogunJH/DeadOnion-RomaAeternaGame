using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGridMap", menuName = "Grid/Grid Map")]
public class GridMap : ScriptableObject
{
    public int Width => _gridBase.Width;
    public int Height => _gridBase.Height;
    public Dictionary<Vector2, GridTileData> Tiles => _gridBase.Tiles;

    public List<Character> Enemies;

    [SerializeField] private GridBase _gridBase;

    #region Indexer

    public GridTileData this[int x, int y] => this[new(x, y)];
    public GridTileData this[Vector2Int coordinates] => _gridBase[coordinates];

    #endregion

    #region Pathfinding

    public bool FindPathBetween(GridEntity traveler, GridTileData start, GridTileData end, out IEnumerable<GridTileData> path)
    {
        // Validate
        foreach (var tile in new List<GridTileData>() { start, end })
        {
            if (Tiles.ContainsKey(tile.Coordinates)) continue;

            Debug.LogWarning($"Tile ({tile.X},{tile.Y}) is invalid!", this);
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
                if ((!neighbour.IsOccupied || !traveler.OccupiesTheWholeTile) && !cameFrom.ContainsKey(neighbour)) // Ensure the neighbor is valid and not visited
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
