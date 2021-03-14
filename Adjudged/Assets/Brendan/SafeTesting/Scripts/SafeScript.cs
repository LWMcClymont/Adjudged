using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SafeScript : MonoBehaviour
{

	public FreezeableCircularDrive m_circularDrive;

	public AudioSource m_unlockSound;
	public AudioSource m_lockSound;

	public bool m_locked = true;

	// Use this for initialization
	void Start ()
	{
		UpdateLock();
	}
	
	// Update is called once per frame
	void Update ()
	{

		// debug
		UpdateLock();
	}

	void UpdateLock()
	{
		if (m_circularDrive != null)
		{
			m_circularDrive.SetFreezeExternal(m_locked);
		}
	}

	public void LockDoor()
	{
		m_locked = true;
		UpdateLock();
		if (m_lockSound != null)
		{
			m_lockSound.Play();
		}
	}

	public void UnlockDoor()
	{
		m_locked = false;
		UpdateLock();
		if (m_unlockSound != null)
		{
			m_unlockSound.Play();
		}
	}
}
