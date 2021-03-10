using System;
using System.Collections;
using UnityEngine;

public class SustainedProtocolTutorialScreen : MonoBehaviour
{
    private static int NEXT_STEP_HASH = Animator.StringToHash("NextStep");
    private static int SHOW_HASH = Animator.StringToHash("Show");
    private static int TUTORIAL_LAYER_INDEX;
    private static int PLAY_LAYER_INDEX;
    private static int CHORD_LAYER_INDEX;
    private static int ROCK_LAYER_INDEX;

    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private TutorialMusicClipManager tutorialMusicClipManager;
    [SerializeField] private AnimatorStateObserver animatorStateObserver;
    [SerializeField] private Animator fadeToBlackAnimator;
    [SerializeField] private Animator stickFigureAnimator;
    [SerializeField] private NoteTimeline noteTimeline;
    [SerializeField] private Animator noteTimelineAnimator;
    [SerializeField] private Animator textGroupAnimator;
    [SerializeField] private AnimatorStateObserver textGroupAnimatorStateObserver;
    [SerializeField] private InstrumentButton instrumentButton;
    [SerializeField] private TextBox textBox;
    [SerializeField] private Arrow arrow;
    [SerializeField] private float hideTimelineDelay = 60f;
    [SerializeField] private TutorialStep[] tutorialSteps;

    private int index = 0;

    protected void Awake()
    {
        StartTextTutorial();
        fadeToBlackAnimator.SetBool(SHOW_HASH, true);
        animatorStateObserver.AnimatorStateEnteredEvent += OnAnimatorStateEnteredEvent;
        InitializeStickAnimator();
    }

    private void OnAnimatorStateEnteredEvent(string state)
    {
        if (state == "Transparent")
        {
            StartSequence();
        }
    }

    private void StartSequence()
    {
        textBox.TextShownEvent += OnTextShownEvent;
        textBox.ShowText(tutorialSteps[0].Text);
    }

    private void OnTextShownEvent()
    {
        Debug.Log($"Showed Text: {tutorialSteps[index].Text}");
        textBox.TextShownEvent -= OnTextShownEvent;
        index++;
        arrow.ShowArrow(true);
        arrow.ArrowClickedEvent += OnArrowClickedEvent;
    }

    private void OnArrowClickedEvent()
    {
        arrow.ArrowClickedEvent -= OnArrowClickedEvent;
        arrow.ShowArrow(false);
        textBox.ClearText();
        if (index < tutorialSteps.Length)
        {
            textBox.TextShownEvent += OnTextShownEvent;
            textBox.ShowText(tutorialSteps[index].Text, tutorialSteps[index].ShowTextAfterSeconds);
            if (tutorialSteps[index].PlayNextAnimation)
            {
                StartCoroutine(PlayAnimationAfterSeconds(tutorialSteps[index].PlayAnimationAfterSeconds));
            }
        }
        else
        {
            StartNoteTimelineTutorialTransition();
        }
    }

    private void InitializeStickAnimator()
    {
        TUTORIAL_LAYER_INDEX = stickFigureAnimator.GetLayerIndex("Tutorial");
        ROCK_LAYER_INDEX = stickFigureAnimator.GetLayerIndex("Rhythm");
        CHORD_LAYER_INDEX = stickFigureAnimator.GetLayerIndex("Chord");
        PLAY_LAYER_INDEX = stickFigureAnimator.GetLayerIndex("Play");

        stickFigureAnimator.SetLayerWeight(TUTORIAL_LAYER_INDEX, 1);
        stickFigureAnimator.SetLayerWeight(ROCK_LAYER_INDEX, 0);
        stickFigureAnimator.SetLayerWeight(CHORD_LAYER_INDEX, 0);
        stickFigureAnimator.SetLayerWeight(PLAY_LAYER_INDEX, 0);
    }

    private void StartTextTutorial()
    {
        textGroupAnimator.SetBool(SHOW_HASH, true);
        instrumentButton.gameObject.SetActive(false);
        noteTimeline.gameObject.SetActive(false);
        musicMixer.gameObject.SetActive(false);
    }

    private void StartNoteTimelineTutorialTransition()
    {
        textGroupAnimator.SetBool(SHOW_HASH, false);
        textGroupAnimatorStateObserver.AnimatorStateEnteredEvent += OnTextGroupAnimatorStateEnteredEvent;
    }

    private void OnTextGroupAnimatorStateEnteredEvent(string state)
    {
        if (state == "IdleOut")
        {
            StartNoteTimelineTutorial();
        }
    }

    private void StartNoteTimelineTutorial()
    {
        textGroupAnimatorStateObserver.AnimatorStateEnteredEvent -= OnTextGroupAnimatorStateEnteredEvent;
        instrumentButton.gameObject.SetActive(true);
        noteTimeline.gameObject.SetActive(true);
        arrow.gameObject.SetActive(false);
        textBox.gameObject.SetActive(false);
        noteTimelineAnimator.SetBool(SHOW_HASH, true);
        StartCoroutine(HideNoteTimelineAfterSeconds(hideTimelineDelay));
        musicMixer.gameObject.SetActive(true);
        tutorialMusicClipManager.StartTutorialClips();
    }

    private IEnumerator PlayAnimationAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);

        stickFigureAnimator.SetTrigger(NEXT_STEP_HASH);
    }

    private IEnumerator HideNoteTimelineAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        noteTimelineAnimator.SetBool(SHOW_HASH, false);
    }
}
