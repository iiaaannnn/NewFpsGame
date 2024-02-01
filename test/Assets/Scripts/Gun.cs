using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

using TMPro;
using System.Net.Http.Headers;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public int damage;
    public float timeBetweenShooting, timeBetweenShots, spread, range, reloadTime;

    public int magSize, bulletsPerTap;

    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    bool Shooting, readyToShoot;

    public bool reloading;

    [Header("References")]
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;

    public LayerMask WhatIsEnemy;

    public GameObject bulletholeImage;
    public GameObject AttackPoint;
    public GameObject hitMarker;

    public TextMeshProUGUI ammoText;

    public Animator gunanim;
    public Animator handGunanim;

    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject player;

    [Header("Camera Shake")]
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    public Transform camTransform;
    Vector3 originalPos;
    public float shakeDuration = 0f;

    //plasma effect
    float plasmaEffectTimer = 0.2f;

    float hitMarkerTimer = 0.1f;

    private void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = true;
        originalPos = camTransform.localPosition;
        ammoText.SetText(bulletsLeft + "/" + magSize);

        hitMarker.SetActive(false);

    }
    private void Update()
    {
        ammoText.SetText(bulletsLeft + "/" + magSize);
        
        MyInput();

        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }

        if(bulletsLeft == 0)
        {
            Reload();
        }

        //hitmarker
        if(hitMarker.activeInHierarchy)
        {
            if(hitMarkerTimer <= 0)
            {
                hitMarker.SetActive(false);
                hitMarkerTimer = 0.1f;

            }
            else
            {
                hitMarkerTimer -= Time.deltaTime;
            }
        }
    
    }
    private void MyInput()
    {
        //check whether can hold down button
        if (allowButtonHold)
        {
            Shooting = Input.GetButton("Shoot");
        }
        else
        {
            Shooting = Input.GetButtonDown("Shoot");
        }

        //shoot
        if(readyToShoot && Shooting && !reloading && bulletsLeft > 0 && !pauseMenu.activeInHierarchy && !settingsMenu.activeInHierarchy && player.GetComponent<PlayerHealth>().GetHealth() > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
            shakeDuration = 0.1f;
            
        }
        

        AttackPoint.SetActive(Shooting && !reloading);

        //reload
        if (Input.GetButtonDown("Reload") && bulletsLeft < magSize)
        {
            Reload();
        }

    }


    private void Shoot()
    {
        readyToShoot = false;

        //spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calc direction with spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);


        //raycast hit enemy
        if(Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, WhatIsEnemy))
        {
            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
                hitMarker.SetActive(true);
            }
        }

        //spawn plasmaEffect
        var myPlasmaEffect = Instantiate(bulletholeImage, rayHit.point, Quaternion.Euler(0, 0, 0));
        Destroy(myPlasmaEffect, plasmaEffectTimer);

        //spawn muzzleflash
        AttackPoint.SetActive(true);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        //shoot
        if(bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
        
      
        switch(player.GetComponent<PlayerAction>().weaponNum)
        {
            case 1:
                gunanim.SetBool("IsShooting", true);
                break;
            case 2:
                handGunanim.SetBool("IsShooting", true);
                break;
            default: break;
        }

    }

    private void ResetShot() 
    {
        readyToShoot = true;

        switch (player.GetComponent<PlayerAction>().weaponNum)
        {
            case 1:
                gunanim.SetBool("IsShooting", false);
                break;
            case 2:
                handGunanim.SetBool("IsShooting", false);
                break;
            default: break;
        }
    }

    private void Reload()
    {
        reloading = true;

        switch (player.GetComponent<PlayerAction>().weaponNum)
        {
            case 1:
                gunanim.SetBool("Reload", true);
                break;
            case 2:
                handGunanim.SetBool("Reload", true);
                break;
            default: break;
        }

        Invoke("ReloadFinished", reloadTime);
        Invoke("ResetReloadAnim", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magSize;
        reloading = false;
    }

    private void ResetReloadAnim()
    {

        switch (player.GetComponent<PlayerAction>().weaponNum)
        {
            case 1:
                gunanim.SetBool("Reload", false);
                break;
            case 2:
                handGunanim.SetBool("Reload", false);
                break;
            default: break;
        }

    }

}
