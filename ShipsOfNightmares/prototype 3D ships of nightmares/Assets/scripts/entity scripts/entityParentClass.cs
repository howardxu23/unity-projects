using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entityParentClass : MonoBehaviour
{
    public float speed = 1;
    public int maxHealth = 30;
    public int currentHealth;
    public bool died = false;

    public virtual void TakeDamage(int damage, string sound)
    {
        
    }
}
