using UnityEngine;

public class ClipSetSequence : ScriptableObject
{
    [SerializeField] private ClipSet[] clipSets;

    public ClipSet getClipSetAtIndex(int index)
    {
        if (clipSets.Length >= index)
        {
            return clipSets[index];
        }

        Debug.LogError($"Index out of bounds");
        return new ClipSet();
    }
}
