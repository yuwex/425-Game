using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    private Animator animator; // Cached Animator reference
    private Vector3 previousPosition; // To store the player's position in the previous frame
    public bool isMoving;

    void Start()
    {
        // Cache the Animator component from the player object
        animator = player.GetComponent<Animator>();

        // Initialize the previous position to the player's starting position
        previousPosition = player.transform.position;
    }

    void Update()
    {
        // Calculate the player's velocity by comparing positions
        Vector3 currentPosition = player.transform.position;
        Vector3 movement = currentPosition - previousPosition;

        // Determine if the player is moving by checking the magnitude of movement
        isMoving = movement.magnitude > Mathf.Epsilon; // Use a small threshold to avoid floating-point inaccuracies

        // Update animation states
        animator.SetBool("isWalking", isMoving);

        // Update the previous position for the next frame
        previousPosition = currentPosition;
    }
}
