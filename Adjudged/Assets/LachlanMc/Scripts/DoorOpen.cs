using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour {

    public Transform pivot;

    public float rotateSpeed = 100;

    public float rotateDuration = 1;

    private bool rotate = false;

    private float stoppingTime = float.MaxValue;

    public bool testDoorOnStartup = true;

    private bool testedDoor = false;

    void OnEnable ()
    {
        stoppingTime = float.MaxValue;
    }
        
    void Update ()
    {
        if (Time.time > 1 && testDoorOnStartup && !testedDoor)
        {
            OpenDoor();
            testedDoor = !testedDoor;
        }

        if (rotate)
        {
            if (Time.time < stoppingTime)
            {
                transform.RotateAround(pivot.position, Vector3.up, rotateSpeed * Time.deltaTime);
            }
            else
            {
                rotate = false;
            }
        }
    }

    public void OpenDoor ()
    {
        stoppingTime = Time.time + rotateDuration;
        
        rotate = true;
    }

    public void StartRotate (float rotationSpeed, float rotationDuration)
    {
        rotateSpeed = rotationSpeed;
        rotateDuration = rotationDuration;

        stoppingTime = Time.time + rotateDuration;

        rotate = true;
    }
}
