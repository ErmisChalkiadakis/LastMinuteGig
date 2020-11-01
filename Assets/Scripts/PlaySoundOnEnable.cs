using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;

    protected void Awake()
    {
        audioSource.clip = audioClip;
    }

    protected void OnEnable()
    {
        audioSource.Play();
    }

    protected void OnDisable()
    {
        audioSource.Stop();
    }
}
