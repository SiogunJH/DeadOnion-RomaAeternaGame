using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using VInspector;
#endif

[CreateAssetMenu(fileName = "NewAudioDatabase", menuName = "Audio/Audio Database")]
public class AudioDatabase : ScriptableObject
{
    // TODO: Display AudioGroups without the usage of SerializedDictionary or isolate it from vInspector and UnityEditor packages
    [SerializeField] private SerializedDictionary<string, AudioEntry> AudioGroups = new();

    public AudioEntry? GetAudioEntry(string key)
    {
        if (AudioGroups.TryGetValue(key, out AudioEntry result))
            return result;

        return null;
    }
}