using UnityEngine;

public class LayerMusicClipLibrary : ScriptableObject
{
    [SerializeField] private LayerMusicClip[] layerMusicClips;

    public LayerMusicClip GetClipWithInstrumentAndChord(Instrument instrument, Chord chord)
    {
        foreach (LayerMusicClip clip in layerMusicClips)
        {
            if (clip.Instrument == instrument && clip.Chord == chord)
            {
                return clip;
            }
        }

        Debug.LogError($"No layer clip found with instrument: {instrument} and chord: {chord}");
        return null;
    }

    public LayerMusicClip GetClipWithName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        foreach (var layerMusicClip in layerMusicClips)
        {
            if (layerMusicClip.Name == name)
            {
                return layerMusicClip;
            }
        }

        Debug.LogError($"No clip found with name: {name}");
        return null;
    }
}
