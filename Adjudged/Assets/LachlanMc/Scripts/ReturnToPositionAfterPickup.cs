using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPositionAfterPickup : MonoBehaviour {

    public float delay = 3;

    private Vector3 returnPosition;
    private Quaternion returnRotation;

    private Rigidbody rbody;

    private float curTime;
    private bool returning = false;

    void Awake ()
    {
        returnPosition = transform.position;
        returnRotation = transform.rotation;

        if (GetComponent<Rigidbody>())
        {
            rbody = GetComponent<Rigidbody>();
        }
    }

    void Update ()
    {
        if (returning)
        {
            curTime += Time.deltaTime;

            if (curTime >= delay)
            {
                ReturnToOriginalPosition();
                returning = false;
            }
        }
    }

    public void StartReturning ()
    {
        returning = true;
        curTime = 0;
    }

    public void StopReturning ()
    {
        returning = false;
    }

    private void ReturnToOriginalPosition ()
    {
        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = Vector3.zero;

        transform.rotation = returnRotation;
        transform.position = returnPosition;
    }
}
