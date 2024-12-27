using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Removes all children from a Transform object
    /// </summary>
    /// <param name="parent">Parent object, which children will be removed</param>
    /// <returns>Number of children removed</returns>
    public static int RemoveChildren(this Transform parent)
    {
        int removedCount = 0;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

#if UNITY_EDITOR
            if (Application.isPlaying) GameObject.Destroy(child.gameObject); // In Play Mode
            else GameObject.DestroyImmediate(child.gameObject); // In Edit Mode
#else
            GameObject.Destroy(child.gameObject);
#endif

            removedCount++;
        }

        return removedCount;
    }
}
