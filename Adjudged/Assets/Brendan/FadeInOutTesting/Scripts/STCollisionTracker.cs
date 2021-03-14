using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STCollisionTracker : TrackCollidingObjects {

	public SceneTransitionHandler m_sceneHandler;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_sceneHandler != null && !m_sceneHandler.IsInExiting())
		{
			if (m_colliders.Count > 0)
			{
				STTriggerObject stto = m_colliders[0].gameObject.GetComponent<STTriggerObject>();
				if (stto != null)
				{
					m_sceneHandler.TransitionToScene(stto.m_sceneDestination);
				}
			}
		}
	}

	public override bool TriggerDecider(Collider collider)
	{
		if (collider.gameObject.GetComponent<STTriggerObject>() != null)
		{
			return true;
		}
		return false;
	}

}
