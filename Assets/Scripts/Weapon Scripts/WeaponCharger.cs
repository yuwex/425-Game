using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponCharger", menuName = "Weapons/Charger")]
public class WeaponCharger : WeaponBase
{
    [Header("Weapon Stats")]
    public float maxChargeTime;
    public float maxChargeVelocity;
    public float attackDelay;
    public int maxDamage;
    public GameObject projectile;

    private float chargeTime;
    private bool doneAnimating = false;

    public override void Attack()
    {
        if (towerSpawner.buildEnabled || (attacking && chargeTime == 0)) return;

        chargeTime += Time.deltaTime;
        if (chargeTime >= maxChargeTime)
        {
            chargeTime = maxChargeTime;
        }

        attacking = true;
    }

    public override void Release()
    {
        if (attacking && chargeTime > 0)
        {
            GameObject projectile = Instantiate(this.projectile, fppCamera.transform.position + fppCamera.transform.forward, fppCamera.transform.rotation);

            projectile.GetComponent<Rigidbody>().velocity = fppCamera.transform.forward * maxChargeVelocity * ((maxChargeTime + chargeTime) / (2 * maxChargeTime));
            projectile.GetComponent<Projectile>().damage = 5 + (int)Math.Floor(Math.Pow(Math.Pow(maxDamage - 5, 1 / 1.5f) * (chargeTime / maxChargeTime), 1.5f));

            chargeTime = 0;
            player.StartCoroutine(ResetAttack());
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        gameObject.transform.rotation = fppCamera.transform.rotation;
        doneAnimating = false;
        attacking = false;
    }

    public override void Animate()
    {
        if (!doneAnimating)
        {
            gameObject.transform.rotation *= Quaternion.Euler(animateRotation * (Time.deltaTime / maxChargeTime), 0, 0);
            if (chargeTime == maxChargeTime) doneAnimating = true;
        }
    }
}
