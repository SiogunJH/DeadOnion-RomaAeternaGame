using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private static TurnManager instance;
    private List<Character> _charactersOnMap = new();


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        Debug.LogError("NOT FINISHED CODE");
        //fix after changing Character to Character : GridEntity

        try
        {
            GridMap gm = new();
            foreach (var tile in gm.Tiles)
            {
                foreach (var occupant in tile.Occupants)
                {
                    if (occupant == null) continue;
                    //_charactersOnMap.Add(occupant.Character);
                }
            }
        }
        catch (System.Exception) { }

        _charactersOnMap = _charactersOnMap.OrderBy( c => c.CharacterProfile.TotalInitiative ).ToList();
    }

    void Update()
    {
        if(_currentTurn == null) Round();
        Turn();
        if(_endRound) EndRound();
    }

    private bool _endRound = false;
    private Character _currentTurn = null;
    void Round()
    {
        _currentTurn = _charactersOnMap.Where(c => c.HadTurn == false).FirstOrDefault();
        if( _currentTurn == null ) _endRound = true;
    }
    void Turn()
    {
        if (_currentTurn == null) return;
        if(_currentTurn.HadTurn) _currentTurn = null;
    }
    void EndRound()
    {
        _charactersOnMap = _charactersOnMap.OrderBy( c => c.CharacterProfile.TotalInitiative ).ToList();
        _endRound = false;
        foreach(var c in _charactersOnMap)
        {
            c.ResetTurn();
        }
    }
}
