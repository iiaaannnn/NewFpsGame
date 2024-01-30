using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Interactable
{
    public GameObject player;

    protected override void Interact()
    {
        player.GetComponent<PlayerHealth>().RestoreHealth(50);                                                                  
        Destroy(gameObject);
    }
}
