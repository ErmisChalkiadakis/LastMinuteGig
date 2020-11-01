public class MusicClip
{
    public PercussionMusicClip PercussionClip;
    public InputMusicClip InputClip;
    public LayerMusicClip[] LayerClips;

    public double Duration => PercussionClip.ClipDuration;
}
