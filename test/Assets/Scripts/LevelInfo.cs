using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public int kills = 0;
    int killsNeeded;

    [SerializeField]
    private TextMeshProUGUI killsText;

    int currentLevel = 1;

    public GameObject levelDoor;

    // Start is called before the first frame update
    void Start()
    {
        killsNeeded = currentLevel * 5;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    { 
        if(kills == killsNeeded)
        {
            //open door to next level
            levelDoor.SetActive(false);
        }
        killsText.text = kills.ToString() + "/" + killsNeeded.ToString();
    }
}
