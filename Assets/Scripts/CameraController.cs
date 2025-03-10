using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform firstPersonTransform;
    public Transform thirdPersonTransform;
    Camera playerCamera;
    private bool isThirdPerson = false;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        player.SetActive(isThirdPerson);
    }

    public void OnSwitchCamera()
    {

            isThirdPerson = !isThirdPerson;
            StopAllCoroutines();
            StartCoroutine(SwitchCamera());
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnSwitchCamera();
        }
    }
    IEnumerator SwitchCamera()
    {

        Transform target = isThirdPerson ? thirdPersonTransform : firstPersonTransform;
        player.SetActive(isThirdPerson);
        float duration = .5f;
        float elapsed = 0f;

        Vector3 startPost = playerCamera.transform.localPosition;
        Quaternion startRot = playerCamera.transform.rotation;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            playerCamera.transform.position = Vector3.Lerp(startPost, target.position, t);
            playerCamera.transform.rotation = Quaternion.Slerp(startRot, target.rotation, t);
            yield return null;
        }

        playerCamera.transform.position = target.position;
        playerCamera.transform.rotation = target.rotation;

    }
}
