using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridEntity : MonoBehaviour
{
    public string UserFriendlyName;
    [HideInInspector] public GridTile Location;
    [HideInInspector] public int ID = -1;

    public GridEntityCategory Category;
}
