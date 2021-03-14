using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Key : MonoBehaviour {

    public bool inLock = false;

    public float keyUnlockAngle = 45;

    public float keyUnlockRotateSpeed = 45;

    private bool unlocked = false;

    private Transform desiredParent;

    float curYAngle;

    public void LockInPosition (KeyLockInfo info)
    {
        GetComponent<Rigidbody>().isKinematic = true;

        //transform.parent = info.keyLockPos;
        desiredParent = info.keyLockPos;

        transform.position = info.keyLockPos.position;

        transform.forward = info.keyLockPos.forward;

        inLock = true;

    }

    void Update ()
    {
        if (!inLock)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
        }

        if (inLock && !unlocked)
        {
            float curAngle = Vector3.Angle(Vector3.up, transform.up);

            if (curAngle < keyUnlockAngle)
            {
                transform.Rotate(transform.forward * -keyUnlockRotateSpeed * Time.deltaTime);
            }
            else
            {
                unlocked = true;
                //transform.parent = desiredParent;

                //curYAngle = transform.eulerAngles.y;
            }
        }
    }


    void LateUpdate ()
    {
        if (desiredParent == null || !unlocked)
            return;
        
        transform.position = desiredParent.position;
       
        Vector3 curAngle = transform.eulerAngles;

        curAngle.y = desiredParent.eulerAngles.y + curYAngle;

        transform.eulerAngles = curAngle;
    }

}
