using UnityEngine;

namespace Audio
{
    public readonly struct AudioParameters
    {
        public float Volume { get; }
        public float Pitch { get; }
        public bool Loop { get; }
        public bool IsBackgroundMusic { get; }
        public Vector3? Position { get; }

        public AudioParameters(float volume, float pitch = 1f, bool loop = false, bool isBackgroundMusic = false, Vector3? position = null)
        {
            Volume = volume;
            Pitch = pitch;
            Loop = loop;
            IsBackgroundMusic = isBackgroundMusic;
            Position = position;
        }

        public void ApplyTo(AudioSource source)
        {
            source.volume = Volume;
            source.pitch = Pitch;
            source.loop = Loop;
            source.spatialBlend = Position.HasValue ? 1f : 0f;
            if (Position.HasValue) source.transform.position = Position.Value;
        }
    }
}