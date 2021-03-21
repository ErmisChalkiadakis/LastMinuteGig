using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicMixer : MonoBehaviour
{
    public delegate void ClipScheduledHandler(MusicClip scheduledClip, double startingTime);
    public event ClipScheduledHandler ClipScheduledEvent;
    public event ClipScheduledHandler ClipQueuedEvent;

    public double StartTime => startTime;

    private const float FLIP_INTERVAL = 1f;
    private const float DELAY_UNTIL_FIRST = 0.5f;
    private const int MAX_LAYERED_CLIPS = 5;

    private int flip = 0;
    private AudioSource[] percussionAudioSources = new AudioSource[2];
    private AudioSource[] inputAudioSources = new AudioSource[2];
    private AudioSource[] layerAudioSources = new AudioSource[MAX_LAYERED_CLIPS];
    private AudioSource[] layerAudioSourcesFlipped = new AudioSource[MAX_LAYERED_CLIPS];
    private double nextEventTime;
    private double startTime;

    private Queue<MusicClip> clipQueue = new Queue<MusicClip>();

    protected void Awake()
    {
        CreateAudioPlayers();
    }

    protected void Update()
    {
        if (AudioSettings.dspTime + FLIP_INTERVAL > nextEventTime)
        {
            if (clipQueue.Count > 0)
            {
                ScheduleNextClipPlay();
            }
        }
    }

    public void QueueClip(MusicClip musicClip)
    {
        if (clipQueue.Count == 0)
        {
            SetStartTime();
        }

        clipQueue.Enqueue(musicClip);
        double queuedClipStartTime = CalculateQueuedClipStartTime();
        ClipQueuedEvent?.Invoke(musicClip, queuedClipStartTime);
    }

    private void ScheduleNextClipPlay()
    {
        MusicClip nextClip = clipQueue.Dequeue();

        if (nextClip.PercussionClip.AudioClip == null)
        {
            percussionAudioSources[flip].clip = null;
        }
        else
        {
            percussionAudioSources[flip].clip = nextClip.PercussionClip.AudioClip;
            percussionAudioSources[flip].PlayScheduled(nextEventTime);
        }

        for (int i = 0; i < nextClip.LayerClips?.Length; i++)
        {
            if (i >= MAX_LAYERED_CLIPS)
            {
                Debug.LogError($"Too many Layered Clips");
                break;
            }

            if (flip == 0)
            {
                layerAudioSources[i].clip = nextClip.LayerClips[i].AudioClip;
                layerAudioSources[i].PlayScheduled(nextEventTime);
            }
            else
            {
                layerAudioSourcesFlipped[i].clip = nextClip.LayerClips[i].AudioClip;
                layerAudioSourcesFlipped[i].PlayScheduled(nextEventTime);
            }
        }

        if (nextClip.InputClip != null)
        {
            ClipScheduledEvent?.Invoke(nextClip, nextEventTime);
        }

        nextEventTime += nextClip.Duration;

        flip = 1 - flip;
    }

    private void CreateAudioPlayers()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject child = new GameObject($"Percussion Player {i}");
            child.transform.parent = transform;
            percussionAudioSources[i] = child.AddComponent<AudioSource>();

            child = new GameObject($"Input Player {i}");
            child.transform.parent = transform;
            inputAudioSources[i] = child.AddComponent<AudioSource>();
            inputAudioSources[i].volume = 0.8f;
        }

        for (int i = 0; i < MAX_LAYERED_CLIPS; i++)
        {
            GameObject child = new GameObject($"Layer Player {i}");
            child.transform.parent = transform;
            layerAudioSources[i] = child.AddComponent<AudioSource>();

            child = new GameObject($"Layer Player {i}");
            child.transform.parent = transform;
            layerAudioSourcesFlipped[i] = child.AddComponent<AudioSource>();
        }
    }

    private double CalculateQueuedClipStartTime()
    {
        double time = nextEventTime;
        List<MusicClip> list = clipQueue.ToArray().ToList();
        while (list.Count > 1)
        {
            time += list[0].Duration;
            list.RemoveAt(0);
        }

        return time;
    }

    private void SetStartTime()
    {
        startTime = AudioSettings.dspTime + DELAY_UNTIL_FIRST;
        nextEventTime = startTime;
    }
}
