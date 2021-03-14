using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class DestroyWhenPickedUp : MonoBehaviour {

    public float delay = 0;

    private bool destroying = false;

    private float curTime = 0;

    public void DestroyItem()
    {
        if (destroying)
        {
            return;
        }

        destroying = true;

        //Destroy(gameObject, delay);
        //
        //if (transform.parent.GetComponent<Hand>())
        //{
        //    transform.parent.GetComponent<Hand>().DetachObject(gameObject);
        //}
    }

    void Update()
    {
        if (destroying)
        {
            curTime += Time.deltaTime;

            if (curTime >= delay)
            {
                if (transform.parent)
                {
                    if (transform.parent.GetComponent<Hand>())
                    {
                        transform.parent.GetComponent<Hand>().DetachObject(gameObject);
                    }
                }
                Destroy(gameObject);
            }
        }
    }

}
