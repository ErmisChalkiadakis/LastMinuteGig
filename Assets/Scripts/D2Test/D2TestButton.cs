using UnityEngine;
using UnityEngine.EventSystems;

public class D2TestButton : MonoBehaviour, IPointerDownHandler
{
    public delegate void ButtonPressedHandler();
    public event ButtonPressedHandler ButtonPressedEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonPressedEvent?.Invoke();
    }
}
