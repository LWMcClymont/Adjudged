using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanWind : MonoBehaviour {

    public float windSpeed;
	
	// Update is called once per frame
	void OnTriggerStay (Collider obj)
    {
        obj.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * windSpeed);
	}
}
