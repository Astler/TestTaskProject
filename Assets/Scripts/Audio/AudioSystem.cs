using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Audio
{
    public class AudioSystem : MonoBehaviour
    {
        private static AudioSystem Instance { get; set; }

        [SerializeField] private AudioAssetsSo audioAssets;
        [SerializeField] private AudioMixerGroup group;

        private ComponentPool<AudioSource> _pool;
        private const int MaxConcurrentSounds = 20;
        private readonly HashSet<AudioSource> _activeSources = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            GameObject poolRoot = new("[Audio Pool]");
            DontDestroyOnLoad(poolRoot);
            _pool = new ComponentPool<AudioSource>(poolRoot, 50);
        }

        public void PlayOneShot(AudioClipName id, AudioParameters parameters = default)
        {
            if (_activeSources.Count >= MaxConcurrentSounds && !parameters.IsBackgroundMusic)
            {
                var weakestSound = _activeSources.Where(s => !s.loop).OrderBy(s => s.volume).FirstOrDefault();

                if (weakestSound != null)
                {
                    StopAndCleanSource(weakestSound);
                }
            }

            PlayAudioEvent(new AudioEvent(id, parameters));
        }

        private void PlayAudioEvent(AudioEvent evt)
        {
            AudioClip clip = audioAssets.GetAudioClip(evt.Id);
            if (clip == null)
            {
                Debug.LogError("Can't find audio clip with name: " + evt.Id);
                return;
            }

            AudioSource source = _pool.GetAvailableComponent(s => !s.isPlaying);
            source.clip = clip;
            source.outputAudioMixerGroup = group;
            evt.Parameters.ApplyTo(source);

            _activeSources.Add(source);
            source.Play();

            if (!evt.Parameters.IsBackgroundMusic)
            {
                StartCoroutine(CleanupAfterPlay(source, clip.length));
            }
        }

        private IEnumerator CleanupAfterPlay(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay + 0.1f);
            while (source != null && source.isPlaying)
            {
                yield return new WaitForSeconds(0.1f);
            }
    
            if (source != null && !source.loop)
            {
                StopAndCleanSource(source);
            }
        }

        private void StopAndCleanSource(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            _activeSources.Remove(source);
        }

        private void OnDestroy()
        {
            foreach (var source in _activeSources)
            {
                if (source != null)
                {
                    StopAndCleanSource(source);
                }
            }

            _activeSources.Clear();
        }

        #region Static Presets

        public static void
            Play(AudioClipName clipId, float volume = 0.75f, float pitch = 1f, Transform target = null) =>
            Instance.PlayOneShot(clipId,
                new AudioParameters(volume: volume, pitch: pitch, position: target?.position ?? Vector3.zero));

        public static void PlayMusic(AudioClipName clipId, float volume = 0.5f) => Instance.PlayOneShot(clipId,
            new AudioParameters(volume: volume, loop: true, isBackgroundMusic: true));

        #endregion
    }
}