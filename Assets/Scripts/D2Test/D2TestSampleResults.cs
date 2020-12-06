public class D2TestSampleResults
{
    public D2TestCase TestCase { get; private set; }
    public bool CasePressed { get; private set; }
    public double TimeOfClick { get; private set; }

    public D2TestSampleResults(D2TestCase testCase)
    {
        TestCase = testCase;
    }

    public void ButtonPressed(double timeOfClick)
    {
        CasePressed = true;
        TimeOfClick = timeOfClick;
    }

    public override string ToString()
    {
        return $"Case: {TestCase}, Pressed: {CasePressed}, TimeOfClick: {TimeOfClick}";
    }
}
