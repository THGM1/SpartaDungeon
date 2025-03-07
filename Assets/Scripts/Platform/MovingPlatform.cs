using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPos;
    public Transform endPos;
    public float moveSpeed;
    private Transform player;
    private void Start()
    {
        startPos = transform.position;
        StartCoroutine(MoveCoroutine());
    }
    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            yield return MoveToPosition(endPos.position);
            yield return MoveToPosition(startPos);
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            player = other.transform;
            player.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            player.SetParent(null);
            player = null;
        }
    }
}
