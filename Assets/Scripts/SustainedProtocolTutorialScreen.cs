using System;
using UnityEngine;

public class SustainedProtocolTutorialScreen : MonoBehaviour
{
    private static int SHOW_HASH = Animator.StringToHash("Show");

    [SerializeField] private AnimatorStateObserver animatorStateObserver;
    [SerializeField] private Animator fadeToBlackAnimator;
    [SerializeField] private TextBox textBox;
    [SerializeField] private Arrow arrow;
    [SerializeField] private string[] tutorialText;

    private int index = 0;

    protected void Awake()
    {
        fadeToBlackAnimator.SetBool(SHOW_HASH, true);
        animatorStateObserver.AnimatorStateEnteredEvent += OnAnimatorStateEnteredEvent;
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
        textBox.ShowText(tutorialText[0]);
    }

    private void OnTextShownEvent()
    {
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
        if (index < tutorialText.Length)
        {
            textBox.TextShownEvent += OnTextShownEvent;
            textBox.ShowText(tutorialText[index]);
        }

    }
}
