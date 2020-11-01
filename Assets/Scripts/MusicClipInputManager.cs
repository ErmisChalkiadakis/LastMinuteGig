using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClipInputManager : MonoBehaviour
{
    internal class InputWindow
    {
        private double min;
        private double max;

        public InputWindowResults Results { private set; get; }

        public ButtonID ButtonID { private set; get; }

        public InputWindow(double min, double max, ButtonID instrumentButton)
        {
            this.min = min;
            this.max = max;
            ButtonID = instrumentButton;

            Results = new InputWindowResults();
        }

        public bool IsInWindow(double time)
        {
            return time > min && time < max;
        }

        public void ButtonSelected(ButtonID buttonId)
        {
            if (!Results.AnyButtonSelected)
            {
                Results.FirstButtonSelected = buttonId;
            }
            if (ButtonID == buttonId)
            {
                Results.CorrectButtonSelected = true;
            }

            Results.AnyButtonSelected = true;
        }
    }

    private const float CLIP_CHANGE_WINDOW = 0.1f;

    public delegate void ClipInputFinalizedHandler(MusicClipResults clipResults);
    public event ClipInputFinalizedHandler ClipInputFinalizedEvent;

    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private InstrumentButton middleButton;
    [SerializeField] private InstrumentButton upButton;
    [SerializeField] private InstrumentButton downButton;
    [SerializeField, Range(0f, 0.1f)] private float inputWindowLeniency = 0.05f; // in seconds

    private List<InputWindow> currentInputWindows;
    private List<InputWindow> nextInputWindows;

    private InputWindow activeInputWindow;
    private MusicClipResults activeClipResults = new MusicClipResults(); 
    
    protected void Awake()
    {
        musicMixer.ClipScheduledEvent += OnClipScheduledEvent;

        middleButton.ButtonSelectedEvent += OnMiddleButtonSelectedEvent;
        //upButton.ButtonSelectedEvent += OnUpButtonSelectedEvent;
        //downButton.ButtonSelectedEvent += OnDownButtonSelectedEvent;

        currentInputWindows = new List<InputWindow>();
        nextInputWindows = new List<InputWindow>();
    }

    protected void OnDestroy()
    {
        musicMixer.ClipScheduledEvent -= OnClipScheduledEvent;

        middleButton.ButtonSelectedEvent -= OnMiddleButtonSelectedEvent;
        //upButton.ButtonSelectedEvent -= OnUpButtonSelectedEvent;
        //downButton.ButtonSelectedEvent -= OnDownButtonSelectedEvent;
    }

    protected void Update()
    {
        UpdateInputWindows();
    }

    private void UpdateInputWindows()
    {
        foreach (InputWindow inputWindow in currentInputWindows)
        {
            if (inputWindow.IsInWindow(AudioSettings.dspTime))
            {
                if (activeInputWindow != inputWindow)
                {
                    EnableWindow(inputWindow);
                }
            }
            else
            {
                if (activeInputWindow == inputWindow)
                {
                    CloseWindow(inputWindow);
                }
            }
        }
    }

    private void EnableWindow(InputWindow inputWindow)
    {
        activeInputWindow = inputWindow;
    }

    private void CloseWindow(InputWindow inputWindow)
    {
        activeClipResults.InputWindowResults.Add(inputWindow.Results);
        activeInputWindow = null;
    }
    
    private void ClipFinishedCleanup()
    {
        ClipInputFinalizedEvent?.Invoke(activeClipResults);
        activeClipResults.InputWindowResults.Clear();
    }

    private void OnClipScheduledEvent(MusicClip scheduledClip, double startingTime)
    {
        nextInputWindows.Clear();
        foreach (ButtonTiming buttonTiming in scheduledClip.PercussionClip.ButtonTimings)
        {
            double min = startingTime + (scheduledClip.Duration * buttonTiming.Timing) - inputWindowLeniency;
            double max = Mathf.Min((float)(startingTime + (scheduledClip.Duration * buttonTiming.Timing) + inputWindowLeniency), (float)(startingTime + scheduledClip.Duration));
            nextInputWindows.Add(new InputWindow(min, max, buttonTiming.ButtonID));
        }

        double endingTime = startingTime + scheduledClip.Duration;

        StartCoroutine(ClipStarting(scheduledClip, startingTime));
        StartCoroutine(ClipFinished(endingTime));
    }

    private IEnumerator ClipStarting(MusicClip scheduledClip, double scheduledTime)
    {
        while(AudioSettings.dspTime < scheduledTime - CLIP_CHANGE_WINDOW)
        {
            yield return null;
        }

        currentInputWindows = nextInputWindows;
        middleButton.SetInstrumentSounds(scheduledClip.InputClip.AudioClips);
        //upButton.SetInstrumentSound(scheduledClip.UpButtonInstrumentSound);
        //downButton.SetInstrumentSound(scheduledClip.DownButtonInstrumentSound);
    }

    private IEnumerator ClipFinished(double endingTime)
    {
        yield return new WaitForSecondsRealtime((float)(endingTime - AudioSettings.dspTime));

        ClipFinishedCleanup();
    }
    
    private void OnMiddleButtonSelectedEvent(ButtonID buttonId)
    {
        activeInputWindow?.ButtonSelected(buttonId);
    }

    private void OnUpButtonSelectedEvent(ButtonID buttonId)
    {
        activeInputWindow?.ButtonSelected(buttonId);
    }

    private void OnDownButtonSelectedEvent(ButtonID buttonId)
    {
        activeInputWindow?.ButtonSelected(buttonId);
    }
}
