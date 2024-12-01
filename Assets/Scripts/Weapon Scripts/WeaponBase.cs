using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ScriptableObject
{

    [Header("Public Fields")]
    public bool attacking;

    [Header("Model Info")]
    public GameObject prefab;
    public Vector3 position;
    public Vector3 scale;
    public int animateRotation;

    protected LayerMask attackLayer;
    protected string enemyTag;
    protected TowerSpawner towerSpawner;
    protected PlayerMovement player;

    protected GameObject gameObject;
    protected MeshRenderer[] meshes;

    protected Transform fppCamera;

    public void ToggleMesh() {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = !mesh.enabled;
        }
    }

    public void Init(LayerMask attackLayer, string enemyTag, TowerSpawner towerSpawner, PlayerMovement player) {
        this.attackLayer = attackLayer;
        this.enemyTag = enemyTag;
        this.towerSpawner = towerSpawner;
        this.player = player;

        attacking = false;
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

    public virtual void Attack() {}

    public virtual void Release() {}

    public virtual void Animate() {}

}
