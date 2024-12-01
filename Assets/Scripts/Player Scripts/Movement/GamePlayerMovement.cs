using System.Collections.Generic;
using UnityEngine;

public class GamePlayerMovement : PlayerMovement
{
    
    [Header("Attacking")]
    public List<WeaponBase> weapons;
    public LayerMask attackLayer;
    public string enemyTag = "Enemy";
    public TowerSpawner towerSpawner;

    private int currWeapon = 0;

    private void Awake()
    {
        foreach (WeaponBase weapon in weapons)
        {
            weapon.Init(attackLayer, enemyTag, towerSpawner, this);
        }

        weapons[currWeapon].ToggleMesh();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Call PlayerMovement.cs Update() function
        base.Update();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            weapons[currWeapon].Attack();
        }
        else
        {
            weapons[currWeapon].Release();
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
