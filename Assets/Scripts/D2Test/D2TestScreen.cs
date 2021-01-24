using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class D2TestScreen : MonoBehaviour
{
    [SerializeField] private D2TestSample testSample;
    [SerializeField] private int sampleCount = 30;

    private D2TestSampleResults[] testSampleResults;
    private D2TestCase[] testCases;
    private int testSampleCount = 0;

    protected void Awake()
    {
        testSampleResults = new D2TestSampleResults[sampleCount];
        testCases = GenerateRandomTestCases();
        StartTest();
    }

    private void StartTest()
    {
        testSample.StartTest(testCases[testSampleCount]);
        testSample.TestSampleFinishedEvent += OnTestSampleFinishedEvent;
    }

    private void FinishTest()
    {
        SceneManager.LoadScene(1);
    }

    private void OnTestSampleFinishedEvent(D2TestSampleResults results)
    {
        testSample.TestSampleFinishedEvent -= OnTestSampleFinishedEvent;
        testSampleResults[testSampleCount] = results;
        testSampleCount++;

        if (testSampleCount < sampleCount)
        {
            StartTest();
        }
        else
        {
            FinishTest();
        }
    }

    private D2TestCase[] GenerateRandomTestCases()
    {
        Array values = Enum.GetValues(typeof(D2TestCase));
        Random random = new Random();
        var testCases = new D2TestCase[sampleCount];

        int correctCount = 0;
        int incorrectCount = 0;

        for (int i = 0; i < sampleCount; i++)
        {
            int correctCase = random.Next(2);
            if (correctCase == 0)
            {
                testCases[i] = (D2TestCase)correctCase;
                correctCount++;
            }
            else
            {
                int incorrectCase = random.Next(1, 3);
                testCases[i] = (D2TestCase)incorrectCase;
                incorrectCount++;
            }
        }

        return testCases;
    }
}
