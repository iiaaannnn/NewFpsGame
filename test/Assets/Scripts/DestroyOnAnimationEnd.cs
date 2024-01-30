using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    public void DestroyParent()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
