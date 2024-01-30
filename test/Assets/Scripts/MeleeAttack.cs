using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    public bool hitAlready = false;

    private void OnTriggerEnter(Collider collision)
    {
        Transform hitTransform = collision.transform;

        if (hitTransform.CompareTag("Enemy"))
        {
            if(!hitAlready) //hit only once
            {
                hitTransform.GetComponent<Enemy>().TakeDamage(20);
                hitAlready = true;
            }
        }
   
    }
}
