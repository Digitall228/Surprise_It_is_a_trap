using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Teleport teleport;
    public bool mustSkip = false;
    public bool mustTeleport = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player) && mustTeleport)
        {
            if(mustSkip)
            {
                mustSkip = false;
                return;
            }

            teleport.mustSkip = true;
            collision.transform.position = (Vector2)teleport.transform.position;
        }
    }
}
