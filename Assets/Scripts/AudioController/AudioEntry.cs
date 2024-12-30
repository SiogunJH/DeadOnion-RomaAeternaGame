using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
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

    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] public AudioMixerGroup AudioMixerGroup;
    [SerializeField, Range(-3f, 3f)] public float MinPitch;
    [SerializeField, Range(-3f, 3f)] public float MaxPitch;
    [SerializeField] private PlaybackMode _playbackMode;

    private int _lastIndex;

    public AudioEntry(AudioClip[] clips, AudioMixerGroup mixer, float minPitch, float maxPitch, PlaybackMode mode)
    {
        _audioClips = clips;
        AudioMixerGroup = mixer;
        this.MinPitch = minPitch;
        this.MaxPitch = maxPitch;
        _playbackMode = mode;


        _lastIndex = -1;

        // init for UseAllBeforeRepeat
        _availableIndices = new int[clips.Length];
        for (int i = 0; i < _availableIndices.Length; i++)
        {
            _availableIndices[i] = i;
        }
    }

    public AudioClip GetAudioClip()
    {
        if (_audioClips == null || _audioClips.Length == 0)
            return null;

        switch (_playbackMode)
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
        _lastIndex = (_lastIndex + 1) % _audioClips.Length;
        return _audioClips[_lastIndex];
    }

    private AudioClip GetRandom()
    {
        return _audioClips[Random.Range(0, _audioClips.Length)];
    }

    private AudioClip GetNoConsecutiveRepeat()
    {

        int newIndex = Random.Range(0, _audioClips.Length);

        if (newIndex == _lastIndex)
            return GetInOrder();

        _lastIndex = newIndex;
        return _audioClips[newIndex];
    }

    private int[] _availableIndices;

    // shuffles with Fisher-Yates algorithm
    private void ShuffleOrder()
    {
        for (int i = _availableIndices.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (_availableIndices[i], _availableIndices[j]) = (_availableIndices[j], _availableIndices[i]);
        }
    }

    private AudioClip GetAllBeforeRepeat()
    {
        _lastIndex = (_lastIndex + 1) % _audioClips.Length;
        if (_lastIndex == 0)
        {
            ShuffleOrder();
        }
        return _audioClips[_availableIndices[_lastIndex]];
    }
}
