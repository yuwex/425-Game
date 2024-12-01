using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Info")]
    public CharacterController player;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float speed;

    private Vector3 velocity;
    private float gravity = -9.81f;
    private float groundDistance = 0.4f;
    private Boolean isGrounded;


    protected virtual void Update() 
    {

        // checks if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // get wasd movements
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // get local coordinates for moving
        Vector3 move = transform.right * x + transform.forward * z;
        player.Move(move * speed * Time.deltaTime);

        // calculate and apply gravity
        velocity.y += gravity * Time.deltaTime;
        player.Move(velocity * Time.deltaTime);

    }

}
