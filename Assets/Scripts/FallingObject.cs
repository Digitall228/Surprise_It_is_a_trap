using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : Obstacle
{
    private Vector2 startPoint;
    public Vector2 endPoint;
    public float x;
    public float y;
    public float speed;
    public bool isBacked = true;

    protected new void Start()
    {
        base.Start();
        startPoint = transform.position;
    }
    protected new void Update()
    {
        if (Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(target.position.x, 0)) < Mathf.Abs(x) && Vector2.Distance(new Vector2(0, transform.position.y), new Vector2(0, target.position.y)) < Mathf.Abs(y) && !isCalled)
        {
            isCalled = true;
            DoTask();
        }
    }
    public override void DoTask()
    {
        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        while (transform.position.y > endPoint.y)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            //transform.position = Vector2.Lerp(transform.position, endPoint, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1);
        if (isBacked)
        {
            while (transform.position.y < startPoint.y)
            {
                transform.Translate(Vector2.up * (speed / 3) * Time.deltaTime);
                //transform.position = Vector2.Lerp(startPoint, transform.position, speed/3 * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(2);
            isCalled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, endPoint);
        Gizmos.DrawLine(new Vector2(transform.position.x - x, transform.position.y), new Vector2(transform.position.x + x, transform.position.y));
        Debug.DrawLine(transform.position + Vector3.right/4, new Vector3(transform.position.x + 0.25f, transform.position.y - y), Color.red);
    }
}
