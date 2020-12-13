public static class TempoToBPMHelper
{
    public static float GetBPM(Tempo tempo)
    {
        switch (tempo)
        {
            case Tempo.t96:
                return 96f;
            case Tempo.t120:
                return 120f;
            default:
                return 0f;
        }
    }
}
