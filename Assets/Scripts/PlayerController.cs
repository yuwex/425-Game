using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController player;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float speed = 15.0f;
    public float gravity = -40f;
    public float jumpHeight = 3;
    
    private Vector3 velocity;
    private float groundDistance = 0.4f;
    private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if player is touching a grounded surface
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2;
        }

        // get wasd movements
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // calculate direction for player to move using local player coordinates
        Vector3 move = transform.right * x + transform.forward * z;

        // move player
        player.Move(speed * Time.deltaTime * move);


        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        // calculate gravity
        velocity.y += gravity * Time.deltaTime;

        //apply gravity
        player.Move(velocity * Time.deltaTime);
    }
}
