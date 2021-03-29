public class MusicClip
{
    private static int idCounter = 0;

    public int ID;
    public PercussionMusicClip PercussionClip;
    public InputMusicClip InputClip;
    public LayerMusicClip[] LayerClips;

    public double Duration => PercussionClip.ClipDuration;

    public MusicClip(PercussionMusicClip percussionClip, InputMusicClip inputClip, LayerMusicClip[] layerClips)
    {
        ID = ++idCounter;
        PercussionClip = percussionClip;
        InputClip = inputClip;
        LayerClips = layerClips;
    }

    public MusicClip(int id, PercussionMusicClip percussionClip, InputMusicClip inputClip, LayerMusicClip[] layerClips)
    {
        ID = ++idCounter;
        PercussionClip = percussionClip;
        InputClip = inputClip;
        LayerClips = layerClips;
    }

    public MusicClip(Tempo tempo, Rhythm rhythm)
    {
        ID = ++idCounter;
        PercussionClip = new PercussionMusicClip(tempo, rhythm);
    }
}
