using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour
{
    private Vector2 startPoint;
    public Vector2 endPoint;
    public float speed;

    private void Start()
    {
        startPoint = transform.position;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector2 purpose = endPoint;

        while (true)
        {
            Vector2 nextPosition = Vector2.Lerp(transform.position, purpose, speed * Time.deltaTime);
            transform.position = nextPosition;

            if(Vector2.Distance(transform.position, purpose) < 0.1f)
            {
                purpose = purpose == endPoint ? startPoint : endPoint;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, endPoint, Color.yellow);
    }
}
