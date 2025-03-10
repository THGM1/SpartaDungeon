using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPlatform : MonoBehaviour
{
    public float launchPower = 1000f;
    public float angle = 45f;
    public float delay = 0.5f;
    private bool isWait = false;
    public Transform firePsosition;
    Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {
        rb = other.GetComponent<Rigidbody>();
        if(rb != null && !isWait) StartCoroutine(LaunchCo(rb, other.transform));
    }

    IEnumerator LaunchCo(Rigidbody rb, Transform player)
    {
        isWait = true;
        player.position = transform.position;
    
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);


        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, elapsedTime);
            yield return null;

        }

        Vector3 launchDirection = transform.up;
        rb.velocity = Vector3.zero;
        rb.AddForce(launchDirection * launchPower, ForceMode.Impulse);

        yield return new WaitForSeconds(delay);
        transform.rotation = Quaternion.identity;
        player.transform.rotation = Quaternion.identity;

        Debug.Log(rb.velocity);
        isWait = false;
    }

    IEnumerator test(Rigidbody rb)
    {
        yield return new WaitForSeconds(delay);
        Vector3 launchDirection = transform.up;
        rb.AddForce(Vector2.up * launchDirection * launchPower, ForceMode.Impulse);
    }
}
