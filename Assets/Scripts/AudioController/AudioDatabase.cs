using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = "NewAudioDatabase", menuName = "Audio/Audio Database")]
public class AudioDatabase : ScriptableObject
{
    [SerializeField] private SerializedDict<string, AudioEntry> AudioGroups = new();

    public AudioEntry? GetAudioEntry(string key)
    {
        if (AudioGroups.TryGetValue(key, out AudioEntry result))
            return result;

        return null;
    }
}