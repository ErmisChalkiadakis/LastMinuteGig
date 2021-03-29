using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SustainedProtocolScreen : MonoBehaviour
{
    private static int SHOW_HASH = Animator.StringToHash("Show");

    private const float PROTOCOL_ENDED_DELAY = 2f;

    [SerializeField] private MusicClipManager musicClipManager;
    [SerializeField] private StickFigure stickFigure;
    [SerializeField] private Animator stickFigureAnimator;
    [SerializeField] private Animator sequenceAnimator;
    [SerializeField] private AnimatorStateObserver sequenceAnimatorStateObserver;
    [SerializeField] private int numberOfTotalSongs = 4;

    private int songCount = 0;

    protected void Awake()
    {
        InitializeStickAnimator();
        ShowIntroSequence();
        musicClipManager.SequenceEndedEvent += OnSequenceEndedEvent;
    }

    protected void OnDestroy()
    {
        musicClipManager.SequenceEndedEvent -= OnSequenceEndedEvent;
    }

    private void OnSequenceEndedEvent()
    {
        stickFigure.StopRocking();

        if (songCount < numberOfTotalSongs)
        {
            ShowInbetweenSequence();
        }
        else
        {
            ShowOutroSequence();
        }
    }

    private void InitializeStickAnimator()
    {
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Tutorial"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Rhythm"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Chord"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Play"), 1);
    }

    private void ShowIntroSequence()
    {
        // TODO 
        //StartCoroutine(StartFirstSongAfterSeconds(3f));
    }

    private void ShowInbetweenSequence()
    {
        // TODO
        PlayNextSong();
    }

    private void ShowOutroSequence()
    {
        // TODO
        StartCoroutine(GoBackToMainMenuAfterSeconds(PROTOCOL_ENDED_DELAY));
    }

    private void PlayNextSong()
    {
        songCount++;
        musicClipManager.StartSong();
    }

    private IEnumerator StartFirstSongAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);

        PlayNextSong();
    }

    private IEnumerator GoBackToMainMenuAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("MainMenu");
    }
}
