using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public struct WeaponStats
    {
        public WeaponStats(float attackDistance, float attackDelay, float attackSpeed, int attackDamage, GameObject go, MeshRenderer[] meshes, Action<float> animate)
        {
            this.attackDistance = attackDistance;
            this.attackDelay = attackDelay;
            this.attackSpeed = attackSpeed;
            this.attackDamage = attackDamage;
            this.animate = animate;
            this.go = go;
            this.meshes = meshes;
        }

        public void Animate()
        {
            animate(attackSpeed);
        }

        public void ToggleMesh()
        {
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.enabled = !mesh.enabled;
            }
        }

        public float attackDistance { get; set; }
        public float attackDelay { get; set; }
        public float attackSpeed { get; set; }
        public int attackDamage { get; set; }
        public GameObject go { get; set; }
        public MeshRenderer[] meshes { get; set; }
        private Action<float> animate;

    }

    public CharacterController player;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float speed = 15.0f;
    public float gravity = -70f;

    private Vector3 velocity;
    private float groundDistance = 0.4f;
    private float jumpHeight = 3;
    private bool isGrounded;

    [Header("Knife Stats")]

    public float knifeAttackDistance;
    public float knifeAttackDelay;
    public float knifeAttackSpeed;
    public int knifeAttackDamage;
    public GameObject knife;
    public MeshRenderer[] knifeMeshes;

    [Header("Gun Stats")]

    public float gunAttackDistance;
    public float gunAttackDelay;
    public float gunAttackSpeed;
    public int gunAttackDamage;
    public GameObject gun;
    public MeshRenderer[] gunMeshes;

    [Header("Attacking Misc")]

    public LayerMask attackLayer;
    bool attacking = false;
    public string enemyTag = "Enemy";

    public TowerSpawner towerSpawner;

    public GameObject fppCamera;

    WeaponStats knifeStats;
    WeaponStats gunStats;

    int currWeapon;
    List<WeaponStats> weapons;

    void Start()
    {
        knifeStats = new WeaponStats(knifeAttackDistance, knifeAttackDelay, knifeAttackSpeed, knifeAttackDamage, knife, knifeMeshes, (attackSpeed) => { knife.transform.rotation *= Quaternion.Euler(90 * Time.deltaTime / attackSpeed, 0, 0); });
        gunStats = new WeaponStats(gunAttackDistance, gunAttackDelay, gunAttackSpeed, gunAttackDamage, gun, gunMeshes, (attackSpeed) => {gun.transform.rotation *= Quaternion.Euler(-20 * Time.deltaTime / attackSpeed, 0, 0); });

        weapons = new List<WeaponStats> { knifeStats, gunStats };
        currWeapon = 0;

        foreach (WeaponStats weapon in weapons)
        {
            weapon.ToggleMesh();
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
            Attack();
        }

        if (attacking)
        {
            weapons[currWeapon].Animate();
        }

        if (!attacking && Input.GetKeyDown(KeyCode.E))
        {
            weapons[currWeapon].ToggleMesh();
            currWeapon = (currWeapon + 1) % weapons.Count;
            weapons[currWeapon].ToggleMesh();
        }
    }

    void Attack()
    {
        if (attacking || towerSpawner.buildEnabled) return;

        attacking = true;

        Invoke(nameof(ResetAttack), weapons[currWeapon].attackSpeed);
        Invoke(nameof(AttackRaycast), weapons[currWeapon].attackDelay);
    }

    void ResetAttack()
    {
        weapons[currWeapon].go.transform.rotation = fppCamera.transform.rotation;
        attacking = false;
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, weapons[currWeapon].attackDistance, attackLayer))
        {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag(enemyTag))
            {
                target.GetComponent<EnemyHealth>().Damage(weapons[currWeapon].attackDamage);
            }
        }
    }
}
