using UnityEngine;
using UnityEngine.UI;

public class SampleTimer : MonoBehaviour
{
    public const float GRAPHIC_LENGTH = 700f;
    public const float STANDARD_TIMER = 2f;

    public delegate void TimerFinishedHandler();
    public event TimerFinishedHandler TimerFinishedEvent;

    public double TimeElapsed => timerActive ? AudioSettings.dspTime - startTime : 0f;

    [SerializeField] private RawImage timeLeftImage;
    [SerializeField] private Color plentyOfTimeColor;
    [SerializeField] private Color noTimeColor;

    private bool timerActive;
    private double startTime;
    private double endTime = double.MaxValue;

    protected void Awake()
    {
        timeLeftImage.color = plentyOfTimeColor;
    }

    protected void Update()
    {
        if (timerActive)
        {
            UpdateTimer();
            if (AudioSettings.dspTime > endTime)
            {
                TimerFinished();
            }
        }
    }

    public void StartTimer()
    {
        StartTimer(STANDARD_TIMER);
    }

    public void StartTimer(float timerDuration)
    {
        timerActive = true;
        startTime = AudioSettings.dspTime;
        endTime = startTime + timerDuration;
    }

    public void StopTimer()
    {
        timerActive = false;
        ResetTimer();
    }

    private void UpdateTimer()
    {
        double timeLeft = AudioSettings.dspTime - startTime;
        double timerDuration = endTime - startTime;
        float progress = (float)(timeLeft / timerDuration);
        timeLeftImage.rectTransform.anchoredPosition = new Vector3(0f - GRAPHIC_LENGTH * progress, 0f, 0f);

        if (progress > 0.5f)
        {
            progress -= 0.5f;
            float truncatedProgress = progress / 0.5f;
            timeLeftImage.color = InterpolateColors(plentyOfTimeColor, noTimeColor, truncatedProgress);
        }
    }

    private void TimerFinished()
    {
        timerActive = false;
        TimerFinishedEvent?.Invoke();
    }

    private void ResetTimer()
    {
        endTime = double.MaxValue;
        timeLeftImage.color = plentyOfTimeColor;
        timeLeftImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    private Color InterpolateColors(Color startColor, Color endColor, float progress)
    {
        float r = (endColor.r - startColor.r) * progress + startColor.r;
        float g = (endColor.g - startColor.g) * progress + startColor.g;
        float b = (endColor.b - startColor.b) * progress + startColor.b;
        return new Color(r, g, b);
    }
}
