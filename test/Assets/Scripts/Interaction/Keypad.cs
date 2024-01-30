using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    [Header("Animator")]
    public Animator doorAnim;
    bool doorOpen = false;

    [Header("Keypad Light")]
    public Material green;
    public Material red;
    public GameObject keyPadLight;

    // Start is called before the first frame update
    void Start()
    {
        keyPadLight.GetComponent<MeshRenderer>().material = red;
    }

    protected override void Interact()
    {
        //open/close the door
        if(doorOpen)
        {
            doorAnim.SetBool("OpenDoor", false);
            doorOpen = false;
            keyPadLight.GetComponent<MeshRenderer>().material = red;

        }
        else
        {
            doorAnim.SetBool("OpenDoor", true);
            doorOpen = true;
            keyPadLight.GetComponent<MeshRenderer>().material = green;

        }
    }   
}
