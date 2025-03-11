using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance; // 최대 체크 거리
    public LayerMask layerMask;

    public GameObject curInteractGO;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptTxt;
    private Camera camera;


    private void Start()
    {
        camera = Camera.main;

    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGO)
                {
                    curInteractGO = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else if(Physics.Raycast(ray, out hit, 1f, 1 << 9))
            {
                CharacterManager.Instance.Player.controller.isClimbing = true;
            }
            else
            {
                curInteractGO = null;
                curInteractable = null;
                promptTxt.gameObject.SetActive(false);
                CharacterManager.Instance.Player.controller.isClimbing = false;
            }
        }
    }

    private void SetPromptText()
    {
        promptTxt.gameObject.SetActive(true);
        promptTxt.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGO = null;
            curInteractable = null;
            promptTxt.gameObject.SetActive(false);
        }
    }
}
