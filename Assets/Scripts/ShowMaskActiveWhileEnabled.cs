using UnityEngine;

public class ShowMaskActiveWhileEnabled : MonoBehaviour
{
    [SerializeField] private StickFigure stickFigure;

    private SpriteRenderer[] spriteRenderers;

    protected void OnEnable()
    {
        spriteRenderers = stickFigure.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }

    protected void OnDisable()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
    }
}
