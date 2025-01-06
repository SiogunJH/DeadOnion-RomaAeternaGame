using UnityEngine;
using Utility;

#if UNITY_EDITOR
using VInspector;
#endif

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

    #region Testing

#if UNITY_EDITOR
    [Button("Log next AudioClip from the 'Test' AudioEntry object")]
    private void LogTestEntryClip_DEBUG()
    {
        // Vlidate
        AudioEntry? entryOrNull = GetAudioEntry("Test");
        if (entryOrNull == null)
        {
            Debug.LogWarning("No AudioEntry with 'Test' key exists!");
            return;
        }

        // Get
        AudioEntry entry = (AudioEntry)entryOrNull;
        AudioClip clip = entry.GetAudioClip();

        // Display
        Debug.Log($"{clip.name}");
    }
#endif

    #endregion
}