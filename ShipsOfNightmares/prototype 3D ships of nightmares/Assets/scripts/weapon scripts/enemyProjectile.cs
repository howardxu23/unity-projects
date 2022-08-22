using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectile : bulletScript
{
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 7 || other.gameObject.layer == 9)
        {
            if (other.GetComponent<entityParentClass>() != null)
            {
                other.GetComponent<entityParentClass>().TakeDamage(bulletDamage, "");
            }
            Destroy(gameObject);
        }
    }
}
