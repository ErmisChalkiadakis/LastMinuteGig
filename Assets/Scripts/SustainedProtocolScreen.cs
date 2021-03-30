using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SustainedProtocolScreen : MonoBehaviour
{
    private static int SHOW_INBETWEEN_HASH = Animator.StringToHash("ShowInbetween");
    private static int SHOW_OUTRO_HASH = Animator.StringToHash("ShowOutro");

    private const float PROTOCOL_ENDED_DELAY = 2f;

    [SerializeField] private MusicClipManager musicClipManager;
    [SerializeField] private StickFigure stickFigure;
    [SerializeField] private Animator stickFigureAnimator;
    [SerializeField] private Animator sequenceAnimator;
    [SerializeField] private AnimatorStateObserver sequenceAnimatorStateObserver;
    [SerializeField] private PlaySongOnEnable playSongOnEnable;
    [SerializeField] private int numberOfTotalSongs = 4;

    private int songCount = 0;

    protected void Awake()
    {
        InitializeStickAnimator();
        musicClipManager.SequenceEndedEvent += OnSequenceEndedEvent;
        playSongOnEnable.PlayFirstSongEvent += OnPlayFirstSongEvent;
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

    private void OnPlayFirstSongEvent()
    {
        PlayNextSong();
    }

    private void InitializeStickAnimator()
    {
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Tutorial"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Rhythm"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Chord"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Play"), 1);
    }

    private void ShowInbetweenSequence()
    {
        sequenceAnimator.SetBool(SHOW_INBETWEEN_HASH, true);
    }

    private void ShowOutroSequence()
    {
        sequenceAnimator.SetBool(SHOW_OUTRO_HASH, true);
        StartCoroutine(GoBackToMainMenuAfterSeconds(PROTOCOL_ENDED_DELAY));
    }

    private void PlayNextSong()
    {
        songCount++;
        musicClipManager.StartSong();
    }

    private IEnumerator GoBackToMainMenuAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("MainMenu");
    }
}
