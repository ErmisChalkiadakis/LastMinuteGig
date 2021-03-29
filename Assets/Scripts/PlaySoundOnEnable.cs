using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool fadeOutVolume;
    [SerializeField] private float fadeOutRate = 0.01f;
    [SerializeField] private float fadeOutAfterDelay = 4f;

    private float targetVolume = 1f;
    private float startTime;

    protected void Awake()
    {
        audioSource.clip = audioClip;
    }

    protected void OnEnable()
    {
        audioSource.Play();
        targetVolume = 1f;
        audioSource.volume = targetVolume;
        startTime = Time.time;
    }

    protected void OnDisable()
    {
        audioSource.Stop();
    }

    protected void Update()
    {
        if (fadeOutVolume)
        {
            if (Time.time > startTime + fadeOutAfterDelay)
            {
                targetVolume -= fadeOutRate * Time.deltaTime;
                audioSource.volume = targetVolume;
            }
        }
    }
}
