using UnityEngine;

public class TowerHealth : MonoBehaviour
{

    public InfoBar bar;
    public int health = 100;
    public GameManager manager;


    void Start()
    {
        bar.SetMaxValue(health);
        bar.SetValue(health);
    }

    public void TowerDamage(int damage)
    {
        if (health - damage <= 0)
        {
            Destroy(gameObject);
            manager.gameOver();
        }
        health -= damage;
        bar.SetValue(health);
    }
}