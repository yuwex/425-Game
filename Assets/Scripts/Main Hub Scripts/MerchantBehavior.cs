using TMPro;
using UnityEngine;

public class MerchantBehavior : MonoBehaviour
{

    Animator anim;
    public GameObject merchantUI;
    public GameObject hubUI;
    public MouseControls mouseControls;
    public TMP_Text soulCounter;

    private bool merchantUIActive;

    [Header("SFX")]
    public AudioClip clickSound;

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
        if (Input.GetMouseButtonDown(0) && Camera.main)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
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
        SoundManager.Instance.PlaySFXClip(clickSound, transform);

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
        SoundManager.Instance.PlaySFXClip(clickSound, transform);

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
