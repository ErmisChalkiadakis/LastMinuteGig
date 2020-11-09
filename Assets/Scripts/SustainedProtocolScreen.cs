using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SustainedProtocolScreen : MonoBehaviour
{
    private const float PROTOCOL_ENDED_DELAY = 2f;

    [SerializeField] private MusicClipManager musicClipManager;
    [SerializeField] private StickFigure stickFigure;

    protected void Awake()
    {
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

    private IEnumerator GoBackToMainMenuAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("MainMenu");
    }
}
