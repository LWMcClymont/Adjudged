using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyActivator : MonoBehaviour {

    public List<RequiredClue> requiredClues;
    public bool keyEnabledByDefault = true;
    private bool cluesFound = false;
    public GameObject keyObject;

    void Awake ()
    {
        if (keyObject == null)
        {
            print(gameObject.name + " has no keyObject assigned!");
            return;
        }

        if (requiredClues.Count <= 0)
        {
            cluesFound = true;
        }

        keyObject.SetActive(keyEnabledByDefault);
    }

    public void ObjectFound (RequiredClue clue)
    {
        if (requiredClues.Contains(clue))
        {
            requiredClues.Remove(clue);
        }

        if (requiredClues.Count <= 0)
        {
            AllCluesFoundEvent();
        }
    }

    void OnTriggerEnter (Collider info)
    {
        if (!cluesFound)
        {
            return;
        }

        if (info.transform.root.name == "Player" ||
            info.transform.root.tag == "Player")
        {
            PlayerEnteredTriggerEvent();
        }
    }

    public void AllCluesFoundEvent ()
    {
        cluesFound = true;

        // eg. Enable a light and sound effect
    }

    public void PlayerEnteredTriggerEvent ()
    {
        keyObject.SetActive(true);
    }
}
