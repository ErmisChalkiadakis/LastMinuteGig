using System;
using UnityEngine;

public class PlaySongOnEnable : MonoBehaviour
{
    public event Action PlayFirstSongEvent = delegate { };

    protected void OnEnable()
    {
        PlayFirstSongEvent?.Invoke();
    }
}
