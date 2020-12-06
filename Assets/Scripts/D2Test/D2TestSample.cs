using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class D2TestSample : MonoBehaviour
{
    public delegate void TestSampleFinishedHandler(D2TestSampleResults results);
    public TestSampleFinishedHandler TestSampleFinishedEvent;

    [SerializeField] private TextMeshProUGUI letterField;
    [SerializeField] private TextMeshProUGUI upLineField;
    [SerializeField] private TextMeshProUGUI downLineField;
    [SerializeField] private RawImage fadeImage;
    [SerializeField] private D2TestButton confirmButton;
    [SerializeField] private SampleTimer sampleTimer;
    [SerializeField] private float fadeTime = .25f;

    private D2TestSampleResults activeTestSampleResults;

    protected void Awake()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
    }

    public void StartTest(D2TestCase testCase)
    {
        GenerateSampleFromTestCase(testCase);
        StartCoroutine(FadeInAndStart());
    }

    private void OnTimerFinishedEvent()
    {
        TestCaseFinished();
    }

    private void OnButtonPressedEvent()
    {
        activeTestSampleResults.ButtonPressed(sampleTimer.TimeElapsed);
        TestCaseFinished();
    }

    private void TestCaseFinished()
    {
        StartCoroutine(FadeOutAndFinishTestCase());
    }

    private void ResetLines()
    {
        upLineField.text = "";
        downLineField.text = "";
    }

    private void GenerateSampleFromTestCase(D2TestCase testCase)
    {
        ResetLines();
        activeTestSampleResults = new D2TestSampleResults(testCase);
        Random random = new Random();
        int lineCount;

        switch (testCase)
        {
            case D2TestCase.CorrectD:
                letterField.text = "d";
                lineCount = 2;
                AddLines(lineCount);
                break;
            case D2TestCase.WrongD:
                letterField.text = "d";
                lineCount = (int)random.Next(2, 5);
                if (lineCount == 2)
                {
                    lineCount = 1;
                }
                AddLines(lineCount);
                break;
            case D2TestCase.WrongP:
                letterField.text = "p";
                lineCount = (int)random.Next(1, 3);
                AddLines(lineCount);
                break;
        }
    }

    private void AddLines(int count)
    {
        Random random = new Random();
        int upCount = 0;
        int downCount = 0;
        for (int i = 0; i < count; i++)
        {
            if ((int)random.Next(0, 2) > 0)
            {
                if (upCount == 2)
                {
                    AddLineToField(downLineField);
                    downCount++;
                }
                else
                {
                    AddLineToField(upLineField);
                    upCount++;
                }
            }
            else
            {
                if (downCount == 2)
                {
                    AddLineToField(upLineField);
                    upCount++;
                }
                else
                {
                    AddLineToField(downLineField);
                    downCount++;
                }
            }
        }
    }

    private void AddLineToField(TextMeshProUGUI field)
    {
        if (field.text == "")
        {
            field.text = "|";
        }
        else
        {
            field.text = "| |";
        }
    }

    private IEnumerator FadeInAndStart()
    {
        Color imageColor = fadeImage.color;
        imageColor.a = 1;
        double startTime = AudioSettings.dspTime;
        double endTime = startTime + fadeTime;
        double timeElapsed;
        float progress;

        while (AudioSettings.dspTime < endTime)
        {
            timeElapsed = AudioSettings.dspTime - startTime;
            progress = (float)(timeElapsed / fadeTime);
            imageColor.a = 1f - progress;
            fadeImage.color = imageColor;
            yield return null;
        }
        imageColor.a = 0f;
        fadeImage.color = imageColor;

        sampleTimer.StartTimer();
        sampleTimer.TimerFinishedEvent += OnTimerFinishedEvent;
        confirmButton.ButtonPressedEvent += OnButtonPressedEvent;
    }

    private IEnumerator FadeOutAndFinishTestCase()
    {
        sampleTimer.StopTimer();
        sampleTimer.TimerFinishedEvent -= OnTimerFinishedEvent;
        confirmButton.ButtonPressedEvent -= OnButtonPressedEvent;

        Color imageColor = fadeImage.color;
        imageColor.a = 1;
        double startTime = AudioSettings.dspTime;
        double endTime = startTime + fadeTime;
        double timeElapsed;
        float progress;

        while (AudioSettings.dspTime < endTime)
        {
            timeElapsed = AudioSettings.dspTime - startTime;
            progress = (float)(timeElapsed / fadeTime);
            imageColor.a = progress;
            fadeImage.color = imageColor;
            yield return null;
        }
        imageColor.a = 1f;
        fadeImage.color = imageColor;

        yield return new WaitForSeconds(0.1f);

        ResetLines();
        activeTestSampleResults = null; 
        TestSampleFinishedEvent?.Invoke(activeTestSampleResults);
    }
}
