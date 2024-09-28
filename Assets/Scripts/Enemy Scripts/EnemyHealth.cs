using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int health = 100;

    public void Damage(int damage)
    {
        if (health - damage <= 0)
        {
            Die();
        }
        health -= damage;
    }

    void Die() 
    {
        Destroy(gameObject);
    }
}
