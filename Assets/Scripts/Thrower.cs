using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    public float throwForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out ICharacter character))
        {
            character.Throw(throwForce);
        }
    }
}
