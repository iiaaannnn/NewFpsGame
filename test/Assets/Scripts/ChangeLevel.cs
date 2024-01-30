using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public GameObject levelCompleteOverlay;

    bool startTimer = false;
    float timer = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            levelCompleteOverlay.SetActive(true);
            startTimer = true;
        }
            
    }
 

    private void Update()
    {
        if(startTimer && timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0)
        {
            levelCompleteOverlay.SetActive(false);

            startTimer = false;

            timer = 3f;

            //change level
            SceneManager.LoadScene("Level2");
            
        }
    }
}
