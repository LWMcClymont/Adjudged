using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFadeTrigger : MonoBehaviour {

	public SceneTransitionHandler m_sceneTransitionHandler;
	public string m_sceneTarget = "SceneName";
	public bool m_trigger = false;
	private bool m_done = false;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_trigger && !m_done)
		{
			m_sceneTransitionHandler.TransitionToScene(m_sceneTarget);
			m_done = true;
		}
	}
}
