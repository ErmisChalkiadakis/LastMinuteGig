using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteTimeline : MonoBehaviour
{
    private const float REMOVAL_WIDTH = 100f;

    [SerializeField] private RectTransform noteLine;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject changeIndicatorPrefab;
    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private float widthPerSecond = 350f;
    [SerializeField] private Color noteColor1;
    [SerializeField] private Color noteColor2;
    [SerializeField] private Color tempoChangeColor;
    [SerializeField] private Color rhythmChangeColor;
    [SerializeField] private PercussionMusicClipLibrary percussionLibrary;
    [SerializeField] private InputMusicClipLibrary inputLibrary;

    private double startTime;
    private bool flip;
    private MusicClip cachedClip;

    protected void OnEnable()
    {
        startTime = AudioSettings.dspTime;
        musicMixer.ClipScheduledEvent += OnClipScheduledEvent;
        MusicClip clip96B = new MusicClip(
            percussionLibrary.GetRandomClipWithRhythmAndTempo(Rhythm.ThreeRest, Tempo.t96),
            inputLibrary.GetClipWithInstrumentAndChord(Instrument.ElectricGuitar, Chord.Cm),
            null);
        MusicClip clip120B = new MusicClip(
            percussionLibrary.GetRandomClipWithRhythmAndTempo(Rhythm.ThreeRest, Tempo.t120),
            inputLibrary.GetClipWithInstrumentAndChord(Instrument.ElectricGuitar, Chord.Cm),
            null);
        MusicClip clip96A = new MusicClip(
            percussionLibrary.GetRandomClipWithRhythmAndTempo(Rhythm.EightEight, Tempo.t96),
            inputLibrary.GetClipWithInstrumentAndChord(Instrument.ElectricGuitar, Chord.Cm),
            null); 
        MusicClip clip120A = new MusicClip(
             percussionLibrary.GetRandomClipWithRhythmAndTempo(Rhythm.EightEight, Tempo.t120),
             inputLibrary.GetClipWithInstrumentAndChord(Instrument.ElectricGuitar, Chord.Cm),
             null);
        musicMixer.QueueClip(clip96B);
        musicMixer.QueueClip(clip120B);
        musicMixer.QueueClip(clip120A);
        musicMixer.QueueClip(clip96A);
        musicMixer.QueueClip(clip96B);
        musicMixer.QueueClip(clip96B);
    }

    protected void OnDisable()
    {
        musicMixer.ClipScheduledEvent -= OnClipScheduledEvent;
    }

    protected void Update()
    {
        noteLine.anchoredPosition = new Vector2(noteLine.anchoredPosition.x - Time.deltaTime * widthPerSecond, noteLine.anchoredPosition.y);
    }

    private void OnClipScheduledEvent(MusicClip scheduledClip, double startingTime)
    {
        ButtonTiming[] buttonTimings = scheduledClip.PercussionClip.ButtonTimings;
        HandleMusicalChange(scheduledClip, startingTime);

        foreach (ButtonTiming timing in buttonTimings)
        {
            AddNote(startingTime + scheduledClip.Duration * timing.Timing);
        }

        cachedClip = scheduledClip;
        flip = !flip;
    }

    private void AddNote(double time)
    {
        double clipTime = time - startTime;
        GameObject go = GameObject.Instantiate(notePrefab, noteLine.transform);
        float timeToDestroy = (float)(time - AudioSettings.dspTime);
        GameObject.Destroy(go, timeToDestroy + 1f);
        RectTransform noteRectTransform = go.GetComponent<RectTransform>();
        noteRectTransform.anchoredPosition = new Vector2((float)(widthPerSecond * clipTime), noteRectTransform.anchoredPosition.y);
        RawImage image = go.GetComponent<RawImage>();
        image.color = flip ? noteColor1 : noteColor2;
    }

    private void HandleMusicalChange(MusicClip nextClip, double time)
    {
        if (cachedClip == null)
        {
            return;
        }

        if (cachedClip.PercussionClip.Tempo != nextClip.PercussionClip.Tempo)
        {
            InstantiateChangeIndicator(tempoChangeColor, $"Tempo Change", time);
        }
        else if (cachedClip.PercussionClip.Rhythm != nextClip.PercussionClip.Rhythm)
        {
            InstantiateChangeIndicator(rhythmChangeColor, $"Rhythm Change", time);
        }
    }

    private void InstantiateChangeIndicator(Color color, string text, double time)
    {
        float timeToDestroy = (float)(time - AudioSettings.dspTime);
        GameObject go = GameObject.Instantiate(changeIndicatorPrefab, noteLine.transform);
        GameObject.Destroy(go, timeToDestroy + 1);
        RawImage image = go.GetComponent<RawImage>();
        image.color = color;
        TextMeshProUGUI textMesh = go.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = text;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2((float)(widthPerSecond * (time - startTime)), rectTransform.anchoredPosition.y);
    }
}
