using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunModifier", menuName = "TowerModifiers/Shotgun")]
public class ShotgunModifier : MultiplicitiveModifier
{

    public override void SetupProjectile(TowerProjectile projectile)
    {

        var rigidbody = projectile.GetComponent<Rigidbody>();

        projectile.GetStat(Stat.ProjectileVelocity, out float velocity);

        rigidbody.velocity += Random.insideUnitSphere * velocity / 4;
        projectile.transform.localScale /= 2;
    }
}
