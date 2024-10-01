using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public InfoBar bar;
    public int health = 100;
    private int maxHealth;

    void Start() {
        maxHealth = health;
        bar.setMaxValue(maxHealth);
        bar.setValue(maxHealth);
    }

    public void Damage(int damage)
    {
        if (health - damage <= 0)
        {
            Die();
        }
        health -= damage;
        bar.setValue(health);
    }

    void Die() 
    {
        Destroy(gameObject);
    }
}
