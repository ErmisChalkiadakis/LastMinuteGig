using UnityEngine;

public class HideButtonOnEnable : MonoBehaviour
{
    [SerializeField] private InstrumentButton instrumentButton;

    private CanvasGroup canvasGroup;

    protected void OnEnable()
    {
        canvasGroup = instrumentButton.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        instrumentButton.Interactable = false;
    }

    protected void OnDisable()
    {
        canvasGroup.alpha = 1f;
        instrumentButton.Interactable = true;
    }
}
