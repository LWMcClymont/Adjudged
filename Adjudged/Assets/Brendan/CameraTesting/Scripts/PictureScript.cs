using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureScript : MonoBehaviour {
	
	public GameObject m_imagePlane;

	Material m_material;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void SetMaterial(Material mat)
	{
		if (mat != null)
		{
			m_material = mat;
			if (m_imagePlane != null)
			{
				Renderer r = m_imagePlane.GetComponent<Renderer>();
				if (r != null)
				{
					r.material = m_material;
				}
			}
		}
	}

}
