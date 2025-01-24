using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    private List<Character> _charactersOnMap = new();


    private void Awake()
    {
        Instance = this;
    }


    private void LoadCharactersOnMap()
    {
        _charactersOnMap.Clear();
        ///////////////////////////////////
        Debug.LogError("Code not finished");
        GridMap gm = new();
        //GridMap gm = GameManager.CurrentGridMap;
        ///////////////////////////////////

        foreach (var tile in gm.Tiles)
        {
            foreach (var occupant in tile.Occupants)
            {
                if (occupant == null) continue;
                if (occupant is Character) _charactersOnMap.Add((Character)occupant);
            }
        }

        SortCharacters();
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


    
    public void ContinueTurn()
    {
        if(_charactersOnMap.Where(c => c.HadTurn == false).Count() == 0) EndRound();
        SortCharacters();
        _charactersOnMap.Where(c => c.HadTurn == false).FirstOrDefault().BeginTurn();
    }
    private void EndRound()
    {
        foreach(var c in _charactersOnMap)
        {
            c.ResetTurn();
        }
    }
}
