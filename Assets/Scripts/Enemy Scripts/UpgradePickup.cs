using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    public ModifierBase modifier;

    public string playerTag = "Player";

    [Header("Animation")]
    public float rotationsPerSecond = 0.15f;
    public float oscilationsPerSecond = 0.5f;
    public float oscilationAmount = 0.5f;
    private MeshRenderer meshRenderer;
    private float startY;
    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;

        startY = transform.position.y;

        meshRenderer = GetComponent<MeshRenderer>();

        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(90, 0, 0));

        if (modifier)
            meshRenderer.material = modifier.modifierMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * 360 * rotationsPerSecond));
        transform.position = new Vector3(transform.position.x, startY + Mathf.Sin((Time.time + spawnTime) * Mathf.PI * oscilationsPerSecond) * oscilationAmount, transform.position.z);
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(playerTag))
        {
            other.GetComponent<PlayerInventory>().inventory.Add(modifier);
            Destroy(gameObject);
        }
    }
}
