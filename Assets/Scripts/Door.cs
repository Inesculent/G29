using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen = false;
    [SerializeField]
    private bool IsRotatingDoor = true;
    [SerializeField]
    private float Speed = 1f;

    [Header("Rotation Configs")]
    [SerializeField]
    private float RotationAmount = 90f;
    [SerializeField]
    private float ForwardDirection = 0;

    [Header("Sliding Configs")]
    [SerializeField]
    private Vector3 SlideDirection = Vector3.back;
    [SerializeField]
    private float SlideAmount = 1.9f;

    private Transform PivotTransform;
    private Quaternion StartRotation;
    private Vector3 StartPosition;
    private Vector3 Forward;

    private Coroutine AnimationCoroutine;

    // ✅ ADD THIS: public flag to track whether door has opened
    public bool doorOpened { get; private set; } = false;

    private void Awake()
    {
        PivotTransform = transform.parent != null ? transform.parent : transform;

        StartRotation = PivotTransform.rotation;
        StartPosition = PivotTransform.position;

        Forward = PivotTransform.right;
    }

    public void Open(Vector3 UserPosition)
    {
        if (!IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            if (IsRotatingDoor)
            {
                float dot = Vector3.Dot(Forward, (UserPosition - PivotTransform.position).normalized);
                Debug.Log($"Dot: {dot.ToString("N3")}");
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
            else
            {
                AnimationCoroutine = StartCoroutine(DoSlidingOpen());
            }
        }
    }

    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = PivotTransform.rotation;
        Quaternion endRotation;

        if (ForwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.eulerAngles.y + RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.eulerAngles.y - RotationAmount, 0));
        }

        IsOpen = true;
        float time = 0;
        while (time < 1)
        {
            PivotTransform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

        // ✅ Set the doorOpened flag once door animation finishes
        doorOpened = true;
    }

    private IEnumerator DoSlidingOpen()
    {
        Vector3 endPosition = StartPosition + SlideAmount * PivotTransform.TransformDirection(SlideDirection);
        Vector3 startPosition = PivotTransform.position;

        float time = 0;
        IsOpen = true;
        while (time < 1)
        {
            PivotTransform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

        // ✅ Set the doorOpened flag once door animation finishes
        doorOpened = true;
    }

    public void Close()
    {
        if (IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            if (IsRotatingDoor)
            {
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
            else
            {
                AnimationCoroutine = StartCoroutine(DoSlidingClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = PivotTransform.rotation;
        Quaternion endRotation = StartRotation;

        IsOpen = false;

        float time = 0;
        while (time < 1)
        {
            PivotTransform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

        // Optional: Reset flag if you want to allow the patrol logic to stop again
        // doorOpened = false;
    }

    private IEnumerator DoSlidingClose()
    {
        Vector3 endPosition = StartPosition;
        Vector3 startPosition = PivotTransform.position;
        float time = 0;

        IsOpen = false;

        while (time < 1)
        {
            PivotTransform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

        // Optional: Reset flag if needed
        // doorOpened = false;
    }
}
