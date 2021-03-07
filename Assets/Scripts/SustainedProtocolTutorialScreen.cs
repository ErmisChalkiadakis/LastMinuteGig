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

    [SerializeField] private AnimatorStateObserver animatorStateObserver;
    [SerializeField] private Animator fadeToBlackAnimator;
    [SerializeField] private Animator stickFigureAnimator;
    [SerializeField] private TextBox textBox;
    [SerializeField] private Arrow arrow;
    [SerializeField] private TutorialStep[] tutorialSteps;

    private int index = 0;

    protected void Awake()
    {
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

    private IEnumerator PlayAnimationAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);

        stickFigureAnimator.SetTrigger(NEXT_STEP_HASH);
    }
}
