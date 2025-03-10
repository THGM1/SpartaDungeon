using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumbale,
    Active
}
public enum ConsumableType
{
    Stamina,
    Health
}
public enum ActiveType
{
    None,
    Speed,
    Invincible
}
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}
[System.Serializable]
public class ItemDataActive
{
    public ActiveType type;
    public float value;
    public float duration;
}
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab; 

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Active")]
    public ItemDataActive active;
}
