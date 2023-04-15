using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    public Transform[] wayPoints;
    public float speed;
    public float visibilityX;
    public float visibilityY;
    [SerializeField]
    private Transform nextWayPoint;

    private Transform target;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(Vector2.Distance(new Vector2(0, transform.position.y), new Vector2(0, target.position.y)) < visibilityY && Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(target.position.x, 0)) < visibilityX)
        {
            if(nextWayPoint == null)
            {
                nextWayPoint = FindTheNearestWayPoint();
            }
            if(Vector2.Distance(transform.position, nextWayPoint.position) < 0.1f)
            {
                animator.SetTrigger("Idle");
                nextWayPoint = FindTheNearestWayPoint();
                return;
            }
            animator.SetTrigger("Run");

            Vector2 nextPosition = Vector2.MoveTowards(transform.position, nextWayPoint.position, speed * Time.deltaTime);
            transform.position = nextPosition;

            if (Vector2.Distance(nextWayPoint.position, transform.position) < 0.5f)
            {
                nextWayPoint = FindTheNearestWayPoint();
            }
        }
        else
        {
            animator.SetTrigger("Idle");
        }

    }
    private Transform FindTheNearestWayPoint()
    {
        float distance = 1000f;
        int index = 0;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            if(Vector2.Distance(wayPoints[i].position, target.position) < distance)
            {
                distance = Vector2.Distance(wayPoints[i].position, target.position);
                index = i;
            }
        }

        return wayPoints[index];
    }
    private void Move(float x)
    {
        rb.velocity = new Vector2(x * speed, rb.velocity.y);
        transform.localScale = x > 0 ? new Vector3(48, 48, 1) : new Vector3(-48, 48, 1);
    }
    public void Throw(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(visibilityX*2f, visibilityY*1.5f, 1));
    }
}
