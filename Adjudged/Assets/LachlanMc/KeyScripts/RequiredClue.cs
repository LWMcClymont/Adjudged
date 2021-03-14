using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequiredClue : MonoBehaviour {

    public KeyActivator keyActivator;

    // TODO: ADD CODE TO HAND CLASS IN AttachObject() TO USE THIS

    void Awake ()
    {
        if (keyActivator == null)
        {
            print(transform.name + " has no keyActivator assigned!");
        }    
    }

    public void ObjectPickedUp ()
    {
        if (keyActivator == null)
        {
            return;
        }

        keyActivator.ObjectFound(this);
    }
}
