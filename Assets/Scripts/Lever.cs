using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Door door;
    private bool isUsed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isUsed && collision.TryGetComponent(out Player player))
        {
            door.Open();
            isUsed = true;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
            Destroy(this);
        }
    }
}
