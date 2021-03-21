using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SustainedProtocolScreen : MonoBehaviour
{
    private const float PROTOCOL_ENDED_DELAY = 2f;

    [SerializeField] private MusicClipManager musicClipManager;
    [SerializeField] private StickFigure stickFigure;
    [SerializeField] private Animator stickFigureAnimator;

    protected void Awake()
    {
        InitializeStickAnimator();
        musicClipManager.SequenceEndedEvent += OnSequenceEndedEvent;
    }

    protected void OnDestroy()
    {
        musicClipManager.SequenceEndedEvent -= OnSequenceEndedEvent;
    }

    private void OnSequenceEndedEvent()
    {
        stickFigure.StopRocking();
        StartCoroutine(GoBackToMainMenuAfterSeconds(PROTOCOL_ENDED_DELAY));
    }

    private void InitializeStickAnimator()
    {
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Tutorial"), 0);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Rhythm"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Chord"), 1);
        stickFigureAnimator.SetLayerWeight(stickFigureAnimator.GetLayerIndex("Play"), 1);
    }

    private IEnumerator GoBackToMainMenuAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("MainMenu");
    }
}
