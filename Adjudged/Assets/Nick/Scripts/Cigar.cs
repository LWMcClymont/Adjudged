using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cigar : MonoBehaviour {


    public bool lit = false;
    private bool smoking = false;
    public GameObject smoke;

    private List<GameObject> smokeTrails;

    private bool firstSmoke;
    // Use this for initialization
    void Start ()
    {
        firstSmoke = false;
        smokeTrails = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (lit)
        {
            if (!firstSmoke)
            {
                Debug.Log("firstSmoke");
                firstSmoke = true;
                InstantiateSmoke();
            }
        }
        else
        {
            FadeOutSmoke();
        }

        for (int i = 0; i < smokeTrails.Count; i++)
        {
            SmokeTrail smokeScript = smokeTrails[i].GetComponent<SmokeTrail>();
            if (smokeScript.ReadyToDestroy())
            {
                smokeScript.DestroySmoke();
                smokeTrails.RemoveAt(i);
            }

        }
    }

    void FadeOutSmoke()
    {
        for (int i = 0; i < smokeTrails.Count; i++)
        {
            smokeTrails[i].GetComponent<SmokeTrail>().StopSmoking();
        }
    }

    void InstantiateSmoke()
    {
        if (lit)
        {
            smokeTrails.Add(Instantiate(smoke, transform));
        }
    }
}
