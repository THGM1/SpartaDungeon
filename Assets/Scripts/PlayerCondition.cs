using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage;
    public float decayStamina;

    [Header("낙하")]
    private float lastYPosition;
    private float isFalling;
    public float fallDamageThreshold; // 낙하 피해 높이 제한
    public float fallDamageCal; // 낙하 피해 계산


    void Update()
    {
        if (!CharacterManager.Instance.Player.controller.isRunning)
        {
            stamina.Add(stamina.passvieValue * Time.deltaTime);
        }
    }

    public void Die()
    {
        Debug.Log("Die");
    }

    public void UseStamina()
    {
        stamina.Subtract(decayStamina * Time.deltaTime);
    }
    public bool CanUseStamina()
    {
        return stamina.curValue > 0;
    }
}
