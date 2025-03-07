using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damgae);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage;
    public float decayStamina;

    [Header("낙하")]
    private bool isFalling;
    private float fallTime = 0f;
    public float fallDamageThreshold; // 낙하 시간 제한
    public float fallDamageCal; // 낙하 피해 계산


    void Update()
    {
        if (!CharacterManager.Instance.Player.controller.isRunning)
        {
            stamina.Add(stamina.passvieValue * Time.deltaTime);
        }
        CalFallDamage();
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

    private void CalFallDamage()
    {
        if (!CharacterManager.Instance.Player.controller.IsGrounded())
        { // 공중에 떠 있을 때
            fallTime += Time.deltaTime;
        }
        else // 착지 시
        {
            if(fallTime >= fallDamageThreshold)
            {
                float damage = fallTime * fallDamageCal;
                TakeDamage(damage);
            }
            fallTime = 0f; // 낙하 시간 초기화
        }
    }

    public void TakeDamage(float damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
    public void Eat(float amount)
    {
        StartCoroutine(HealthCoroutine(10f, amount));
    }
    IEnumerator HealthCoroutine(float duration, float amount)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            health.Add(amount);
            yield return new WaitForSeconds(1f);
            elapsed += Time.deltaTime;
        }
        yield return null;
    }
}
