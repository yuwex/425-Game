using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantBehavior : MonoBehaviour
{

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
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
                    // open merchant UI
                }
            }
        }
    }
}
