using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureManagerScript : MonoBehaviour {

	public GameObject m_picturePrefab;
	public Material m_pictureMaterial;
	public RenderTexture m_renderTexture;

	public GameObject m_pictureSpawnLocation;

	public bool m_useEmmision = false;
	public float m_emmisionLevel = 1;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public Texture2D SaveToTexture()
	{
		Texture2D t = null;
		if (m_renderTexture != null)
		{
			t = new Texture2D(m_renderTexture.width, m_renderTexture.height);
			RenderTexture.active = m_renderTexture;
			t.ReadPixels(new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), 0, 0);
			t.Apply();
		}
		return t;
	}

	public void CreatePicture()
	{
		if (m_picturePrefab != null && m_pictureMaterial != null)
		{
			Texture2D t = SaveToTexture();
			if (t != null)
			{
				GameObject pic = Instantiate(m_picturePrefab);
				PictureScript ps = pic.GetComponentInChildren<PictureScript>();
				if (ps != null)
				{
					Material mInstance = new Material(m_pictureMaterial);
					mInstance.mainTexture = t;
					if (m_useEmmision)
					{
						mInstance.SetTexture("_EmissionMap", t);
						mInstance.SetColor("_EmissionColor", Color.white * m_emmisionLevel);
					}
					ps.SetMaterial(mInstance);
				}
				if (m_pictureSpawnLocation != null)
				{
					pic.transform.position = m_pictureSpawnLocation.transform.position;
					pic.transform.rotation = m_pictureSpawnLocation.transform.rotation;
					pic.transform.localScale = m_pictureSpawnLocation.transform.localScale;
				}
			}
		}
	}

	void OnDestroy() // clean up script
	{
		print("Script was destroyed");
	}

}
