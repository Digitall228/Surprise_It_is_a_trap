using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlock : Obstacle
{
    public float speed;
    public Vector2 direction;
    public int moveTime = 400;
    public override void DoTask()
    {
        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        for (int i = 0; i < moveTime; i++)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);
    }
}
