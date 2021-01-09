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
            case Rhythm.ThreeFour:
                return 3;
            default:
                return 1;
        }
    }
}
