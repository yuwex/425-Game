using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public List<AudioClip> modifierPickUpSounds;
    public List<ModifierBase> inventory = new();
    public UnityEvent onPickup;
    public GameObject player;


    public void Add(ModifierBase modifier)
    {
        SoundManager.Instance.PlayRandomSFXClip(modifierPickUpSounds, player.transform);
        inventory.Add(modifier);
        onPickup.Invoke();
    }
}
