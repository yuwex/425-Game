using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 target;
    public float velocity;
    public int damage;

    void Start()
    {
        // Find fancy arc
        CalculateLateralTrajectory(transform.position, target, velocity, target.y, out var force, out var gravity);
        
        // Set custom gravity and force
        GetComponent<ConstantForce>().force = gravity;
        GetComponent<Rigidbody>().velocity = force;
    }

    public static bool CalculateLateralTrajectory(Vector3 pos, Vector3 target, float velocity, float maxHeight, out Vector3 force, out Vector3 gravity) {
    
        // Adapted from https://www.forrestthewoods.com/blog/solving_ballistic_trajectories/
        // Credit: Forest Smith
        
        force = Vector3.zero;
        gravity = Vector3.zero;

        Vector3 diff = target - pos;
        Vector3 diffXZ = new(diff.x, 0f, diff.z);
        float lateralDist = diffXZ.magnitude;

        if (lateralDist == 0)
            return false;

        float time = lateralDist / velocity;

        force = diffXZ.normalized * velocity;

        float a = pos.y;        // initial
        float b = maxHeight;    // peak
        float c = target.y;     // final

        gravity.y = 4*(a - 2*b + c) / (time * time);
        force.y = -(3*a - 4*b + c) / time;

        return true;
    }

    private void OnTriggerEnter(Collider other) 
    {

        if (other.gameObject.CompareTag("Enemy")) 
        {
            other.gameObject.GetComponent<EnemyHealth>().Damage(damage);
        }
        Destroy(gameObject);

    }
}
