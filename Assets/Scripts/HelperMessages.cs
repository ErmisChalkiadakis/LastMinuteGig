using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HelperMessages : MonoBehaviour
{
    private static int SHOW_HASH = Animator.StringToHash("Show");
    private static int SHOW_ARROW_HASH = Animator.StringToHash("ShowArrow");

    private const float DELAY_UNTIL_MESSAGE = 2f;

    [SerializeField] private InstrumentButton instrumentButton;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Animator messageAnimator;

    private bool started;
    private bool buttonPressed;
    private double lastButtonPress;

    protected void Awake()
    {
        messageAnimator.SetBool(SHOW_HASH, false);
        instrumentButton.ButtonSelectedEvent += OnButtonSelectedEvent;
    }

    protected void OnDestroy()
    {
        instrumentButton.ButtonSelectedEvent -= OnButtonSelectedEvent;
    }

    protected void Update()
    {
        if (started && AudioSettings.dspTime > lastButtonPress + DELAY_UNTIL_MESSAGE)
        {
            if (!buttonPressed)
            {
                textMesh.text = "Click <b>this</b> button to play along";
                messageAnimator.SetBool(SHOW_HASH, true);
            }
            else
            {
                textMesh.text = "Try to play along with the music";
                messageAnimator.SetBool(SHOW_HASH, true);
            }
        }
    }

    public void StartMessages()
    {
        started = true;
        lastButtonPress = AudioSettings.dspTime;
    }

    private void OnButtonSelectedEvent(ButtonID buttonId)
    {
        buttonPressed = true;
        lastButtonPress = AudioSettings.dspTime;
        StartCoroutine(HideButtonAfterSeconds(2f));
    }

    private IEnumerator HideButtonAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);

        messageAnimator.SetBool(SHOW_ARROW_HASH, false);
        messageAnimator.SetBool(SHOW_HASH, false);
    }
}
