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
    public float gravity = -70f;

    private Vector3 velocity;
    private float groundDistance = 0.4f;
    private float jumpHeight = 3;
    private bool isGrounded;

    [Header("Attacking")]

    public List<WeaponBase> weapons;

    public LayerMask attackLayer;
    public string enemyTag = "Enemy";
    public TowerSpawner towerSpawner;

    private int currWeapon = 0;

    void Awake()
    {
        foreach (WeaponBase weapon in weapons)
        {
            weapon.Init(attackLayer, enemyTag, towerSpawner, this);
        }

        weapons[currWeapon].ToggleMesh();
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

        if (Input.GetKey(KeyCode.Mouse0))
        {
            weapons[currWeapon].Attack();
        }

        if (weapons[currWeapon].attacking)
        {
            weapons[currWeapon].Animate();
        }

        if (!weapons[currWeapon].attacking && Input.GetKeyDown(KeyCode.E))
        {
            weapons[currWeapon].ToggleMesh();
            currWeapon = (currWeapon + 1) % weapons.Count;
            weapons[currWeapon].ToggleMesh();
        }
    }

}
