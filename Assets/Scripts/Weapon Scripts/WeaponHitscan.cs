using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponHitscan", menuName = "Weapons/Hitscan")]
public class WeaponHitscan : WeaponBase
{
    public int animateRotation;

    [Header("Weapon Stats")]
    public float attackDistance;
    public float attackDelay;
    public float attackSpeed;
    public int attackDamage;
    public int upgradedDamage;

    // debugging
    public int totalDamage;

    [Header("SFX")]
    public List<AudioClip> attackSounds;

    private void OnEnable() {
        upgradedDamage = 0;
        totalDamage = attackDamage + upgradedDamage;
    }

    public override void Attack()
    {
        if (attacking || towerSpawner.buildEnabled) return;

        attacking = true;

        SoundManager.Instance.PlayRandomSFXClip(attackSounds, Camera.main.transform);

        player.StartCoroutine(ResetAttack());
        player.StartCoroutine(Damage());
    }

    private IEnumerator Damage() {

        totalDamage = attackDamage + upgradedDamage;

        yield return new WaitForSeconds(attackDelay);
        if (Physics.Raycast(fppCamera.position, fppCamera.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag(enemyTag))
            {
                target.GetComponent<Enemy>().Hurt(totalDamage);
            }
        }
    }

    private IEnumerator ResetAttack() {
        yield return new WaitForSeconds(attackSpeed);
        gameObject.transform.rotation = fppCamera.transform.rotation;
        attacking = false;
    }

    public override void Animate() {
        gameObject.transform.rotation *= Quaternion.Euler(animateRotation * Time.deltaTime / attackSpeed, 0, 0); 
    }

}
