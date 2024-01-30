using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool paused = false;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Paused"))
        {
            //open pause menu
            if(!paused)
            {
                pauseMenu.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                paused = true;
            }
        }
     
    }

    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;

        paused = false;
        pauseMenu.SetActive(false);
    }

    public void Settings()
    {
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);

    }

   
    public void Quit()
    {
        Application.Quit();
    }
}
