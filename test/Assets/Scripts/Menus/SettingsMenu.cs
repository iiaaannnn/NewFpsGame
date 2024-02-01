using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    public TextMeshProUGUI sensitivityText;
    public TextMeshProUGUI volumeText;

    [SerializeField]
    private Slider mouseSensSlider;
    public float sensitivity = 50;

    [SerializeField]
    private Slider volumeSlider;
    public float volume = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //mouse sens
        sensitivity = mouseSensSlider.value;
        sensitivityText.text = "Mouse Sensitivity " + sensitivity.ToString();

        //volume 
        volume = volumeSlider.value;
        volumeText.text = "Master Volume " + volume.ToString(); 
    }

    public void Back()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

   
}
