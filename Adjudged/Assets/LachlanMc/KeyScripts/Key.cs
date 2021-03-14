using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class Key : MonoBehaviour {

    public Door door;
    public Transform drawerTransform;
    public float keyUnlockAngle = 80;

    private Vector3 anchorPosition;
    private Vector3 anchorRotation;

    public float keyVariance = 180;

    public bool keyBeingHeld = false;

    public bool isDrawer = false;

    void Awake ()
    {
        anchorPosition = transform.position;
        anchorRotation = transform.localEulerAngles;

        if (door == null && !isDrawer)
        {
            print(transform.name + " has no door assigned!");
            return;
        }

        if (isDrawer && drawerTransform == null)
        {
            print(transform.name + " has no drawer assigned!");
            return;
        }
    }

    void LateUpdate ()
    {
        if (keyBeingHeld)
        {
            transform.position = anchorPosition;

            transform.rotation = Quaternion.Euler(anchorRotation.x, anchorRotation.y - keyVariance, transform.eulerAngles.z);  // EDIT: - 180 // keyVariance

            if (GetAngleOfKey() >= keyUnlockAngle)
            {
                keyBeingHeld = false;

                if (isDrawer)
                {
                    UnlockDrawer();
                }
                else
                {

                    UnlockDoor();
                }
            }
        }
        else if (transform.parent == null)
        {
            if (!isDrawer)
            {
                transform.SetParent(door.transform);
            }
            else
            {
                transform.SetParent(drawerTransform);
            }
        }
    }

    void UnlockDoor ()
    {
        door.OpenDoor();

        if (transform.parent.GetComponent<Hand>())
        {
            transform.parent.GetComponent<Hand>().DetachObject(gameObject);

            Destroy(GetComponent<Throwable>());
            Destroy(GetComponent<VelocityEstimator>());
            Destroy(GetComponent<Interactable>());
        }
    }

    void UnlockDrawer()
    {
        //drawerTransform.GetComponent<LinearDrive>().enabled = true;

        drawerTransform.GetComponent<BoxCollider>().enabled = true;

        if (transform.parent.GetComponent<Hand>())
        {
            transform.parent.GetComponent<Hand>().DetachObject(gameObject);

            Destroy(GetComponent<Throwable>());
            Destroy(GetComponent<VelocityEstimator>());
            Destroy(GetComponent<Interactable>());
        }
    }

    public void KeyGrabbed ()
    {
        keyBeingHeld = true;
    }

    public void KeyReleased ()
    {
        keyBeingHeld = false;
    }

    float GetAngleOfKey ()
    {
        return Vector3.Angle(Vector3.up, transform.up);
    }
}
