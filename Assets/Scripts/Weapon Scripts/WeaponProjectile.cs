using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponProjectile", menuName = "Weapons/Projectile")]
public class WeaponProjectile : WeaponBase
{
    [Header("Weapon Stats")]
    public float velocity;
    public float attackDelay;
    public float attackSpeed;
    public int attackDamage;
    public GameObject projectile;

    public override void Attack()
    {
        if (attacking || towerSpawner.buildEnabled) return;

        attacking = true;

        player.StartCoroutine(ResetAttack());
        player.StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(attackDelay);
        GameObject projectile = Instantiate(this.projectile, fppCamera.transform.position + fppCamera.transform.forward, fppCamera.transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = fppCamera.transform.forward * velocity;
        projectile.GetComponent<Fireball>().damage = attackDamage;
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackSpeed);
        gameObject.transform.rotation = fppCamera.transform.rotation;
        attacking = false;
    }

    public override void Animate()
    {
        gameObject.transform.rotation *= Quaternion.Euler(animateRotation * Time.deltaTime / attackSpeed, 0, 0);
    }

}
