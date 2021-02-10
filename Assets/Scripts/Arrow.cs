using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Arrow : MonoBehaviour, IPointerClickHandler
{
    private static int SHOW_HASH = Animator.StringToHash("Show");

    public event Action ArrowClickedEvent = delegate { };

    [SerializeField] private Animator animator;

    public void ShowArrow(bool value)
    {
        animator.SetBool(SHOW_HASH, value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Mphka suntekne");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ArrowClickedEvent?.Invoke();
    }
}
