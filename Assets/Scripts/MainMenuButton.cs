using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private const string BUTTON_SELECTED = "ButtonSelected";

    private static int HOVER_HASH = Animator.StringToHash("Hover");
    private static int DOWN_HASH = Animator.StringToHash("Down");

    public delegate void ButtonSelectedHandler();
    public event ButtonSelectedHandler ButtonSelectedEvent;
    
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorStateObserver animatorStateObserver;
    [SerializeField] private AudioSource hoverAudioSource;
    [SerializeField] private AudioSource downAudioSource;
    [SerializeField] private AudioSource upAudioSource;

    private bool isInteractable = true;

    protected void Awake()
    {
        animatorStateObserver.AnimatorStateEnteredEvent += OnAnimatorStateEnteredEvent;
    }

    protected void OnDestroy()
    {
        animatorStateObserver.AnimatorStateEnteredEvent -= OnAnimatorStateEnteredEvent;
    }

    private void OnAnimatorStateEnteredEvent(string state)
    {
        if (state == BUTTON_SELECTED)
        {
            StartCoroutine(SelectButton());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isInteractable)
        {
            animator.SetBool(HOVER_HASH, true);
            if (!animator.GetBool(DOWN_HASH))
            {
                hoverAudioSource.Play();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isInteractable)
        {
            animator.SetBool(HOVER_HASH, false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isInteractable)
        {
            animator.SetBool(DOWN_HASH, true);
            downAudioSource.Play();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isInteractable)
        {
            animator.SetBool(DOWN_HASH, false);
        }
    }

    private IEnumerator SelectButton()
    {
        upAudioSource.Play();
        yield return new WaitForSeconds(.5f);

        ButtonSelectedEvent?.Invoke();
        isInteractable = false;
    }
}
