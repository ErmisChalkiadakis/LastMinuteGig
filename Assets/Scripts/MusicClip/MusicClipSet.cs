public class MusicClipSet
{
    public int ClipCount => MusicClips.Length;
    public bool IsEmpty;
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
    public ChordProgression ChordProgression;

    public MusicClipSet(MusicClip[] musicClips, Key key, Rhythm rhythm, Tempo tempo, ChordProgression chordProgression, bool isEmpty)
    {
        MusicClips = musicClips;
        Key = key;
        Rhythm = rhythm;
        Tempo = tempo;
        ChordProgression = chordProgression;
        IsEmpty = isEmpty;
    }
}
