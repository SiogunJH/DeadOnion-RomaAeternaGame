using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    public string UserFriendlyName;
    public GridTile Location;
    public GridEntityCategory Category;
}

public enum GridEntityCategory
{
    Ally, Enemy, Object
}
