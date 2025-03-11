using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour, IInteractable
{
    private bool isOpened = false;
    public string GetInteractPrompt()
    {
        string str = $"Ŭ���Ͽ� ��ȣ�ۿ�";
        return str;
    }

    public void OnInteract()
    {
        if (!isOpened)
        {
            transform.rotation = Quaternion.Euler(0, -80, 0);
            isOpened = true;
        }
        else
        {
            transform.rotation =  Quaternion.identity;
            isOpened = false;
        }
        
    }
}
