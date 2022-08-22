using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("hitbox is what interactes with munitions and weapons, rather then coliding with enviroment objects.")]
    private GameObject enemyCollision;
    public GameObject enemyAttackHitbox;
    public GameObject player;
    public float enemyAttackRange = 1.5f;
    private bool enemyAttackReady = true;
    public float enemyAttackCoolDown = 0.5f;
    public float enemyAttackLingerDuration = 0.25f;
    public int health=1;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(6, 8);
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("Sword", false);// Set to false (assumes player didn't attack). ~ SK
        
        // If enemy and player or close enough AND attack is ready... ~ SK
        if (Vector3.Distance(transform.position, player.transform.position) <= enemyAttackRange && enemyAttackReady)
        {
            enemyAttackHitboxDuration(enemyAttackLingerDuration);

            enemyAttackCoolDownDur(enemyAttackCoolDown);

            //animator.SetBool("Sword", true);// Play sword attack animation. ~ SK
        }
    }
    void enemyAttackHitboxDuration(float duration)
    {
        StartCoroutine(doEnemyAttackHitboxDuration(duration));
    }
    IEnumerator doEnemyAttackHitboxDuration(float duration)
    {
        enemyAttackHitbox.SetActive(true);
        yield return new WaitForSeconds(duration);
        enemyAttackHitbox.SetActive(false);
    }
    void enemyAttackCoolDownDur(float duration)
    {
        StartCoroutine(doEnemyAttackCoolDown(duration));
    }
    IEnumerator doEnemyAttackCoolDown(float duration)
    {
        enemyAttackReady = false;
        yield return new WaitForSeconds(duration);
        enemyAttackReady = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Enemy died");
        //Destroy(gameObject);
        gameObject.SetActive(false);// Less intense than Destroy(gameObject); ~ SK
    }
}
