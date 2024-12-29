using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using VInspector;
#endif

[System.Serializable]
public class GridEntity : MonoBehaviour
{
    [HideInInspector] public GridTileData Location = null;
    [HideInInspector] public int ID = -1;

    public string UserFriendlyName { get => CharacterProfile.Name; }
    public GridMap Map { get => GridManager.Instance.Grid; }

    public CharacterProfile CharacterProfile;
    public GridEntityCategory Category;
}
