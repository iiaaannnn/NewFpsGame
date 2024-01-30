using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    float health;
    float lerptimer;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    public GameObject damageOverlay;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        damageOverlay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        //debug
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RestoreHealth(10);
        }
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;

        float healthPercentage = health / maxHealth;

        //take damage
        if (fillBack > healthPercentage)
        {
            frontHealthBar.fillAmount = healthPercentage;
            backHealthBar.color = Color.red;
            lerptimer += Time.deltaTime;

            float percentComplete = lerptimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthPercentage, percentComplete);

            //turn off damage overlay
            if(percentComplete >= 0.1)
            { 
                damageOverlay.SetActive(false);
            }
        }

        //restore health
        if (fillFront < healthPercentage)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = healthPercentage;
            lerptimer += Time.deltaTime;

            float percentComplete = lerptimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }

        

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerptimer = 0f;
        damageOverlay.SetActive(true);
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerptimer = 0f;
        damageOverlay.SetActive(false);

    }
}
