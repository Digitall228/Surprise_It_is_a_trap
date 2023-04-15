using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    public float distanceDifference;

    protected bool isCalled = false;
    protected Transform target;

    protected void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }

    protected void Update()
    {
        if(Vector2.Distance(transform.position, target.position) < distanceDifference && !isCalled)
        {
            isCalled = true;
            DoTask();
        }
    }
    public abstract void DoTask();
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceDifference);
    }
}
