using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public InfoBar bar;
    public float health = 100;
    private float maxHealth;


    void Start()
    {
        maxHealth = health;
        bar.setMaxValue((int)maxHealth);
        bar.setValue((int)maxHealth);
    }

    public void Damage(float damage)
    {
        if (health - damage <= 0)
        {
            Die();
        }
        health -= damage;
        bar.setValue((int)health);
    }

    void Die()
    {
        Destroy(gameObject);
        GameManager.Instance.updateCoins(100);
    }

}
