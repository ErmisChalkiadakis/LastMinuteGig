using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MusicClipResults
{
    [NonSerialized]
    private const double EMPTY_WEIGHT = 0.5f;
    [NonSerialized]
    private const double MULTIPLE_WEIGHT = 0.15f;

    [NonSerialized]
    public int ID;
    [NonSerialized]
    public List<InputWindowResults> InputWindowResults;
    public double ClipStartTime;
    public Rhythm ClipRhythm;
    public Tempo ClipTempo;
    public List<double> ButtonPressTimes;

    public MusicClipResults(double clipStartTime)
    {
        ClipStartTime = clipStartTime;
        InputWindowResults = new List<InputWindowResults>();
        ButtonPressTimes = new List<double>();
    }

    public MusicClipResults(double clipStartTime, int id)
    {
        ClipStartTime = clipStartTime;
        ID = id;
        InputWindowResults = new List<InputWindowResults>();
        ButtonPressTimes = new List<double>();
    }

    public MusicClipResults(double clipStartTime, Rhythm rhythm, Tempo tempo)
    {
        ClipStartTime = clipStartTime;
        InputWindowResults = new List<InputWindowResults>();
        ButtonPressTimes = new List<double>();
        ClipRhythm = rhythm;
        ClipTempo = tempo;
    }

    public override string ToString()
    {
        foreach (double entry in ButtonPressTimes)
        {
            Debug.Log($"Entry: {entry}");
        }
        return JsonUtility.ToJson(this);
        //return $"Start time: {ClipStartTime}/n Rhythm: {ClipRhythm}/n Tempo: {ClipTempo}/n ButtonPresses: {}";
    }
    public static double MercuryDistance(MusicClipResults clipResultsA, MusicClipResults clipResultsB)
    {
        double distance = 0f;

        List<double> timesA = new List<double>();
        List<double> timesB = new List<double>();
        Dictionary<double, List<double>> timePairs = new Dictionary<double, List<double>>();

        // Case: Results A is empty
        if (clipResultsA.ButtonPressTimes.Count == 0)
        {
            return clipResultsB.ButtonPressTimes.Count * EMPTY_WEIGHT;
        }

        // Calculate all times of Results A and generate the pairs
        foreach (double time in clipResultsA.ButtonPressTimes)
        {
            timesA.Add(time - clipResultsA.ClipStartTime);
            timePairs.Add(time, new List<double>());
        }
        // Calculate all times of Results B and create the time pairs
        foreach (double time in clipResultsB.ButtonPressTimes)
        {
            timesB.Add(time - clipResultsB.ClipStartTime);
            double closestA = FindClosestInList(time, timesA);
            timePairs[closestA].Add(time);
        }
        // Assign double pairs to nearby empty times
        for (int i = 0; i < timesA.Count; i++)
        {
            if (timePairs[timesA[i]].Count == 0)
            {
                double current = timesA[i];
                double previous = 0f;
                double next = double.MaxValue;
                if (i > 0)
                {
                    List<double> timesPrevious = timePairs[timesA[i - 1]];
                    if (timesPrevious.Count > 1) 
                    {
                        previous = timesPrevious[timesPrevious.Count - 1];
                    }
                }
                if (i < timesA.Count - 1)
                {
                    List<double> timesNext = timePairs[timesA[i + 1]];
                    if (timesNext.Count > 1)
                    {
                        next = timesNext[0];
                    }
                }

                if (previous == 0f && next == double.MaxValue)
                {
                    continue;
                }

                double previousToCurrent = Mathf.Abs((float)(previous - current));
                double nextToCurrent = Mathf.Abs((float)(next - current));
                if (previousToCurrent < nextToCurrent)
                {
                    List<double> timesPrevious = timePairs[timesA[i - 1]];
                    double number = timesPrevious[timesPrevious.Count - 1];
                    timePairs[timesA[i]].Add(number);
                    timesPrevious.RemoveAt(timesPrevious.Count - 1);
                }
                else
                {
                    List<double> timesNext = timePairs[timesA[i + 1]];
                    double number = timesNext[0];
                    timePairs[timesA[i]].Add(number);
                    timesNext.RemoveAt(0);
                }
            }
        }

        // Calculate distance
        foreach (var kvp in timePairs)
        {
            if (kvp.Value.Count == 0)
            {
                distance += EMPTY_WEIGHT;
                continue;
            }
            foreach(double time in timePairs[kvp.Key])
            {
                distance += Mathf.Abs((float)(time - kvp.Key));
            }
            if (kvp.Value.Count > 1)
            {
                distance += (kvp.Value.Count - 1) * MULTIPLE_WEIGHT;
            }
        }

        return distance;
    }

    private static double FindClosestInList(double target, List<double> list)
    {
        double closestDistance = double.MaxValue;
        double closestNumber = 0f;

        foreach (double number in list)
        {
            float distance = Mathf.Abs((float)(number - target));
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNumber = number;
            }
        }

        return closestNumber;
    }
}
