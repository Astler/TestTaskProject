namespace Audio
{
    public readonly struct AudioEvent
    {
        public AudioClipName Id { get; }
        public AudioParameters Parameters { get; }

        public AudioEvent(AudioClipName id, AudioParameters parameters)
        {
            Id = id;
            Parameters = parameters;
        }
    }
}