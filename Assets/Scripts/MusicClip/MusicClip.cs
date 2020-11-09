public class MusicClip
{
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

    public MusicClip(float tempo, Rhythm rhythm)
    {
        PercussionClip = new PercussionMusicClip(tempo, rhythm);
    }
}
