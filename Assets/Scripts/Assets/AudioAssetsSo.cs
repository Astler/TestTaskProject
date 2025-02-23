using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using Utils;

namespace Assets
{
    [CreateAssetMenu(fileName = "Audio Assets", menuName = "Test project/Audio Assets")]
    public class AudioAssetsSo : ScriptableObject
    {
        [SerializeField]
        public ValueByKey<AudioClipName, AudioClip>[] clips = Array.Empty<ValueByKey<AudioClipName, AudioClip>>();

        private readonly Dictionary<AudioClipName, AudioClip> _cachedAudioClips = new();

        public AudioClip GetAudioClip(AudioClipName clipName)
        {
            if (_cachedAudioClips.Count != 0) return _cachedAudioClips[clipName];

            foreach (ValueByKey<AudioClipName, AudioClip> valueByKey in clips)
            {
                _cachedAudioClips[valueByKey.Key] = valueByKey.Value;
            }

            return _cachedAudioClips[clipName];
        }
    }
}