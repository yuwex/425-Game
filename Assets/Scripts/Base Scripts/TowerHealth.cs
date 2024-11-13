using UnityEngine;

public class TowerHealth : MonoBehaviour
{

    public InfoBar bar;
    public int health = 100;
    public GameManager manager;


    void Start()
    {
        bar.setMaxValue(health);
        bar.setValue(health);
    }

    public void TowerDamage(int damage)
    {
        if (health - damage <= 0)
        {
            Destroy(gameObject);
            manager.gameOver();
        }
        health -= damage;
        bar.setValue(health);
    }
}