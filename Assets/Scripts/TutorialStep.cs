using System;

[Serializable]
public class TutorialStep
{
    public string Text;
    public bool PlayNextAnimation;
    public float ShowTextAfterSeconds = 0f;
    public float PlayAnimationAfterSeconds = 0f;
}
