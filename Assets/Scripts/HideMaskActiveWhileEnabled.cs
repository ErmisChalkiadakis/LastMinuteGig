using UnityEngine;

public class HideMaskActiveWhileEnabled : MonoBehaviour
{
    public float Alpha = 1f;

    [SerializeField] private StickFigure stickFigure;

    private SpriteRenderer[] spriteRenderers;

    protected void OnEnable()
    {
        spriteRenderers = stickFigure.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
    }

    protected void OnDisable()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
    }

    protected void Update()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Alpha);
        }
    }
}
