using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    private bool hasItem;
    private PlayerCondition condition;
    private PlayerController controller;
    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;
        CharacterManager.Instance.Player.inventory = this;

        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];
        for(int i =0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }
       
    }
    public void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360));
    }
    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            bool isEmpty = !HasAnyItem();
            emptySlot.data = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            if (isEmpty) SelectItem(0);
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    public void SelectItem(int index)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (selectedItem != null && i != index)
            {
                slots[i].Select(false); 
            }
        }
        selectedItem = slots[index];
        selectedItem.Select(true);
        selectedItemIndex = index;
    }
    public void UpdateUI()
    {
        hasItem = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data != null)
            {
                slots[i].Set();
                hasItem = true;
            }
            else
            {
                slots[i].Clear();

            }
        }
        inventoryWindow.SetActive(hasItem);
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data == data && slots[i].quantity < data.maxStackAmount) return slots[i];
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data == null) return slots[i];
        }
        return null;
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            for (int i = 0; i < selectedItem.data.consumables.Length; i++)
            {
                switch (selectedItem.data.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Eat(selectedItem.data.consumables[i].value); break;

                }

            }
            RemoveSelectedItem();
        }
    }

    public void OnDrop()
    {
        ThrowItem(selectedItem.data);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            selectedItem.data = null;
        }
        UpdateUI();
    }

    public void ChangeSelectedItem(int direction)
    {
        selectedItemIndex += direction;

        if (selectedItemIndex >= slots.Length) selectedItemIndex = 0;
        if (selectedItemIndex < 0) selectedItemIndex = slots.Length -1;

        SelectItem(selectedItemIndex);
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (hasItem)
        {
            if (context.phase != InputActionPhase.Performed) return;
            float scrollValue = context.ReadValue<Vector2>().y;

            if (scrollValue > 0)
            {
                ChangeSelectedItem(1);
            }
            else if (scrollValue < 0)
            {
                ChangeSelectedItem(-1);
            }
        }
    }
    private bool HasAnyItem()
    {
        foreach (var slot in slots)
        {
            if (slot.data != null) return true;
        }
        return false;
    }

}
