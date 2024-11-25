using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject player;
    private Animator animator;
    private Vector3 previousPosition;
    public bool isMoving;
    public bool isDead;

    void Start()
    {
        animator = player.GetComponent<Animator>();
        previousPosition = player.transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = player.transform.position;
        Vector3 movement = currentPosition - previousPosition;

        if (isDead)
        {
            animator.SetBool("isDead", isDead);
        }
        else
        {
            isMoving = movement.magnitude > Mathf.Epsilon;
            animator.SetBool("isMoving", isMoving);
        }

        previousPosition = currentPosition;


    }
}
