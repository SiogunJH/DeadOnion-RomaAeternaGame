using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridEntity : MonoBehaviour
{
    [HideInInspector] public GridTile Location;
    [HideInInspector] public int ID = -1;

    public string UserFriendlyName { get => CharacterProfile.Name; }
    public CharacterProfile CharacterProfile;
    public GridEntityCategory Category;
}
