using UnityEngine;

public class TowerHealth : MonoBehaviour
{

    public InfoBar bar;
    public InfoBar uibar;
    public int health = 100;
    public UiTasks manager;


    void Start()
    {
        bar.SetMaxValue(health);
        bar.SetValue(health);
        uibar.SetMaxValue(health);
        uibar.SetValue(health);
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
        uibar.SetValue(health);
    }
}