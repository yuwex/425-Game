using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour {
    public float moveSpeed = 1f; // Speed at which the enemy moves towards the center
    private Vector3 targetPosition = Vector3.zero; // Center (origin) target position

    void Start()
    {
        // Rotate the enemy to face the center (origin)
        RotateTowardsCenter();
    }

    void Update()
    {
        // Move the enemy towards the center
        MoveTowardsCenter();

        // Check if the enemy has reached the center (or close enough)
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Destroy the object when it reaches the center
            Destroy(gameObject);
        }
    }

    void RotateTowardsCenter()
    {
        // Calculate the direction vector from the enemy's position to the center
        Vector3 direction = targetPosition - transform.position;

        // Get the rotation angle needed to face the center
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Apply the rotation to the enemy (ignoring the y-axis)
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }

    void MoveTowardsCenter()
    {
        // Move the enemy forward at the defined speed
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
