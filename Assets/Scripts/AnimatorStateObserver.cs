using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateObserver : MonoBehaviour
{
    public delegate void AnimatorStateChangedHandler(string state);
    public event AnimatorStateChangedHandler AnimatorStateEnteredEvent = delegate { };
    public event AnimatorStateChangedHandler AnimatorStateExitedEvent = delegate { };
    private readonly List<string> states = new List<string>();
    private readonly List<string> heartBeats = new List<string>();
    private List<string> enteredStates = new List<string>();
    private List<string> exitedStates = new List<string>();

    public void SignalHeartbeat(string characterState, int stateIdentifier)
    {
        if (!heartBeats.Contains(characterState))
        {
            heartBeats.Add(characterState);
        }
    }

    public bool IsInState(string characterState)
    {
        return states.Contains(characterState);
    }

    public bool IsInAllStates(params string[] states)
    { 
        bool allStates = true;

        foreach (var item in states)
        {
            allStates = IsInState(item);
        }

        return allStates;
    }

    public bool IsInAnyOfStates(params string[] states)
    {
        foreach (var item in states)
        {
            if (IsInState(item))
            {
                return true;
            }
        }

        return false;
    }

    private void LateUpdate()
    {
        enteredStates.Clear();
        for (int i = 0; i < heartBeats.Count; i++)
        {
            if (!states.Contains(heartBeats[i]))
            {
                enteredStates.Add(heartBeats[i]);
                states.Add(heartBeats[i]);
            }
        }

        exitedStates.Clear();
        for (int i = states.Count - 1; i >= 0; i--)
        {
            if (!heartBeats.Contains(states[i]))
            {
                exitedStates.Add(states[i]);
                states.RemoveAt(i);
            }
        }

        heartBeats.Clear();

        if (AnimatorStateEnteredEvent != null)
        {
            foreach (string enteredState in enteredStates)
            {
                AnimatorStateEnteredEvent(enteredState);
            }
        }
        if (AnimatorStateExitedEvent != null)
        {
            foreach (string exitedState in exitedStates)
            {
                AnimatorStateExitedEvent(exitedState);
            }
        }
    }
}
