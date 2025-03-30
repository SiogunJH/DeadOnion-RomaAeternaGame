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

    #region Initialization

    public void Initialize()
    {
        Debug.Log("Initializing [Grid Manager]");

        DisplayGrid();
    }

    #endregion

    #region Display 

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

        DisplayCombatants(Grid.Enemies, GridTileData.TileTag.EnemySpawn);
        DisplayCombatants(Grid.Allies, GridTileData.TileTag.AllySpawn);
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

    private void DisplayCombatants(List<Character> combatants, GridTileData.TileTag spawnTileTag)
    {
        List<GridTileData> combatantSpawnTilePool = Grid.Tiles.Where(tile => tile.Value.HasTag(spawnTileTag)).Select(tile => tile.Value).ToList();
        Debug.Assert(combatantSpawnTilePool.Count >= combatants.Count, "Amount of combatants to spawn is greater than available spawn tiles!");

        List<GridTileData> combatantSpawnTiles = new();
        for (int i = 0; i < combatants.Count; i++)
        {
            int randIndex = Random.Range(0, combatantSpawnTilePool.Count);

            combatantSpawnTiles.Add(combatantSpawnTilePool[randIndex]);
            combatantSpawnTilePool.RemoveAt(randIndex);
        }

        Debug.Assert(combatantSpawnTiles.Count == combatants.Count);
        for (int i = 0; i < combatants.Count; i++)
        {
            // Validate
            Debug.Assert(combatants[i] != null, "Enemy cannot be null!", this);

            // Create visual representation and clone
            var instantiatedCombatant = Instantiate(combatants[i].gameObject, combatantSpawnTiles[i].Controller.transform).GetComponent<GridEntity>();
            instantiatedCombatant.transform.localPosition = Vector3.zero;

            // Assign data
            combatantSpawnTiles[i].AddOccupant(instantiatedCombatant);
            instantiatedCombatant.Location = Grid[combatantSpawnTiles[i].Coordinates];
            AssignEntityID(instantiatedCombatant);

            // Log
            if (instantiatedCombatant is Character instantiatedCharacter)
            {
                Debug.Log($"Spawned [{instantiatedCombatant.UserFriendlyName}] with [{instantiatedCharacter.CurrentHealth}/{instantiatedCharacter.CharacterProfile.TotalVitality}] health");
            }
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
