using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MouseControls : MonoBehaviour
{

    public float sens = 150f;
    public Transform player;
    private float xRotate;

    // Start is called before the first frame update
    void Start()
    {
        // lock cursor to center and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // retrieve mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * sens;
        float mouseY = Input.GetAxis("Mouse Y") * sens;

        xRotate -= mouseY;

        // clamp player view rotation
        xRotate = Mathf.Clamp(xRotate, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotate, 0, 0);
        player.Rotate(Vector3.up * mouseX);
    }

    public void UpdateSens(float newSens)
    {
        sens = newSens;
    }
}
