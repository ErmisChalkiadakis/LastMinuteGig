using System;
using UnityEngine;

public class BowOnEnable : MonoBehaviour
{
    public event Action BowEvent = delegate { };
    public event Action BowEndedEvent = delegate { };

    protected void OnEnable()
    {
        BowEvent?.Invoke();
    }

    protected void OnDisable()
    {
        BowEndedEvent?.Invoke();
    }
}
