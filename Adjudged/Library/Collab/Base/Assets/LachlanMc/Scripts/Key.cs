using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Key : MonoBehaviour {

    public bool inLock = false;

    private KeySwivler[] swivlers;

    private float holdLength = 2;

    bool beingHeld = false;

    public float openAngle = -45;

    private bool unlocked = false;

    void Start()
    {
        swivlers = GameObject.FindObjectsOfType<KeySwivler>();
    }

    public void LockInPosition (KeyLockInfo info)
    {
        GetComponent<Rigidbody>().isKinematic = true;

        transform.parent = info.keyLockPos;

        transform.position = info.keyLockPos.position;

        transform.forward = info.keyLockPos.forward;

        inLock = true;

    }

    void Update ()
    {
        if (transform.parent == null)
        {
            transform.Translate(-Vector3.forward * Time.deltaTime);
        }
        
        if (unlocked)
        {
            return;
        }

        for (int i = 0; i < swivlers.Length; i++)
        {
            if (Vector3.Distance(swivlers[i].transform.position, transform.position) < holdLength)
            {
                if (!swivlers[i].GrabInput())
                {
                    beingHeld = false;
                    return;
                }

                if (swivlers[i].GrabInput() && !beingHeld)
                {
                    swivlers[i].transform.up = Vector3.up;

                    beingHeld = true;
                }

                float rollAngle = swivlers[i].GetRollAngle();

                Vector3 curRot = transform.eulerAngles;
                curRot.z = swivlers[i].GetRollAngle();
                transform.eulerAngles = curRot;

                if (openAngle < 0)
                {
                    print(Vector3.Angle(Vector3.left, transform.up));
                    if (Vector3.Angle(Vector3.up, transform.up) > -openAngle &&
                        Vector3.Angle(Vector3.right, transform.up) < 90)
                    {
                        unlocked = true;
                        print("UNLOCKED DOOR");
                    }
                }
                else if (openAngle >= 0)
                {
                    print(Vector3.Angle(Vector3.up, transform.up));
                    if (Vector3.Angle(Vector3.up, transform.up) >= openAngle &&
                        Vector3.Angle(Vector3.right, transform.up) > 90)
                    {
                        unlocked = true;
                        print("UNLOCKED DOOR");
                    }
                }
                
            }
            else
            {
                beingHeld = false;
            }
        }
    }

}
