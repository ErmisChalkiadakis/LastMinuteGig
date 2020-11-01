using System.Collections.Generic;
using UnityEngine;

public class MusicMixer : MonoBehaviour
{
    public delegate void ClipScheduledHandler(PercussionMusicClip scheduledClip, double startingTime);
    public event ClipScheduledHandler ClipScheduledEvent;

    private const float FLIP_INTERVAL = 0.5f;
    private const float DELAY_UNTIL_FIRST = 1f;

    private int flip = 0;
    private AudioSource[] audioSources = new AudioSource[2];
    private double nextEventTime;

    private Queue<PercussionMusicClip> clipQueue = new Queue<PercussionMusicClip>();

    protected void Awake()
    {
        CreateAudioPlayers();
        nextEventTime = AudioSettings.dspTime + DELAY_UNTIL_FIRST;
    }

    protected void Update()
    {
        if (AudioSettings.dspTime + FLIP_INTERVAL > nextEventTime )
        {
            if (clipQueue.Count > 0)
            {
                ScheduleNextClipPlay();
            }
        }
    }

    public void QueueClip(PercussionMusicClip musicClip)
    {
        clipQueue.Enqueue(musicClip);
    }

    private void ScheduleNextClipPlay()
    {
        PercussionMusicClip nextClip = clipQueue.Dequeue();

        audioSources[flip].clip = nextClip.AudioClip;
        audioSources[flip].PlayScheduled(nextEventTime);
        ClipScheduledEvent?.Invoke(nextClip, nextEventTime);

        nextEventTime += nextClip.ClipDuration;

        flip = 1 - flip;
    }

    private void CreateAudioPlayers()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject child = new GameObject($"Player {i}");
            child.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
        }
    }
}
