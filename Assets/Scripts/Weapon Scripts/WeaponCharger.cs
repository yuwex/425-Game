using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponCharger", menuName = "Weapons/Charger")]
public class WeaponCharger : WeaponBase
{
    public Vector3 recoil;

    [Header("Weapon Stats")]
    public float maxChargeTime;
    public float maxChargeVelocity;
    public float attackDelay;
    public int maxDamage;
    public GameObject projectile;

    private float chargeTime;
    private GameObject chargeBar;

    public override void uniqueInit()
    {
        chargeBar = GameObject.FindGameObjectWithTag("ChargeBar");
        chargeBar.SetActive(false);
    }

    public override void Attack()
    {
        if (towerSpawner.buildEnabled || (attacking && chargeTime == 0)) return;

        chargeBar.SetActive(true);
        chargeBar.GetComponent<Slider>().value = 0;

        chargeTime += Time.deltaTime;
        chargeBar.GetComponent<Slider>().value = chargeTime / maxChargeTime;
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
            GameObject projectile = Instantiate(this.projectile, fppCamera.transform.position + fppCamera.transform.forward * 2, fppCamera.transform.rotation);

            projectile.GetComponent<Rigidbody>().velocity = fppCamera.transform.forward * maxChargeVelocity * ((maxChargeTime + chargeTime) / (2 * maxChargeTime));
            projectile.GetComponent<Projectile>().damage = 5 + (int)Math.Floor(Math.Pow(Math.Pow(maxDamage - 5, 1 / 1.5f) * (chargeTime / maxChargeTime), 1.5f));

            chargeBar.SetActive(false);
            chargeTime = 0;
            gameObject.transform.localPosition = position + recoil;

            player.StartCoroutine(ResetAttack());
            player.StartCoroutine(Recoil());
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attacking = false;
    }

    private IEnumerator Recoil()
    {
        float totalTime = 0;
        while (totalTime < attackDelay)
        {
            totalTime += Time.deltaTime;
            gameObject.transform.localPosition = Vector3.Lerp(position + recoil, position, totalTime / attackDelay);
            yield return null;
        }
        gameObject.transform.localPosition = position;
    }

}
