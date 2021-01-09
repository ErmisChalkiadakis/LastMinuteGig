using UnityEngine;

public class ClipSetSequence : ScriptableObject
{
    public int SequenceLength => clipSets.Length;

    [SerializeField] private ClipSet[] clipSets;

    public ClipSet getClipSetAtIndex(int index)
    {
        if (clipSets.Length > index)
        {
            return clipSets[index];
        }

        return new ClipSet();
    }
}
