using UnityEngine;

[CreateAssetMenu(fileName = "MortarModifier", menuName = "TowerModifiers/Mortar")]
public class MortarModifier : ModifierBase
{

    public float height = 20f;
    public override void SetupProjectile(TowerProjectile projectile)
    {

        Vector3 targetPos = projectile.target.GetComponent<Collider>().bounds.center;

        // Get Velocity
        projectile.GetStat(Stat.ProjectileVelocity, out var velocity);

        // Find fancy arc
        TowerProjectile.CalculateLateralTrajectory(projectile.transform.position, targetPos, velocity, height, out var force, out var gravity);
        
        // Set custom gravity and force
        ConstantForce constantForce = projectile.GetComponent<ConstantForce>();
        Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
        
        constantForce.force = (constantForce.force + gravity) / 2;
        rigidbody.velocity = (rigidbody.velocity + force) / 2;

        projectile.transform.localScale *= 2;
        // rigidbody.AddForce(force);


        // projectile.GetComponent<ConstantForce>().force = gravity;

        // projectile.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}
