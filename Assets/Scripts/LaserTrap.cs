using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class LaserTrap : MonoBehaviour
{
    public float detectRange = 300f; // 레이캐스트 범위
    public float damage = 10f;
    private LineRenderer lineRenderer;
    public LayerMask layer;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.useWorldSpace = false;
            lineRenderer.enabled = true;
            Show();
        }
    }
    private void Update()
    {
        DetectPlayer();
        Debug.DrawRay(transform.position, transform.forward, Color.black);
    }



    void DetectPlayer()
    {
        lineRenderer.SetPosition(0, transform.position);
        Vector3 direction = transform.forward;
        RaycastHit hit;
        lineRenderer.SetPosition(0, direction * detectRange);
        if (Physics.Raycast(transform.position, direction, out hit, detectRange, layer))
        {
            CharacterManager.Instance.Player.condition.TakeDamage(damage);
        }


        
    }

    void Show()
    {
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.red;
        lineRenderer.material = material;
        lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.04f;
        lineRenderer.positionCount = 2;
    }
}
