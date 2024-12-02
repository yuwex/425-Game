using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponProjectile", menuName = "Weapons/Projectile")]
public class WeaponProjectile : WeaponBase
{
    public Vector3 recoil; 

    [Header("Weapon Stats")]
    public float velocity;
    public float attackDelay;
    public float attackSpeed;
    public int attackDamage;
    public int upgradedDamage = 0;
    public GameObject projectile;

    // debugging 
    public int totalDamage;

    private float totalTime = 0;

    private void OnEnable() {
        upgradedDamage = 0;
        totalDamage = attackDamage + upgradedDamage;
    }

    public override void Attack()
    {
        if (attacking || towerSpawner.buildEnabled) return;

        attacking = true;
        gameObject.transform.localPosition = position + recoil;

        player.StartCoroutine(ResetAttack());
        player.StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        totalDamage = attackDamage + upgradedDamage;

        yield return new WaitForSeconds(attackDelay);
        GameObject projectile = Instantiate(this.projectile, fppCamera.transform.position + fppCamera.transform.forward * 2, fppCamera.transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = fppCamera.transform.forward * velocity;
        projectile.GetComponent<Fireball>().damage = totalDamage;
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackSpeed);
        gameObject.transform.localPosition = position;
        attacking = false;
        totalTime = 0;
    }

    public override void Animate()
    {
        totalTime += Time.deltaTime;
        gameObject.transform.localPosition = Vector3.Lerp(position + recoil, position, totalTime/attackSpeed);
    }

}
