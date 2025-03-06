using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData data;

    public UIInventory inventory;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    public bool selected;
    public int index;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = selected;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = data.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if(outline != null)
        {
            outline.enabled = selected;
        }
    }

    public void Clear()
    {
        data = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

}
