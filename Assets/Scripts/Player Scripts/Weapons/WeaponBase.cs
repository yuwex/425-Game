using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class WeaponBase : ScriptableObject
{
    [Header("Weapon Stats")]
    public float attackDistance;
    public float attackDelay;
    public float attackSpeed;
    public int attackDamage;

    [Header("Model Info")]
    public GameObject prefab;
    public Vector3 position;
    public Vector3 scale;
    public int animateRotation;
    
    private LayerMask attackLayer;
    private string enemyTag;

    private GameObject gameObject;
    private MeshRenderer[] meshes;

    private Transform fppCamera;

    public virtual void Init(LayerMask attackLayer, string enemyTag) {
        this.attackLayer = attackLayer;
        this.enemyTag = enemyTag;
        fppCamera = Camera.main.transform;

        gameObject = Instantiate(prefab, fppCamera.transform.position + position, Quaternion.identity);
        gameObject.transform.localScale = scale;
        gameObject.transform.parent = Camera.main.transform;

        meshes = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = false;
        }
    }

    public virtual void Damage() {
        if (Physics.Raycast(fppCamera.position, fppCamera.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag(enemyTag))
            {
                target.GetComponent<EnemyHealth>().Damage(attackDamage);
            }
        }
    }

    public virtual void ToggleMesh() {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = !mesh.enabled;
        }
    }

    public virtual void Animate() {
        gameObject.transform.rotation *= Quaternion.Euler(animateRotation * Time.deltaTime / attackSpeed, 0, 0); 
    }

    public virtual void Reset() {
        gameObject.transform.rotation = fppCamera.transform.rotation;
    }
}
