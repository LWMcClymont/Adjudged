using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrackCollidingObjects : MonoBehaviour {

	public List<Collider> m_colliders;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// can be overriden to allow only certain collisions to be accepted
	/// </summary>
	/// <param name="collider"></param>
	/// <returns></returns>
	virtual public bool TriggerDecider(Collider collider)
	{
		return true;
	}

	void OnTriggerEnter(Collider collider)
	{
		//print("collider:\"" + collider.gameObject.name + "\" triggered:\"" + gameObject.name + "\"");
		if (!m_colliders.Contains(collider) && TriggerDecider(collider))
		{
			m_colliders.Add(collider);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		m_colliders.Remove(collider);
	}

	
}
