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

    public float attackDistance;
    public float attackDelay;
    public float attackSpeed;
    public int attackDamage;
    public LayerMask attackLayer;
    bool attacking = false;
    public string enemyTag = "Enemy";

    public TowerSpawner towerSpawner;

    public GameObject knife;
    public GameObject fppCamera;

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

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }

        if (attacking)
        {
            knife.transform.rotation *= Quaternion.Euler(90 * Time.deltaTime / attackSpeed, 0, 0);
        }
    }

    void Attack()
    {
        if (attacking || towerSpawner.buildEnabled) return;

        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);
    }

    void ResetAttack()
    {
        knife.transform.rotation = fppCamera.transform.rotation;
        attacking = false;
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag(enemyTag))
            {
                target.GetComponent<EnemyHealth>().Damage(attackDamage);
            }
        }
    }
}
