using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstrumentButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private static int HOVER_HASH = Animator.StringToHash("Hover");
    private static int DOWN_HASH = Animator.StringToHash("Down");

    private const int AUDIO_SOURCE_COUNT = 2;

    public delegate void ButtonSelectedHandler(ButtonID buttonId);
    public event ButtonSelectedHandler ButtonSelectedEvent;

    [SerializeField] private ButtonID buttonId;
    [SerializeField] private Animator animator;
    [SerializeField] private Image instrumentIcon;

    private AudioSource[] audioSources;
    private int flip = 0;

    private AudioClip[] audioClips;
    private int clipIndex = 0;

    private bool test = true;

    protected void Awake()
    {
        CreateAudioSources();
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ButtonSelected();
        }
    }

    public void SetInstrumentSounds(AudioClip[] audioClips)
    {
        this.audioClips = audioClips;
        SetNextAudioClip();

        test = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool(HOVER_HASH, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool(HOVER_HASH, false);
        animator.SetBool(DOWN_HASH, false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonSelected();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool(DOWN_HASH, false);
    }

    private void ButtonSelected()
    {
        PlayAudioClip();
        animator.SetBool(DOWN_HASH, true);
        ButtonSelectedEvent?.Invoke(buttonId);

        if (audioClips != null)
        {
            SetNextAudioClip();
        }
    }

    private void CreateAudioSources()
    {
        audioSources = new AudioSource[AUDIO_SOURCE_COUNT];
        for (int i = 0; i < AUDIO_SOURCE_COUNT; i++)
        {
            GameObject audioSource = new GameObject($"Audio Source {i}");
            audioSource.transform.parent = gameObject.transform;
            audioSources[i] = audioSource.AddComponent<AudioSource>();
            audioSources[i].volume = 0.8f;
        }
    }
    
    private void SetNextAudioClip()
    {
        if (audioSources[flip].isPlaying)
        {
            flip = 1 - flip;
        }

        audioSources[flip].clip = audioClips[clipIndex];

        if (++clipIndex >= audioClips.Length)
        {
            clipIndex = 0;
        }
    }

    private void PlayAudioClip()
    {
        audioSources[1 - flip].Stop();
        audioSources[flip].Play();
    }
}
