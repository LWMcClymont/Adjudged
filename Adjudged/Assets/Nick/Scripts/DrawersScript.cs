using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class DrawersScript : MonoBehaviour
{

    public List<GameObject> contents;
    private LinearMapping map;
    private bool objectsActive = true;
    public float Threshold = 0.02f;
    private bool isDrawer;

    void Start()
    {
        contents = new List<GameObject>();
        map = GetComponentInParent<LinearMapping>();
        if (map != null)
            isDrawer = true;
        else isDrawer = false;
    }


    void Update()
    {
        if (isDrawer)
        {
            if (objectsActive && map.value < Threshold)
            {
                for (int i = 0; i < contents.Count; i++)
                {
                    contents[i].SetActive(false);
                }
                objectsActive = false;
            }
            else if (!objectsActive && map.value > Threshold)
            {
                for (int i = 0; i < contents.Count; i++)
                {
                    contents[i].SetActive(true);
                }
                objectsActive = true;
            }
        }
    }

	void OnTriggerEnter(Collider obj)
	{
        // Lachlan's edit
        ////////////////////////////
        if (obj.GetComponent<Key>())
            return;
        /////////////////////

		obj.GetComponent<Throwable>().SetParent(transform);
        if(!contents.Contains(obj.gameObject))
            contents.Add(obj.gameObject);
        Debug.Log("enterTrigger " + transform);
	}
	
	void OnTriggerExit(Collider obj)
	{
        contents.Remove(obj.gameObject);
		obj.GetComponent<Throwable>().SetParent(null);
        Debug.Log("exitTrigger");
	}
}