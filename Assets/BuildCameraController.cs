using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCameraController : MonoBehaviour
{
    public GameObject target; // The player GameObject to follow
    public float maxHeight = 30f; // Maximum camera height at the center
    public float minHeight = 15f; // Minimum camera height at the edges
    public Vector2 boardCenter = Vector2.zero; // Center of the board
    public float spread = 10f; // Controls the spread of the Gaussian (the width of the bell curve)

    void Update()
    {
        
        // Calculate the player's distance from the board center in the X and Z axes
        float distanceFromCenter = Vector2.Distance(
            new Vector2(target.transform.position.x, target.transform.position.z),
            boardCenter
        );

        // Calculate the Gaussian height value based on the distance from the center
        // Apply the Gaussian formula
        float gaussianHeight = Mathf.Exp(-Mathf.Pow(distanceFromCenter, 2) / (2 * Mathf.Pow(spread, 2)));

        // Map the Gaussian height to the desired range between minHeight and maxHeight
        float height = Mathf.Lerp(minHeight, maxHeight, gaussianHeight);

        // Calculate the new position of the camera
        Vector3 newPosition = new Vector3(
            target.transform.position.x, // Follow player's X position
            height,                       // Adjust Y based on Gaussian distribution
            target.transform.position.z // Follow player's Z position with offset
        );

        // Assign the new position to the camera
        transform.position = newPosition;
       
    }
}
