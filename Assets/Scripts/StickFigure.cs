using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickFigure : MonoBehaviour
{
    private static int ROCK_HASH = Animator.StringToHash("Rock");
    private static int PLAY_HASH = Animator.StringToHash("Play");
    private static int ROCK_SPEED_HASH = Animator.StringToHash("RockSpeed");
    private const string CHORD = "Chord";
    private const Tempo BASE_TEMPO = Tempo.t120;

    [SerializeField] private Animator animator;
    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private InstrumentButton instrumentButton;

    private bool isRocking;

    protected void Awake()
    {
        musicMixer.ClipScheduledEvent += OnClipScheduledEvent;
        instrumentButton.ButtonSelectedEvent += OnButtonSelectedEvent;
    }

    protected void OnDestroy()
    {
        musicMixer.ClipScheduledEvent -= OnClipScheduledEvent;
    }

    public void StopRocking()
    {
        isRocking = false;
        animator.SetBool(ROCK_HASH, false);
    }

    private void OnClipScheduledEvent(MusicClip scheduledClip, double startingTime)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (!isRocking)
        {
            StartCoroutine(Rock(startingTime));
        }

        if (scheduledClip.InputClip != null)
        {
            StartCoroutine(ChangeChord(scheduledClip, startingTime));
        }
    }

    private void OnButtonSelectedEvent(ButtonID buttonId)
    {
        animator.SetTrigger(PLAY_HASH);
    }

    private IEnumerator Rock(double startingTime)
    {
        while (AudioSettings.dspTime < startingTime)
        {
            yield return null;
        }

        isRocking = true;
        animator.SetTrigger(ROCK_HASH);
    }

    private IEnumerator ChangeChord(MusicClip scheduledClip, double startingTime)
    {
        while (AudioSettings.dspTime < startingTime)
        {
            yield return null;
        }

        string newChordName = CHORD + scheduledClip.InputClip.Chord.ToString();
        animator.SetTrigger(Animator.StringToHash(newChordName));
        animator.SetFloat(ROCK_SPEED_HASH, TempoToBPMHelper.GetBPM(scheduledClip.PercussionClip.Tempo) / TempoToBPMHelper.GetBPM(BASE_TEMPO));
    }
}
