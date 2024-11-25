using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerRotation : MonoBehaviour
{
    public Transform rotationPoint;
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        if (rotationPoint != null)
        {
            transform.RotateAround(rotationPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
            transform.LookAt(rotationPoint);
        }
    }
}
