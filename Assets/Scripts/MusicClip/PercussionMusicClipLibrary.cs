﻿using UnityEngine;
using Random = System.Random;

public class PercussionMusicClipLibrary : ScriptableObject
{
    [SerializeField] private PercussionMusicClip[] percussionMusicClips;

    public PercussionMusicClip GetRandomClip()
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

    public PercussionMusicClip GetRandomClipWithTempo(float tempo)
    {
        foreach (var percussionMusicClip in percussionMusicClips)
        {
            if (percussionMusicClip.Tempo == tempo)
            {
                return percussionMusicClip;
            }
        }

        Debug.LogError($"No percussion clip found with Tempo: {tempo}");
        return null;
    }

    public PercussionMusicClip GetRandomClipWithRhythm(Rhythm rhythm)
    {
        foreach (var percussionMusicClip in percussionMusicClips)
        {
            if (percussionMusicClip.Rhythm == rhythm)
            {
                return percussionMusicClip;
            }
        }

        Debug.LogError($"No percussion clip found with Rhythm: {rhythm}");
        return null;
    }

    public PercussionMusicClip GetRandomClipWithRhythmAndTempo(Rhythm rhythm, float tempo)
    {
        foreach (var percussionMusicClip in percussionMusicClips)
        {
            if (percussionMusicClip.Rhythm == rhythm && percussionMusicClip.Tempo == tempo)
            {
                return percussionMusicClip;
            }
        }

        Debug.LogError($"No percussion clip found with Rhythm: {rhythm} and Tempo: {tempo}");
        return null;
    }

    public PercussionMusicClip GetClipWithName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        foreach (var percussionMusicClip in percussionMusicClips)
        {
            if (percussionMusicClip.Name == name)
            {
                return percussionMusicClip;
            }
        }

        Debug.LogError($"No clip found with name: {name}");
        return null;
    }
}
