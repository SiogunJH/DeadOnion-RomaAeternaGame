using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileController : MonoBehaviour
{
    public GridTileData Data;

    public void Highlight()
    {
        Debug.Log($"HIGHLIGHTING: [{Data}]");
    }

    public void Unhighlight()
    {
        Debug.Log($"UIHIGHLIGHTING: [{Data}]");
    }
}
