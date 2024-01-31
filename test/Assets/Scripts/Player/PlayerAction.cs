using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class PlayerAction : MonoBehaviour
{
    public PlayerCam fpsCam;
    public Transform player;
    Vector3 initialpos;
    int mode = 1;

    [Header("Block")]
    public GameObject block;
    public Transform PLACEBLOCKPOINT;
    public GameObject handblock;
    public List<GameObject> blocks = new List<GameObject>();
    bool isClearing = false;

    [Header("Gun")]
    public GameObject gun;
    public GameObject handGun;
    int weaponNum = 1;
    public Animator gunanim;
    public Animator handGunanim;
    public GameObject ammotext;

    [Header("WeaponUI")]
    public GameObject select1;
    public GameObject select2;

    [Header("Melee")]
    public GameObject arm;
    float meleeTimer = 0.5f;
    public GameObject meleeIconSelect;

    [Header("Ability")]
    public GameObject openedArm;
    float abilityTimer = 0.5f;
    float abilityCooldown = 0f;
    public GameObject abilityIconSelect;
    public TextMeshProUGUI cooldownText;
    public GameObject abilityIconDarken;
    public GameObject explosionEffect;
    public RaycastHit rayHit;
    public LayerMask WhatIsEnemy;

    public float range;

    int abilityDamage = 10;
    float effectTimer = 1f;
    // Start is called before the first frame update
    void Start()
    {
        initialpos = player.transform.position;

        arm.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        MeleeAttack();
        UseAbility();

        if(Input.GetKey(KeyCode.F1))
        {
            player.transform.position = initialpos;
            
        }
        //if(Input.GetButtonDown("SwitchMode"))
        //{
        //    switch(mode)
        //    {
        //        case 1: //set to block
        //            mode = 2;
        //            gun.SetActive(false);
        //            handblock.SetActive(true);
        //            ammotext.SetActive(false);
        //            break;
        //        case 2: //set to gun
        //            mode = 1;
        //            gun.SetActive(true);
        //            handblock.SetActive(false);
        //            ammotext.SetActive(true);
        //            break;
        //        default:
        //            break;
        //    }
                
        //}

        //if (isClearing)
        //{
        //    foreach (var myblock in blocks)
        //    {
        //        blocks.Remove(myblock);
        //        Destroy(myblock);
        //        Debug.Log(blocks.Capacity.Equals(0));
        //    }
        //    isClearing = false;
        //}
        

        switch (mode)
        {
            case 1:
                if (Input.GetButton("Scope"))
                {
                    Scope();
                }
                else
                {
                    UnScope();
                }
                break;
            case 2:
                if (Input.GetButtonDown("PlaceBlock"))
                {
                    var myblock = Instantiate(block, PLACEBLOCKPOINT.position, Quaternion.identity);
                    blocks.Add(myblock);
                }

                if ((Input.GetButtonDown("Clear")) && (!isClearing))
                {
                    isClearing = true;
                    Debug.Log("isclearing");
                }
                break;
            default:
                break;
        }


        //switch weapon
        if (!gun.GetComponent<Gun>().reloading)
        {
            if (Input.GetButtonDown("Weapon1") && weaponNum != 1)
            {
                Weapon1();
            }
            if (Input.GetButtonDown("Weapon2") && weaponNum != 2)
            {
                Weapon2();
            }
        }

        


    }
    private void MeleeAttack()
    {
        if (Input.GetButtonDown("Melee") && !arm.activeInHierarchy && !openedArm.activeInHierarchy)
        {
            arm.SetActive(true);
            meleeIconSelect.SetActive(true);
        }

        if (arm.activeInHierarchy)
        {
            if (meleeTimer <= 0)
            {
                arm.SetActive(false);
                meleeTimer = 0.5f;
                arm.GetComponent<MeleeAttack>().hitAlready = false;
                meleeIconSelect.SetActive(false);

            }
            else
            {
                meleeTimer -= Time.deltaTime;
            }
        }
    }
    
    private void UseAbility()
    {

        if(abilityCooldown > 0)
        {
            abilityCooldown -= Time.deltaTime;
            abilityIconDarken.SetActive(true);
            cooldownText.text = (Mathf.Round(abilityCooldown * 10f) * 0.1f).ToString();
        }
        else
        {
            abilityIconDarken.SetActive(false);
            cooldownText.text = "";
        }
        if(Input.GetButtonDown("Ability1") && !openedArm.activeInHierarchy && !arm.activeInHierarchy && abilityCooldown <= 0)
        {
            openedArm.SetActive(true);
            abilityIconDarken.SetActive(false);
            abilityCooldown = 5f;

            Vector3 direction = fpsCam.transform.forward;

            if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, WhatIsEnemy))
            {
                if (rayHit.collider.CompareTag("Enemy"))
                {
                    //deal extra damage when precisely hit enemy
                    rayHit.collider.GetComponent<Enemy>().TakeDamage(abilityDamage);

                }
            }
            var myExplosionEffect = Instantiate(explosionEffect, rayHit.point, Quaternion.Euler(0, 0, 0));
            Destroy(myExplosionEffect, effectTimer);
        }

        if (openedArm.activeInHierarchy)
        {
            if(abilityTimer <= 0)
            {
                openedArm.SetActive(false);
                abilityTimer = 0.5f;
            }
            else
            {
                abilityTimer -= Time.deltaTime;
            }
        }
    }

    private void Scope()
    {
        fpsCam.DoFov(30);
        fpsCam.sensX = 375;
        fpsCam.sensY = 375;

        gunanim.SetBool("IsScoped", true);
        handGunanim.SetBool("IsScoped", true);
    }
    private void UnScope()
    {
        fpsCam.DoFov(80);
        fpsCam.sensX = 1000;
        fpsCam.sensY = 1000;

        gunanim.SetBool("IsScoped", false);
        handGunanim.SetBool("IsScoped", false);
    }

    private void Weapon1()
    {
        gun.SetActive(true);
        handGun.SetActive(false);
        weaponNum = 1;
        select1.SetActive(true);
        select2.SetActive(false);
    }

    private void Weapon2()
    {
        gun.SetActive(false);
        handGun.SetActive(true);
        weaponNum = 2;
        select2.SetActive(true);
        select1.SetActive(false);
    }
}
