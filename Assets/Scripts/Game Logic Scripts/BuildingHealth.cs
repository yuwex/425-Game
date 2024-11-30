using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    public int health = 100;
    private GameBoard board;

    void Awake()
    {
        board = FindObjectOfType<GameBoard>();
    }

    public void Hurt(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            board.SetObjectAtPos(gameObject.transform.position, null);
            board.UpdateWalls(gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
