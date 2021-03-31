using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MoveSpotlight : MonoBehaviour
{
    [SerializeField] public bool Move = false;
    [SerializeField] public bool MoveToFirst = false;
    [SerializeField] private Vector3[] eulerAnglesOrientations;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float minDelay = 0.2f;
    [SerializeField] private float maxDelay = 0.4f;

    private List<Quaternion> quaternions;
    private Quaternion previousTargetOrientation;
    private Quaternion targetOrientation;
    private Quaternion startOrientation = Quaternion.identity;
    private float rotationProgress = 0f;
    private float rotateToFirstProgress = 0f;
    private Quaternion firstOrientation;
    private bool waiting;
    private float moveTime;

    private static Random random = new Random();

    protected void OnEnable()
    {
        quaternions = new List<Quaternion>();
        Vector3 cachedEulerAngles = transform.localEulerAngles;
        firstOrientation = transform.localRotation;
        foreach (Vector3 eulerAngles in eulerAnglesOrientations)
        {
            transform.localEulerAngles = eulerAngles;
            quaternions.Add(transform.localRotation);
        }
        transform.localEulerAngles = cachedEulerAngles;

        int randomIndex = random.Next(quaternions.Count);
        targetOrientation = quaternions[randomIndex];
        previousTargetOrientation = transform.localRotation;
    }

    protected void Update()
    {
        if (MoveToFirst)
        {
            if (startOrientation != Quaternion.identity)
            {
                startOrientation = transform.localRotation;
            }
            if (transform.localRotation != firstOrientation)
            {
                rotateToFirstProgress += rotationSpeed * Time.deltaTime;
                transform.localRotation = Quaternion.Slerp(startOrientation, firstOrientation, rotateToFirstProgress);
            }
            else
            {
                startOrientation = Quaternion.identity;
                rotateToFirstProgress = 0f;
            }
        }
        else if (Move)
        {
            if (transform.localRotation != targetOrientation && !waiting)
            {
                rotationProgress += rotationSpeed * Time.deltaTime;
                transform.localRotation = Quaternion.Slerp(previousTargetOrientation, targetOrientation, rotationProgress);
            }
            else if (!waiting)
            {
                int newIndex = random.Next(quaternions.Count);
                previousTargetOrientation = targetOrientation;
                targetOrientation = quaternions[newIndex];
                rotationProgress = 0f;
                waiting = true;
                moveTime = Time.time + (float)random.Next((int)(minDelay * 100), (int)(maxDelay * 100)) / 100f;
            }
            else
            {
                if (Time.time > moveTime)
                {
                    waiting = false;
                }
            }
        }
    }
}
