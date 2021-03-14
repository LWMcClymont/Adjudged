using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePrint : MonoBehaviour {

	[Tooltip("String to be printed to log when function 'DoTheThing' is called")]
	public string m_string = "Thing to print";

	public LogType m_logType = LogType.Log;

	public bool m_printOnStart = false;
	public bool m_printOnAwake = false;
	public bool m_printOnEnabled = false;
	public bool m_printOnUpdate = false;
	public bool m_printOnFixedUpdate = false;


	// Use this for initialization
	void Start()
	{
		if (m_printOnStart)
		{
			DoTheThing();
		}
	}

	void Awake()
	{
		if (m_printOnAwake)
		{
			DoTheThing();
		}
	}

	void OnEnable()
	{
		if (m_printOnEnabled)
		{
			DoTheThing();
		}
	}
	

	// Update is called once per frame
	void Update()
	{
		if (m_printOnUpdate)
		{
			DoTheThing();
		}
	}

	void FixedUpdate()
	{
		if (m_printOnFixedUpdate)
		{
			DoTheThing();
		}
	}

	public void DoTheThing()
	{
		Debug.unityLogger.Log(m_logType, m_string);
	}
}
