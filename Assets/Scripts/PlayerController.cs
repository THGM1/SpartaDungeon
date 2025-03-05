using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState { Idle, Walk, Run, Jump }
public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpForce;
    private Vector2 curMove;
    public bool isRunning = false;

    [Header("카메라")]
    public Transform cameraContainer;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Move();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) // 누르고 있을 때
        {
            curMove = context.ReadValue<Vector2>(); //이동
        }
        else if (context.phase == InputActionPhase.Canceled) // 뗐을 때
        {
            curMove = Vector2.zero; //멈춤
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) // 눌렀을 때
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }
     
    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            isRunning = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
        }
    }
    private void Move()
    {
        float speed = isRunning && CharacterManager.Instance.Player.condition.CanUseStamina() ? runSpeed : moveSpeed;
        if (isRunning)
        {
            CharacterManager.Instance.Player.condition.UseStamina();
        }
        Vector3 dir = transform.forward * curMove.y + transform.right * curMove.x;
        dir *= speed;
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }
}
