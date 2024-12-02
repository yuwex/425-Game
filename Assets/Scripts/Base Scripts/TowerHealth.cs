using UnityEngine;
using System.Collections.Generic;


public class TowerHealth : MonoBehaviour
{
    public GameObject tower;
    public InfoBar bar;
    public InfoBar uibar;
    public int health = 100;
    public UiTasks manager;
    public List<AudioClip> towerDamageSounds;
    public AudioClip towerDestroyedSound;


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
            SoundManager.Instance.PlaySFXClip(towerDestroyedSound, tower.transform);
            Destroy(gameObject);
            manager.gameOver();
        }
        health -= damage;
        bar.SetValue(health);
        uibar.SetValue(health);
        SoundManager.Instance.PlayRandomSFXClip(towerDamageSounds, tower.transform);

    }
}