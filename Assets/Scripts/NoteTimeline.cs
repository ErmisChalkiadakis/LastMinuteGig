using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteTimeline : MonoBehaviour
{
    [SerializeField] private RectTransform noteLine;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private GameObject changeIndicatorPrefab;
    [SerializeField] private GameObject separatorLinePrefab;
    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private float widthPerSecond = 350f;
    [SerializeField] private Color noteColor1;
    [SerializeField] private Color noteColor2;
    [SerializeField] private Color tempoChangeColor;
    [SerializeField] private Color rhythmChangeColor;
    [SerializeField] private PercussionMusicClipLibrary percussionLibrary;
    [SerializeField] private InputMusicClipLibrary inputLibrary;

    private bool firstClipQueued;
    private bool colorFlip;
    private MusicClip cachedClip;

    protected void OnEnable()
    {
        musicMixer.ClipQueuedEvent += OnClipQueuedEvent;
    }

    protected void OnDisable()
    {
        musicMixer.ClipQueuedEvent -= OnClipQueuedEvent;
    }

    protected void Update()
    {
        if (firstClipQueued && AudioSettings.dspTime > musicMixer.StartTime)
        {
            noteLine.anchoredPosition = new Vector2(noteLine.anchoredPosition.x - Time.deltaTime * widthPerSecond, noteLine.anchoredPosition.y);
        }
    }

    private void OnClipQueuedEvent(MusicClip queuedClip, double startingTime)
    {
        if (!firstClipQueued)
        {
            firstClipQueued = true;
        }

        HandleMusicalChange(queuedClip, startingTime);

        AddBar(startingTime, queuedClip.Duration);

        if (cachedClip != null)
        {
            InstantiateSeparatorLine(startingTime);
        }

        cachedClip = queuedClip;
    }

    private void AddBar(double time, double duration)
    {
        double clipTime = time - musicMixer.StartTime;
        GameObject go = GameObject.Instantiate(barPrefab, noteLine.transform);
        float timeToDestroy = (float)(time - AudioSettings.dspTime);
        GameObject.Destroy(go, timeToDestroy + 3f);
        RectTransform barRectTransform = go.GetComponent<RectTransform>();
        barRectTransform.sizeDelta = new Vector2((float)(widthPerSecond * duration), barRectTransform.sizeDelta.y);
        barRectTransform.anchoredPosition = new Vector2((float)(widthPerSecond * clipTime), barRectTransform.anchoredPosition.y);
        RawImage image = go.GetComponent<RawImage>();
        image.color = colorFlip ? noteColor1 : noteColor2;
    }

    private void HandleMusicalChange(MusicClip nextClip, double time)
    {
        if (cachedClip == null)
        {
            return;
        }

        if (cachedClip.PercussionClip.Tempo != nextClip.PercussionClip.Tempo)
        {
            colorFlip = !colorFlip;
            InstantiateChangeIndicator(tempoChangeColor, $"Tempo Change", time);
        }
        else if (cachedClip.PercussionClip.Rhythm != nextClip.PercussionClip.Rhythm)
        {
            colorFlip = !colorFlip;
            InstantiateChangeIndicator(rhythmChangeColor, $"Rhythm Change", time);
        }
    }

    private void InstantiateSeparatorLine(double time)
    {
        float timeToDestroy = (float)(time - AudioSettings.dspTime);
        GameObject go = GameObject.Instantiate(separatorLinePrefab, noteLine.transform);
        GameObject.Destroy(go, timeToDestroy + 3);
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2((float)(widthPerSecond * (time - musicMixer.StartTime)), rectTransform.anchoredPosition.y);
    }

    private void InstantiateChangeIndicator(Color color, string text, double time)
    {
        float timeToDestroy = (float)(time - AudioSettings.dspTime);
        GameObject go = GameObject.Instantiate(changeIndicatorPrefab, noteLine.transform);
        GameObject.Destroy(go, timeToDestroy + 3);
        RawImage image = go.GetComponent<RawImage>();
        image.color = color;
        TextMeshProUGUI textMesh = go.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = text;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2((float)(widthPerSecond * (time - musicMixer.StartTime)), rectTransform.anchoredPosition.y);
    }
}
