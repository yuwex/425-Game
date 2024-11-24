using UnityEngine;

public class TowerHealth : MonoBehaviour
{

    // public GameObject healthBarUI;
    public InfoBar bar;
    public int health = 100;
    public GameManager manager;

    // private InfoBar uiBar;


    void Start()
    {
<<<<<<< Updated upstream
        bar.setMaxValue(health);
        bar.setValue(health);
=======
        //uiBar = healthBarUI.GetComponentInChildren<InfoBar>();
        bar.SetMaxValue(health);
        bar.SetValue(health);
        //uiBar.SetMaxValue(health);
        //uiBar.SetValue(health);
>>>>>>> Stashed changes
    }

    public void TowerDamage(int damage)
    {
        if (health - damage <= 0)
        {
            Destroy(gameObject);
            manager.gameOver();
        }
<<<<<<< Updated upstream
        health -= damage;
        bar.setValue(health);
=======
        health = 0;
        bar.SetValue(health);
        //uiBar.SetValue(health);
>>>>>>> Stashed changes
    }
}