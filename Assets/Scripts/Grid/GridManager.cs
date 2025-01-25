using System.Collections;
using System.Collections.Generic;
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
        Grid.InitializeGrid(true);
    }

    #endregion

    #region Display 

#if UNITY_EDITOR
    [Button]
#endif
    private void DisplayGrid()
    {
        // Clear
        _gridContainer.transform.RemoveChildren();

        // Generate
        for (int y = 1; y <= Grid.Height; y++)
        {
            for (int x = 1; x <= Grid.WidthL; x++)
            {
                DisplayTile(x, y);
            }

            for (int x = 1; x <= Grid.WidthR; x++)
            {
                DisplayTile(-x, y);
            }
        }
    }

    private void DisplayTile(int x, int y)
    {
        // Get ref
        GridTileData tile = Grid[x, y];

        // Validate
        if (!tile.IsEnabled) return;

        // Create
        GridTileController newTile = Instantiate(_tileVisualization, _gridContainer).GetComponent<GridTileController>();
        newTile.gameObject.name = $"Tile ({x},{y})";

        newTile.Data = tile;
        tile.Controller = newTile;

        // Position
        newTile.transform.localPosition = new(x * TILE_SPACING, 0, y * TILE_SPACING);

        // Handle occupants
        for (int i = 0; i < tile.Occupants.Count; i++)
        {
            // Validate
            Debug.Assert(tile.Occupants[i] != null, "Occupant cannot be null!", this);

            // Create visual representation and clone
            tile.Occupants[i] = Instantiate(tile.Occupants[i].gameObject, newTile.transform).GetComponent<GridEntity>();
            tile.Occupants[i].transform.localPosition = Vector3.zero;

            // Assign data
            tile.Occupants[i].Location = Grid[x, y];
            AssignEntityID(tile.Occupants[i]);
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
        foreach (var tile in Grid.Tiles) result += $"{tile}\n";
        Debug.Log(result);
    }
#endif

    #endregion
}
