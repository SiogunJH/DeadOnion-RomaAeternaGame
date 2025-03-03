using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using VInspector;
#endif

public class GridManager : MonoBehaviourSingleton<GridManager>
{
#if UNITY_EDITOR
    [Tab("Grid Manager/External References")]
#endif

    public GridMap Grid;
    [SerializeField] private Transform _gridContainer;

#if UNITY_EDITOR
    [Tab("Grid Manager/Internal References")]
#endif

    [SerializeField] private GameObject _tileVisualization;

#if UNITY_EDITOR
    [EndTab]
#endif

    private const float TILE_SPACING = 1.25f;

    #region MonoBehaviour

    private void Start()
    {
        // Validate
        Debug.Assert(Grid != null, "[Grid] is not assigned!", this);
        Debug.Assert(_gridContainer != null, "[Grid Container] is not assigned!", this);

        // Initialize Grid
        Grid = Instantiate(Grid);
    }

    #endregion

    #region Display 

#if UNITY_EDITOR
    [Button]
#endif
    private void DisplayGrid()
    {
        // Validate
        if (!Application.isPlaying)
        {
            Debug.LogWarning("GridMap cannot be displayed outside of play mode!");
            return;
        }

        // Clear
        _gridContainer.transform.RemoveChildren();

        // Generate tiles
        for (int y = 1; y <= Grid.Height; y++)
        {
            for (int x = 1; x <= Grid.Width; x++)
            {
                DisplayTile(x, y);
            }
        }

        DisplayEnemies();
    }

    private void DisplayTile(int x, int y)
    {
        // Get ref
        GridTileData tile = Grid[x, y];

        // Create
        GridTileController newTile = Instantiate(_tileVisualization, _gridContainer).GetComponent<GridTileController>();
        newTile.gameObject.name = $"Tile ({x},{y})";

        newTile.Data = tile;
        tile.Controller = newTile;

        // Position
        newTile.transform.localPosition = new(x * TILE_SPACING, 0, y * TILE_SPACING);

        // Clear occupants
        tile.SetOccupants(null);
    }

    private void DisplayEnemies()
    {
        List<GridTileData> enemySpawnTilePool = Grid.Tiles.Where(tile => tile.Value.HasTag(GridTileData.TileTag.EnemySpawn)).Select(tile => tile.Value).ToList();
        Debug.Assert(enemySpawnTilePool.Count >= Grid.Enemies.Count, "Amount of enemies to spawn is greater than available spawn tiles!");

        List<GridTileData> enemySpawnTiles = new();
        for (int i = 0; i < Grid.Enemies.Count; i++)
        {
            int randIndex = Random.Range(0, enemySpawnTilePool.Count);

            enemySpawnTiles.Add(enemySpawnTilePool[randIndex]);
            enemySpawnTilePool.RemoveAt(randIndex);
        }

        Debug.Assert(enemySpawnTiles.Count == Grid.Enemies.Count);
        for (int i = 0; i < Grid.Enemies.Count; i++)
        {
            // Validate
            Debug.Assert(Grid.Enemies[i] != null, "Enemy cannot be null!", this);

            // Create visual representation and clone
            var instantiatedEnemy = Instantiate(Grid.Enemies[i].gameObject, enemySpawnTiles[i].Controller.transform).GetComponent<GridEntity>();
            instantiatedEnemy.transform.localPosition = Vector3.zero;

            // Assign data
            enemySpawnTiles[i].AddOccupant(instantiatedEnemy);
            instantiatedEnemy.Location = Grid[enemySpawnTiles[i].Coordinates];
            AssignEntityID(instantiatedEnemy);
        }
    }

    #endregion

    #region Occupant Management

    private static int _lastEntityID = 0;

    public static void AssignEntityID(GridEntity entity)
    {
        _lastEntityID++;
        entity.ID = _lastEntityID;
    }

    #endregion

    #region Debug

#if UNITY_EDITOR
    [Button]
    private void LogGridInfo()
    {
        string result = "";
        foreach (var tile in Grid.Tiles) result += $"{tile.Value}\n";
        Debug.Log(result);
    }
#endif

    #endregion
}
