using UnityEngine;
using Random = System.Random;

public class InputMusicClipLibrary : ScriptableObject
{
    [SerializeField] private InputMusicClip[] inputMusicClips;

    public InputMusicClip GetRandomClip()
    {
        if (inputMusicClips.Length > 0)
        {
            Random random = new Random();

            int randomIndex = random.Next(inputMusicClips.Length);
            return inputMusicClips[randomIndex];
        }

        Debug.LogError($"InputMusicClipLibrary is empty.");
        return null;
    }

    public InputMusicClip GetClipWithInstrumentAndChord(Instrument instrument, Chord chord)
    {
        foreach (var inputMusicClip in inputMusicClips)
        {
            if (inputMusicClip.Instrument == instrument && inputMusicClip.Chord == chord)
            {
                return inputMusicClip;
            }
        }

        Debug.LogError($"No clip found with Instrument: {instrument} and Chord: {chord}");
        return null;
    }
}
