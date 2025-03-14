﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public LayerMask ground;
    public bool canMove = true;
    public bool isClimbing = false;

    [Header("카메라")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;
    private Rigidbody rb;
    public Action inventory;
    Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            Move(); 
        }
        if (isClimbing)
        {
            Climb();
        }
    }
    private void LateUpdate()
    {
        CameraLook();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) // 누르고 있을 때
        {
            curMove = context.ReadValue<Vector2>(); //이동
            animator.SetBool("Walk", true);

        }
        else if (context.phase == InputActionPhase.Canceled) // 뗐을 때
        {
            curMove = Vector2.zero; //멈춤
            animator.SetBool("Walk", false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded()) // 눌렀을 때
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }
     
    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            isRunning = true;
            animator.SetBool("Run", true);
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
            animator.SetBool("Run", false);
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

    public bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * .2f) + (transform.up * .01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * .2f) + (transform.up * .01f), Vector3.down),
            new Ray(transform.position + (transform.right * .2f) + (transform.up * .01f), Vector3.down),
            new Ray(transform.position + (-transform.right * .2f) + (transform.up * .01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, ground)) return true;
        }
        return false;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot ,0 , 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
        }
    }

   public void OnThrow(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            CharacterManager.Instance.Player.inventory.OnDrop();
        }
    }
    public void ActivateItem(ItemData data)
    {
        StartCoroutine(ApplyItem(data));
    }

    IEnumerator ApplyItem(ItemData item)
    {
        switch (item.active.type)
        {
            case ActiveType.Speed:
                moveSpeed += item.active.value;
                break;
            case ActiveType.Invincible:
                CharacterManager.Instance.Player.condition.invincible = true;
                break;
        }

        yield return new WaitForSeconds(item.active.duration);

        switch (item.active.type)
        {
            case ActiveType.Speed:
                moveSpeed -= item.active.value;
                break;
            case ActiveType.Invincible:
                Debug.Log("해제");
                CharacterManager.Instance.Player.condition.invincible = false;
                break;
        }
        yield return null;
    }

    void Climb()
    {
        if (isClimbing)
        {
            canMove = false;
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(Vector3.up * 1f, ForceMode.VelocityChange);
                CharacterManager.Instance.Player.condition.UseStamina();
            }
            else
            {
                rb.AddForce(Vector3.down * .4f, ForceMode.VelocityChange);
            }


        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            canMove = true;
            isClimbing = false;
            rb.useGravity = true;
        }
    }
}
