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
    public Transform startPoint;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {

            lineRenderer.enabled = true;
            Show();
        }
    }
    private void Update()
    {
        DetectPlayer();
    }


    void DetectPlayer()
    {
        lineRenderer.SetPosition(0, startPoint.position);
        Vector3 direction = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(startPoint.position, direction, out hit, detectRange, layer))
        {
            CharacterManager.Instance.Player.condition.TakeDamage(damage);
            if(lineRenderer != null)
            {
                lineRenderer.SetPosition(1, hit.point);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, startPoint.position + transform.forward * detectRange);
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
