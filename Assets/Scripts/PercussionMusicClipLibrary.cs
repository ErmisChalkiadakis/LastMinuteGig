using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PercussionMusicClipLibrary : ScriptableObject
{
    [SerializeField] private PercussionMusicClip[] musicClips;

    /*
    public PercussionMusicClip GetClipWithChord(Chord chord)
    {
        foreach (var musicClip in musicClips)
        {
            if (musicClip.MiddleButtonClipData.chord == chord)
            {
                return musicClip;
            }
        }

        Debug.LogError($"No music clip with chord {Enum.GetName(typeof(Chord), chord)}");
        return null;
    }*/

    public PercussionMusicClip GetRandomPercussionClip()
    {
        if (musicClips.Length > 0)
        {
            Random random = new Random();

            List<int> indices = new List<int>();
            for (int i = 0; i < musicClips.Length; i++)
            {
                indices.Add(i);
            }

            int randomInt;
            while (indices.Count > 0)
            {
                randomInt = random.Next(indices.Count);
                if (musicClips[randomInt].Instrument == Instrument.Percussion)
                {
                    return musicClips[randomInt];
                }

                indices.RemoveAt(randomInt);
            }


            Debug.LogError($"No Percussion clip found.");
            return null;
        }

        Debug.LogError($"MusicClipLibrary is empty.");
        return null;
    }
}
