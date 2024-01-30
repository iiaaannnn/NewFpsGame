using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;

    //called from player
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        //overidden in other scripts
    }
   
}
