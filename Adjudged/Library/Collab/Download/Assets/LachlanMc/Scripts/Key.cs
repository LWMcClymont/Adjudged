using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Rigidbody))]

public class Key : MonoBehaviour
{

    public bool inLock = false;

    public float keyUnlockAngle = 45;

    public float keyUnlockRotateSpeed = 45;

    private bool unlocked = false;

    private Transform desiredParent;

    float curYAngle;

    private bool opened = false;

    public DoorOpen doorOpener;

    public bool ableToOpenDoor = true;

    public Animator animController;

    //public void LockInPosition (KeyLockInfo info)
    //{
    //    GetComponent<Rigidbody>().isKinematic = true;
    //
    //    //transform.parent = info.keyLockPos;
    //    desiredParent = info.keyLockPos;
    //
    //    //transform.parent = null;
    //    transform.parent.GetComponent<Hand>().DetachObject(this.gameObject, false);
    //    transform.GetComponent<Rigidbody>().isKinematic = true;
    //
    //    transform.parent = null;
    //
    //    transform.position = info.keyLockPos.position;
    //
    //    transform.forward = info.keyLockPos.forward;
    //    transform.up = Vector3.up;
    //
    //    inLock = true;
    //
    //    Destroy(GetComponent<Throwable>());
    //    Destroy(GetComponent<VelocityEstimator>());
    //    Destroy(GetComponent<Interactable>());
    //
    //    animController.SetTrigger("Play");
    //
    //    //transform.parent.GetComponent<Hand>().DetachObject(this.gameObject);
    //
    //    //transform.parent = null;
    //}

    public void LockInPosition(KeyLockInfo info)
    {
        GetComponent<Rigidbody>().isKinematic = true;

        //transform.parent = info.keyLockPos;
        desiredParent = info.keyLockPos;

        transform.parent = null;

        transform.position = info.keyLockPos.position;

        transform.forward = info.keyLockPos.forward;
        transform.up = Vector3.up;

        inLock = true;

        Destroy(GetComponent<Throwable>());
        Destroy(GetComponent<VelocityEstimator>());
        Destroy(GetComponent<Interactable>());

        //transform.parent.GetComponent<Hand>().DetachObject(this.gameObject);

        //transform.parent = null;
    }

    //void Update()
    //{
    //    if (inLock && !unlocked)
    //    {
    //
    //    }
    //}

    void Update()
    {
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

        if (unlocked && !opened)
        {
            doorOpener.OpenDoor();

            //if (transform.parent != null)
            //{
            //    transform.parent.GetComponent<Hand>().DetachObject(this.gameObject);
            //}

            opened = true;
        }
    }


    void LateUpdate()
    {
        if (desiredParent == null || !unlocked)
            return;

        transform.position = desiredParent.position;

        Vector3 curAngle = transform.eulerAngles;

        curAngle.y = desiredParent.eulerAngles.y + curYAngle;

        transform.eulerAngles = curAngle;

        //transform.forward = desiredParent.forward;
    }

}