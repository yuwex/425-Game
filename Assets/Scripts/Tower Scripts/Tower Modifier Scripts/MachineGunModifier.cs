using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MachineGunModifier", menuName = "TowerModifiers/MachineGun")]
public class MachineGunModifier : MultiplicitiveModifier
{
    public override void SetupProjectile(TowerProjectile projectile)
    {

        var transform = projectile.transform;

        if (projectile.batchIndex % 2 == 0) 
        {
            transform.position += transform.right * 0.5f;
        }
        else
        {
            transform.position -= transform.right * 0.5f;
        }
    }
}
