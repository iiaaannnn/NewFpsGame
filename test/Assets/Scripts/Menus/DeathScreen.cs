using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject player;

    private void Update()
    {
        if (player.GetComponent<PlayerHealth>().GetHealth() <= 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;                                     
            Time.timeScale = 0.2f;
            deathScreen.SetActive(true);

        }
    }

    public void MainMenu()
    {
        Time.timeScale = 0.75f;
        SceneManager.LoadScene(0);
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
