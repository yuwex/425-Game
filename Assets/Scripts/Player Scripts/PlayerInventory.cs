using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public List<ModifierBase> inventory = new();
    public UnityEvent onPickup;

    public void Add(ModifierBase modifier)
    {
        inventory.Add(modifier);
        onPickup.Invoke();
    }
}
