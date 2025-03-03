using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorExtensions
{
    private const int DEFAULT_HEADER_SIZE = 20;
    public static void DrawHeader(string text, int size = DEFAULT_HEADER_SIZE)
    {
        GUIStyle headerStyle = new(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = size,
        };
        GUILayout.Label(text, headerStyle);
    }
}
