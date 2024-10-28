using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPerspectiveController : MonoBehaviour
{
    public Transform player; // Assign your player object in the Inspector
    public Vector3 offset;   // Set the desired offset for the third-person view
    public float smoothSpeed = 0.125f; // Adjust this for smoothing

    private bool isThirdPersonView = false;
    private Vector3 originalPosition; // To store the original position
    private Quaternion originalRotation; // To store the original rotation

    void Start()
    {
        // Store the original position and rotation
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // Check if the "P" key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleCameraView();
        }

        // If in third-person view, move the camera
        if (isThirdPersonView)
        {
            MoveCamera();
        }
        else
        {
            // Reset to the original first-person view
            ResetCamera();
        }
    }

    void ToggleCameraView()
    {
        isThirdPersonView = !isThirdPersonView;
    }

    void MoveCamera()
    {
        // Calculate the desired position
        Vector3 desiredPosition = player.position + offset;

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.LookAt(player); // Make the camera look at the player
    }

    void ResetCamera()
    {
        // Smoothly return to the original position and rotation
        transform.position = Vector3.Lerp(transform.position, originalPosition, smoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, smoothSpeed);
    }
}