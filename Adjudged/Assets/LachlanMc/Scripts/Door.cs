using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour {

    public float openTime = 2f;

    public float speedMultiplier = 2f;

    private float curOpeningTime = 0;

    public AnimationCurve openCurve;

    private bool movingDoor = false;
	
    [Tooltip("Called when the door is opened")]
    public UnityEvent m_doorOpenCallback;
    [Tooltip("Called when the door is closed")]
    public UnityEvent m_doorCloseCallback;

    public void OpenDoor ()
    {
        movingDoor = true;

        speedMultiplier = Mathf.Abs(speedMultiplier);
        m_doorOpenCallback.Invoke();
    }

    public void CloseDoor ()
    {
        movingDoor = true;

        speedMultiplier = -Mathf.Abs(speedMultiplier);
        m_doorCloseCallback.Invoke();
    }

    void Update ()
    {
        if (movingDoor && curOpeningTime < openTime)
        {
            curOpeningTime += Time.deltaTime;

            transform.Rotate(Vector3.up * openCurve.Evaluate(curOpeningTime / openTime) * speedMultiplier);
        }
        else if (movingDoor && curOpeningTime >= openTime)
        {
            curOpeningTime = 0;
            movingDoor = false;
        }
    }
}
