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

    [Header("Sounds")]
    public List<AudioClip> weaponSwitchSounds;

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

        if (!weapons[currWeapon].attacking && !towerSpawner.buildEnabled)
        {
            var weaponChanged = false;

            if (Input.GetKeyDown(KeyCode.Alpha1) && currWeapon != 0 && weapons.Count >= 1)
            {
                weaponChanged = true;

                weapons[currWeapon].ToggleMesh();
                currWeapon = 0;
                weapons[currWeapon].ToggleMesh();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && currWeapon != 1 && weapons.Count >= 2)
            {
                weaponChanged = true;

                weapons[currWeapon].ToggleMesh();
                currWeapon = 1;
                weapons[currWeapon].ToggleMesh();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && currWeapon != 2 && weapons.Count >= 3)
            {
                weaponChanged = true;

                weapons[currWeapon].ToggleMesh();
                currWeapon = 2;
                weapons[currWeapon].ToggleMesh();
            }

            if (weaponChanged) 
                SoundManager.Instance.PlayRandomSFXClip(weaponSwitchSounds, gameObject.transform);
        }
    }
}
