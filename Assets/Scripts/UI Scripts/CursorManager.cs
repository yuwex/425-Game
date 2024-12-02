using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {

        StartCoroutine(menuStart());
    }

    IEnumerator menuStart()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        yield return new WaitForSeconds(1);
        Time.timeScale = 0f;
    }
}
