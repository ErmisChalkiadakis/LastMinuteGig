public interface IMusicClipSetProvider
{
    MusicClipSet GetFirstClipSet();
    MusicClipSet GetNextClipSet(MusicClipResults[] musicClipResults);
}
