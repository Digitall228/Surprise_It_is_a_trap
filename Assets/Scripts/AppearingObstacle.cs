using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingObstacle : Obstacle
{
    public float speed;
    public float backSpeed;
    public override void DoTask()
    {
        StartCoroutine(Appearing());
    }
    private IEnumerator Appearing()
    {
        Vector2 purpose = transform.position + Vector3.up;

        while (Vector2.Distance(transform.position, purpose) > 0.02f)
        {
            Vector2 nextPosition = Vector2.MoveTowards(transform.position, purpose, speed * Time.deltaTime);
            transform.position = nextPosition;

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.5f);
        purpose -= Vector2.up;

        while (Vector2.Distance(transform.position, purpose) > 0.02f)
        {
            Vector2 nextPosition = Vector2.MoveTowards(transform.position, purpose, backSpeed * Time.deltaTime);
            transform.position = nextPosition;

            yield return new WaitForFixedUpdate();
        }

        isCalled = false;
    }
}
