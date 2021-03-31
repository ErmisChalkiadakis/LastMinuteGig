using System;
using UnityEngine;

public class PlayFirstSongOnEnable : MonoBehaviour
{
    public event Action PlaySongEvent = delegate { };

    protected void OnEnable()
    {
        PlaySongEvent?.Invoke();
    }
}
