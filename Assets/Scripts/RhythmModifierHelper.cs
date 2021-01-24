using UnityEngine;

public static class RhythmModifierHelper
{
    public static float GetRhythmModifier(Rhythm rhythm)
    {
        switch (rhythm)
        {
            case Rhythm.FourFour:
                return 4;
            case Rhythm.EightEight:
                return 4;
            case Rhythm.FourRest:
                return 4;
            case Rhythm.ThreeRest:
                return 4;
            default:
                return 4;
        }
    }
}
