using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : Obstacle
{
    public Transform rotatingPoint;
    public int direction = 1;
    public int times = 1;
    public float delay = 1;

    public override void DoTask()
    {
        StartCoroutine(Rotating());
    }
    private IEnumerator Rotating()
    {
        for (int i = 0; i < times; i++)
        {
            transform.RotateAround(rotatingPoint.position, Vector3.forward, 90 * direction);

            yield return new WaitForSeconds(delay);
        }

        isCalled = false;
    }
}
