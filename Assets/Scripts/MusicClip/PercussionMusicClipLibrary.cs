using UnityEngine;
using Random = System.Random;

public class PercussionMusicClipLibrary : ScriptableObject
{
    [SerializeField] private PercussionMusicClip[] percussionMusicClips;

    public PercussionMusicClip GetRandomPercussionClip()
    {
        if (percussionMusicClips.Length > 0)
        {
            Random random = new Random();

            int randomIndex = random.Next(percussionMusicClips.Length);
            return percussionMusicClips[randomIndex];
        }

        Debug.LogError($"PercussionMusicClipLibrary is empty.");
        return null;
    }
}
