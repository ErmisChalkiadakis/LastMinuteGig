using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMusicClipManager : MonoBehaviour
{
    [Serializable]
    private class TutorialClip
    {
        public Rhythm Rhythm => rhythm;
        public Tempo Tempo => tempo;

        [SerializeField] private Rhythm rhythm;
        [SerializeField] private Tempo tempo;
    }

    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private MusicClipInputManager inputManager;
    [SerializeField] private PercussionMusicClipLibrary percussionClipLibrary;
    [SerializeField] private InputMusicClipLibrary inputClipLibrary;
    [SerializeField] private int loopCount;
    [SerializeField] private TutorialClip[] tutorialClips;

    private List<MusicClip> musicClips;

    protected void Awake()
    {
        GenerateTutorialClips();
    }

    protected void OnDestroy()
    {
        musicClips.Clear();
    }

    public void StartTutorialClips()
    {
        for (int k = 0; k < loopCount; k++)
        {
            for (int i = 0; i < musicClips.Count; i++)
            {
                musicMixer.QueueClip(musicClips[i]);
            }
        }
    }

    private void GenerateTutorialClips()
    {
        musicClips = new List<MusicClip>();
        for (int i = 0; i < tutorialClips.Length; i++)
        {
            musicClips.Add(
                new MusicClip(
                    percussionClipLibrary.GetRandomClipWithRhythmAndTempo(tutorialClips[i].Rhythm, tutorialClips[i].Tempo),
                    inputClipLibrary.GetClipWithInstrumentAndChord(Instrument.ElectricGuitar, Chord.Cm),
                    null)
                );
        }
    }
}
