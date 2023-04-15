using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2 endPoint;
    public float speed;

    public void Open()
    {
        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        while (transform.position.y < endPoint.y)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, endPoint);
    }
}
