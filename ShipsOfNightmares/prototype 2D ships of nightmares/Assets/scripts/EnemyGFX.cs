using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath;
    public GameObject enemyAttackHitbox;

    // Update is called once per frame
    void Update()
    {
        // Flip sprite (and attack hitbox child) depending on direction it wants to move to. ~SK
        if (aiPath.desiredVelocity.x >= 0.01f)// If wanting to go left. ~SK
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)// If wanting to go right. ~SK
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
