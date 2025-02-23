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

        public void PlayOneShot(AudioClipName id, AudioParameters parameters = default) =>
            PlayAudioEvent(new AudioEvent(id, parameters));

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
            source.Play();
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