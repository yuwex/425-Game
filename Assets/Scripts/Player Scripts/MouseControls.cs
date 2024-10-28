using System.Collections;
using System.Collections.Generic;
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
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        // retrieve mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sens;

        xRotate -= mouseY;

        // clamp player view rotation
        xRotate = Mathf.Clamp(xRotate, -90, 90);

        player.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(xRotate, 0, 0);
    }
}
