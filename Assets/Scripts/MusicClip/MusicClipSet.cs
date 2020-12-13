public class MusicClipSet
{
    public int ClipCount => MusicClips.Length;
    public double Duration
    {
        get
        {
            double duration = 0;
            foreach (var musicClip in MusicClips)
            {
                duration += musicClip.Duration;
            }
            return duration;
        }
    }
    public MusicClip[] MusicClips;
    public Key Key;
    public Rhythm Rhythm;
    public Tempo Tempo;

    public MusicClipSet(MusicClip[] musicClips, Key key, Rhythm rhythm, Tempo tempo)
    {
        MusicClips = musicClips;
        Key = key;
        Rhythm = rhythm;
        Tempo = tempo;
    }
}
