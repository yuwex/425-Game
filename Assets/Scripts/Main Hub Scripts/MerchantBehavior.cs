using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantBehavior : MonoBehaviour
{

    Animator anim;
    public GameObject merchantUI;
    public GameObject hubUI;
    public MouseControls mouseControls;
    public TMP_Text soulCounter;
    public WeaponHitscan sword;
    public WeaponCharger crossbow;
    public WeaponProjectile scepter;


    private bool merchantUIActive;

    void Awake()
    {
        // ensure correct UI objects are opened on start
        hubUI.SetActive(false);
        merchantUI.SetActive(false);
        merchantUIActive = false;
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        soulCounter.text = "Souls: " + GameManager.Instance.playerSouls;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "Merchant")
                {

                    anim.SetTrigger("Clicked");
                    
                    OpenMerchantMenu();

                }
            }
        }

        if (merchantUIActive)
        {
            soulCounter.text = "Souls: " + GameManager.Instance.playerSouls;
        }
    }


    void OpenMerchantMenu()
    {
        // enable mouse controls
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // deactivate hub UI
        hubUI.SetActive(false);

        // pause game
        Time.timeScale = 0f;

        // pause camera
        mouseControls.enabled = false;

        // activate merchant UI
        merchantUI.SetActive(true);
        merchantUIActive = true;

    }

    public void CloseMerchantMenu()
    {
        // disable mouse controls
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // deactivate merchant UI
        merchantUI.SetActive(false);

        // resume game
        Time.timeScale = 1.0f;

        // resume camera motion
        mouseControls.enabled = true;

        hubUI.SetActive(true);
        merchantUIActive = false;
    }
}
