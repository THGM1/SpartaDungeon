using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; // 현재 값
    public float maxValue; // 최대값
    public float passvieValue; // 지속적으로 증감하는 값
    public Image uiBar;

    private void Start()
    {
        curValue = maxValue;
    }
    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
