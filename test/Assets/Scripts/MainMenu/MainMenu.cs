using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator mainMenuAnim;
    public Animator settingsMenuAnim;

    float waitTime = 0.01f;

    //MAIN MENU
    public void OnStartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnSettings()
    {
        mainMenuAnim.SetBool("MainMenuActive", false);
        StartCoroutine(openSettings());

    }

    public void OnQuit()
    {
        Application.Quit();
    }

    //SETTINGS
    public void OnBack()
    {
        settingsMenuAnim.SetBool("SettingsMenuActive", false);
        StartCoroutine(openMainMenu());
    }

    //COROUTINES
    IEnumerator openSettings()
    {
        yield return new WaitForSeconds(waitTime);
        settingsMenuAnim.SetBool("SettingsMenuActive", true);

    }
    IEnumerator openMainMenu()
    {
        yield return new WaitForSeconds(waitTime);
        mainMenuAnim.SetBool("MainMenuActive", true);

    }
}
