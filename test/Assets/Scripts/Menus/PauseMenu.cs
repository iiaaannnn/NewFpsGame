using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool paused = false;
    public GameObject pauseMenuOverlay;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    public Animator pauseAnim;

    private void Start()
    {
        pauseMenuOverlay.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Paused"))
        {
            //open pause menu
            if(!paused)
            {
                pauseMenu.SetActive(true);
                pauseMenuOverlay.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                pauseAnim.SetBool("IsOpen", true);

                paused = true;

                StartCoroutine(waitForAnimationOpen());
            }
            else
            {
                Resume();
            }
        }
     
    }

    public void Resume()
    {
        Time.timeScale = 1; 

        paused = false;
        pauseMenuOverlay.SetActive(false);

        pauseAnim.SetBool("IsOpen", false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pauseMenu.SetActive(false);


    }

    public void Settings()
    {
        settingsMenu.SetActive(true);
        //pauseMenu.SetActive(false);

    }

   
    public void MainMenu()
    {
        Time.timeScale = 0.75f;
        SceneManager.LoadScene(0);
    }

    IEnumerator waitForAnimationOpen()
    {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 0f;
    }

}
