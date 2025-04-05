using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using VInspector;
#endif

public class CombatManager : MonoBehaviourSingleton<CombatManager>
{
    [Tab("Combat Manager")]
    public CombatAbilityUIManager UI;
    public Character CurrentCombatant = null;
    public CombatAbility CurrentAbility = null;
    public CombatAbility MostRecentAbility = null;

    private List<Character> _charactersOnMap = new();
    private int _roundNumber;

    private Character _combatantNextInTurn
    {
        get
        {
            SortCharacters();
            return _charactersOnMap.FirstOrDefault(c => !c.HadTurn);
        }
    }

    #region MonoBehaviour

    private void Start()
    {
        Debug.Log("Initializing [Combat Manager]");
        Initialize();
    }

    #endregion

    #region Initialization

#if UNITY_EDITOR
    // [Button]
#endif
    public void Initialize()
    {
        // Validate
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Combat cannot be initialized outside of play mode!");
            return;
        }

        GridManager.Instance.Initialize();

        // Handle Combatants
        if (!LoadCharactersOnMap())
        {
            Debug.LogError("Failed to load any Characters from Map!");
            return;
        }

        // Handle UI
        UI.HideAbilities();

        // Start
        _roundNumber = 0;
        StartCombat();
    }

    #endregion

    #region Character Management

    private bool LoadCharactersOnMap()
    {
        _charactersOnMap.Clear();
        GridMap gm = GridManager.Instance.Grid;
        if (gm == null) return false;

        foreach (var tile in gm.Tiles)
        {
            foreach (var occupant in tile.Value.Occupants)
            {
                if (occupant == null) continue;
                if (occupant is Character character) _charactersOnMap.Add(character);
            }
        }

        SortCharacters();
        return true;
    }

    private void SortCharacters()
    {
        _charactersOnMap = _charactersOnMap.OrderBy(c => c.CharacterProfile.TotalInitiative).ToList();
    }

    public void AddCharacter(Character c)
    {
        _charactersOnMap.Add(c);
    }

    public void RemoveCharacter(Character c)
    {
        if (!_charactersOnMap.Contains(c)) return;
        _charactersOnMap.Remove(c);
    }

    #endregion

    #region Combat Management

    public void StartCombat()
    {
        // Validate
        Debug.Assert(_charactersOnMap != null && _charactersOnMap.Count != 0, "No combatants are active!");

        Debug.Log("Combat has started!");

        UI.gameObject.SetActive(true);
        UI.HideAbilities();

        StartNewRound();
    }

    public void EndCombat()
    {
        Debug.Log("Combat has ended!");
    }

    public bool TryToEndCombat()
    {

        // No combatants left
        if (_charactersOnMap == null || _charactersOnMap.Count == 0)
        {
            EndCombat();
            return true;
        }

#pragma warning disable CS0162 // Unreachable code detected
        // TODO: Add logic checking if combat needs to end
        if (false)
        {
            EndCombat();
            return true;
        }
#pragma warning restore CS0162

        return false;
    }

    #endregion

    #region Turn Management

    public void NextTurn()
    {
        // Check if combat has ended
        if (TryToEndCombat()) return;

        var combatant = _combatantNextInTurn;
        if (combatant != null)
        {
            StartTurn(combatant);
        }
        else
        {
            StartNewRound();
        }
    }

    private void StartNewRound()
    {
        _roundNumber++;
        Debug.Log($"Round [{_roundNumber}] started!");

        foreach (var c in _charactersOnMap)
        {
            c.ResetTurn();
        }

        NextTurn();
    }

    private void StartTurn(Character combatant)
    {
        Debug.Assert(combatant != null, "Combatant is null!");

        CurrentCombatant = combatant;
        combatant.BeginTurn();
        CameraManager.Instance.CameraLookAt(combatant.gameObject.transform);
    }

    #endregion

    #region Ability Handling

    public void HighlightTilesInRange()
    {
        Character caster = CurrentCombatant;
        CombatAbility ability = CurrentAbility;

        // Mark all tiles as invalid
        IEnumerable<GridTileController> allTilesInRange = GridManager.Instance.Grid.GetTilesInPattern(caster.Location, ability.Range).Select(tile => tile.Controller);
        foreach (var tile in allTilesInRange)
        {
            tile.SetHighlightMode(GridTileController.HighlighMode.Invalid);
        }

        // Mark valid tiles
        IEnumerable<GridTileController> validTiles = ability.GetValidTargets(caster).Select(tile => tile.Controller);
        foreach (var tile in validTiles)
        {
            tile.SetHighlightMode(GridTileController.HighlighMode.Valid);
        }
    }

    /// <summary>
    /// Clears all highligh by default, but can be overriden
    /// </summary>
    public void ClearTileHighlight(IEnumerable<GridTileController> tilesToClear = null)
    {
        // If no tile pool was determined, select all
        tilesToClear ??= GridManager.Instance.Grid.Tiles.Select(tile => tile.Value.Controller);

        // Clear highlight
        foreach (var tile in tilesToClear)
        {
            tile.SetHighlightMode(GridTileController.HighlighMode.None);
        }
    }

    #endregion
}
