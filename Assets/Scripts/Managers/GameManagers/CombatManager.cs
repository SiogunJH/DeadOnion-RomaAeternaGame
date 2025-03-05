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

#if UNITY_EDITOR
    [Button]
#endif
    public void Initialize()
    {
        // Validate
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Combat cannot be initialized outside of play mode!");
            return;
        }

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

    public void HighlightTilesInRange(Character caster, CombatAbility ability, bool setHighlight)
    {
        IEnumerable<GridTileController> tilesToHighlight = GridManager.Instance.Grid.GetTilesInPattern(caster.Location, ability.Range).Select(tile => tile.Controller);
        foreach (var tile in tilesToHighlight)
        {
            tile.Highlight();
        }
    }

    #endregion
}
