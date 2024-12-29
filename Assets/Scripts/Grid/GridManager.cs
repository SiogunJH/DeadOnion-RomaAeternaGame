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
        Grid.OptimizeGrid();
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
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int z = 0; z < Grid.Height; z++)
            {
                // Validate
                if (!Grid[x, z].IsEnabled) continue;

                // Create
                GameObject newTile = Instantiate(_tileVisualization, _gridContainer);
                newTile.name = $"Tile ({x},{z})";

                // Position
                newTile.transform.localPosition = new(x * TILE_SPACING, 0, z * TILE_SPACING);

                // Handle occupants
                foreach (var occupant in Grid[x, z].Occupants)
                {
                    Debug.Assert(occupant != null, "Occupant cannot be null!", this);

                    GameObject newOccupant = Instantiate(occupant.gameObject, newTile.transform);
                    newOccupant.transform.localPosition = Vector3.zero;
                }
            }
        }
    }

    #endregion
}
