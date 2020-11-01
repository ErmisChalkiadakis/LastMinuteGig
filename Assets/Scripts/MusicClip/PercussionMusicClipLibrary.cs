using UnityEngine;
using Random = System.Random;

public class PercussionMusicClipLibrary : ScriptableObject
{
    [SerializeField] private PercussionMusicClip[] musicClips;

    public PercussionMusicClip GetRandomPercussionClip()
    {
        if (musicClips.Length > 0)
        {
            Random random = new Random();

            int randomInt = random.Next(musicClips.Length);
            return musicClips[randomInt];
        }

        Debug.LogError($"PercussionMusicClipLibrary is empty.");
        return null;
    }
}
