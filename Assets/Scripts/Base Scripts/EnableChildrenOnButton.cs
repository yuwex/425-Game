using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableChildrenOnButton : MonoBehaviour
{
    // Update is called once per frame
    public KeyCode key;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
