using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioDatabase", menuName = "Audio/Audio Database")]
public class AudioDatabase : ScriptableObject
{
    private Dictionary<string, AudioEntry> AudioGroups = new  Dictionary<string, AudioEntry>();
    
    public AudioEntry? GetAudioEntry(string key)
    {
        if (AudioGroups.TryGetValue(key, out AudioEntry result))
            return result;

        return null;
    }
}