using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

[System.Serializable]
public struct AudioEntry
{
    public enum PlaybackMode
    {
        InOrder,
        Random,
        NoConsecutiveRepeats,
        UseAllSamplesBeforeRepeat
    }

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] public AudioMixerGroup audioMixerGroup;
    [SerializeField, Range(-3f, 3f)] public float minPitch;
    [SerializeField, Range(-3f, 3f)] public float maxPitch;
    [SerializeField] private PlaybackMode playbackMode;

    private int lastIndex;

    public AudioEntry(AudioClip[] clips, AudioMixerGroup mixer, float minPitch, float maxPitch, PlaybackMode mode)
    {
        audioClips = clips;
        audioMixerGroup = mixer;
        this.minPitch = minPitch;
        this.maxPitch = maxPitch;
        playbackMode = mode;


        lastIndex = -1;

        // dla UseAllBeforeRepeat
        availableIndices = new int[clips.Length];
        for (int i = 0; i < availableIndices.Length; i++)
        {
            availableIndices[i] = i;
        }
    }

    public AudioClip GetAudioClip()
    {
        if (audioClips == null || audioClips.Length == 0)
            return null;

        switch (playbackMode)
        {
            case PlaybackMode.InOrder:
                return GetInOrder();
            case PlaybackMode.Random:
                return GetRandom();
            case PlaybackMode.NoConsecutiveRepeats:
                return GetNoConsecutiveRepeat();
            case PlaybackMode.UseAllSamplesBeforeRepeat:
                return GetAllBeforeRepeat();
            default:
                return null;
        }
    }


    private AudioClip GetInOrder()
    {
        lastIndex = (lastIndex + 1) % audioClips.Length;
        return audioClips[lastIndex];
    }

    private AudioClip GetRandom()
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }

    private AudioClip GetNoConsecutiveRepeat()
    {

        int newIndex = Random.Range(0, audioClips.Length);

        if (newIndex == lastIndex)
            return GetInOrder();

        lastIndex = newIndex;
        return audioClips[newIndex];
    }

    private int[] availableIndices;

    // miesza kolejność algorytmem Fisher-Yates
    private void ShuffleOrder()
    {
        for (int i = availableIndices.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (availableIndices[i], availableIndices[j]) = (availableIndices[j], availableIndices[i]);
        }
    }

    private AudioClip GetAllBeforeRepeat()
    {
        lastIndex = (lastIndex + 1) % audioClips.Length;
        if (lastIndex == 0)
        {
            ShuffleOrder();
        }
        return audioClips[availableIndices[lastIndex]];
    }
}
