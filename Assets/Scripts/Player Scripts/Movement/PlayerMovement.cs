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
    public AudioClip walkingSound;


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

        Vector3 move;

        if (Camera.main == GetComponentInChildren<Camera>())
        {
            move = transform.right * x + transform.forward * z;
        }
        else
        {
            move = new Vector3(x, 0, z);
        }
        // Prevent diagonal being faster
        if (move.magnitude > 1)
            move.Normalize();

        // get local coordinates for moving
        player.Move(speed * Time.deltaTime * move);

        // calculate and apply gravity
        velocity.y += gravity * Time.deltaTime;
        player.Move(velocity * Time.deltaTime);

        // if (move.magnitude > 0)
        // {
        //     SoundManager.Instance.PlaySFXClip(walkingSound, player.transform);
        // }
    }

}
