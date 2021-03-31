using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SustainedProtocolScreen : MonoBehaviour
{
    private static int SHOW_INBETWEEN_HASH = Animator.StringToHash("ShowInbetween");
    private static int SHOW_OUTRO_HASH = Animator.StringToHash("ShowOutro");
    private static int BOW_HASH = Animator.StringToHash("Bow");
    private static int DEEP_BOW_HASH = Animator.StringToHash("DeepBow");

    private const float PROTOCOL_ENDED_DELAY = 2f;

    [SerializeField] private MusicClipManager musicClipManager;
    [SerializeField] private StickFigure stickFigure;
    [SerializeField] private Animator stickFigureAnimator;
    [SerializeField] private Animator sequenceAnimator;
    [SerializeField] private AnimatorStateObserver sequenceAnimatorStateObserver;
    [SerializeField] private PlayFirstSongOnEnable playSongOnEnable;
    [SerializeField] private BowOnEnable bowOnEnable;
    [SerializeField] private BowOnEnable deepBowOnEnable;
    [SerializeField] private int numberOfTotalSongs = 4;

    private int songCount = 0;

    protected void Awake()
    {
        InitializeStickAnimator();
        musicClipManager.SequenceEndedEvent += OnSequenceEndedEvent;
        playSongOnEnable.PlaySongEvent += OnPlaySongEvent;
        bowOnEnable.BowEvent += OnBowEvent;
        bowOnEnable.BowEndedEvent += OnBowEndedEvent;
        deepBowOnEnable.BowEvent += OnDeepBowEvent;
        deepBowOnEnable.BowEndedEvent += OnDeepBowEndedEvent;
        sequenceAnimatorStateObserver.AnimatorStateEnteredEvent += OnAnimatorStateEnteredEvent;
    }

    protected void OnDestroy()
    {
        musicClipManager.SequenceEndedEvent -= OnSequenceEndedEvent;
        playSongOnEnable.PlaySongEvent -= OnPlaySongEvent;
        bowOnEnable.BowEvent -= OnBowEvent;
        bowOnEnable.BowEndedEvent -= OnBowEndedEvent;
        deepBowOnEnable.BowEvent -= OnDeepBowEvent;
        deepBowOnEnable.BowEndedEvent -= OnDeepBowEndedEvent;
        sequenceAnimatorStateObserver.AnimatorStateEnteredEvent -= OnAnimatorStateEnteredEvent;
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

    private void OnPlaySongEvent()
    {
        PlayNextSong();
    }

    private void OnBowEvent()
    {
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Tutorial"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Rhythm"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Chord"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Play"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Bow"), 1);

        stickFigureAnimator.SetTrigger(BOW_HASH);
    }

    private void OnBowEndedEvent()
    {
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Tutorial"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Rhythm"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Chord"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Play"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Bow"), 0);
    }

    private void OnDeepBowEvent()
    {

        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Tutorial"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Rhythm"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Chord"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Play"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Bow"), 1);

        stickFigureAnimator.SetTrigger(DEEP_BOW_HASH);
    }

    private void OnDeepBowEndedEvent()
    {
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Tutorial"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Rhythm"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Chord"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Play"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Bow"), 0);
    }

    private void OnAnimatorStateEnteredEvent(string state)
    {
        if (state == "IdleOut")
        {
            GoBackToMainMenu();
        }
    }

    private void InitializeStickAnimator()
    {
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Tutorial"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Rhythm"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Chord"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Play"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Bow"), 0);
    }

    private void ShowInbetweenSequence()
    {
        sequenceAnimator.SetTrigger(SHOW_INBETWEEN_HASH);
    }

    private void ShowOutroSequence()
    {
        sequenceAnimator.SetBool(SHOW_OUTRO_HASH, true);
    }

    private void PlayNextSong()
    {
        songCount++;
        musicClipManager.StartSong();
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
