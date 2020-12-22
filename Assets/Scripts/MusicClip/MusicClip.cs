public class MusicClip
{
    public int ID;
    public PercussionMusicClip PercussionClip;
    public InputMusicClip InputClip;
    public LayerMusicClip[] LayerClips;

    public double Duration => PercussionClip.ClipDuration;

    public MusicClip(PercussionMusicClip percussionClip, InputMusicClip inputClip, LayerMusicClip[] layerClips)
    {
        PercussionClip = percussionClip;
        InputClip = inputClip;
        LayerClips = layerClips;
    }

    public MusicClip(int id, PercussionMusicClip percussionClip, InputMusicClip inputClip, LayerMusicClip[] layerClips)
    {
        ID = id;
        PercussionClip = percussionClip;
        InputClip = inputClip;
        LayerClips = layerClips;
    }

    public MusicClip(int id, Tempo tempo, Rhythm rhythm)
    {
        ID = id;
        PercussionClip = new PercussionMusicClip(tempo, rhythm);
    }
}
