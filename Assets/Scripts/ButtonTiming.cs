using System;
using UnityEngine;

[Serializable]
public struct ButtonTiming
{
    [Range(0f, 1f)]
    public float Timing; // in percentage of clip played [0,1]
    public ButtonID ButtonID;
}
