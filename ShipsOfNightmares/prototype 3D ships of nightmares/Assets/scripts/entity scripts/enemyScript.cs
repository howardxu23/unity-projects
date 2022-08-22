using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[System.Serializable]
public class enemyScript : entityParentClass
{
    protected NavMeshAgent navMeshAgent;
    protected Transform movePositionTransform;
    protected Rigidbody rigidbody;
    protected GameObject player;
    
    public float attackRange = 2f;
    protected bool attackReady = true;
    public float attackCoolDown = 0.5f;

    [SerializeField]
    protected GameObject enemyObject;
    protected Animator enemyAnimatior;
    protected EnemyAnimWorkaround enemyAnimWA;
    [SerializeField]
    [Tooltip("how long the sword hitbox should linger in seconds")]
    protected float attackLingerDuration = 0.25f;
    [SerializeField]
    [Tooltip("delay before attack actally connects in seconds")]
    protected float attackDelay=0;
    [SerializeField]
    protected bool moving = true;


    public Text winText;
    public HealthBar healthBar;

    [SerializeField]
    private GameObject swordHitbox;

    protected AudioManager audioManager;
    [HideInInspector]
    public string deathSound;// Different sound between ranged and melee enemies. ~ SK
    [HideInInspector]
    public int voice;// Determines if their voice sounds will be low, normal or high pitched (1,2, or 3). ~ SK

    protected void Awake()
    {
        enemyAnimWA = gameObject.GetComponent<EnemyAnimWorkaround>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        movePositionTransform = player.transform;
        
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        navMeshAgent.speed = speed;//sets the object's speed
        
    }


    protected virtual void Start()
    {

        rigidbody = GetComponent<Rigidbody>();
        swordHitbox.SetActive(false);
        enemyAnimatior = enemyObject.GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.MaxHealth(maxHealth);

        deathSound = "DeathRattle";
        voice = Random.Range(1, 3);
        
    }
    protected void attackHitboxDuration(float duration,float attackcooldown,float attackdelay)
    {

        StartCoroutine(doAttackHitboxDuration(duration, attackcooldown,attackdelay));
    }
    protected bool isCoRotineExeucating=false;
    protected virtual IEnumerator doAttackHitboxDuration(float duration,float attackcooldown, float attackDelay)
    {
        if (isCoRotineExeucating)
        {
            yield break;
        }
        isCoRotineExeucating = true;
        //enemyAnimatior.SetTrigger("Attack");
        enemyAnimWA.SetTrigger("Attack");
        moving = false;
        yield return new WaitForSeconds(attackDelay);
        audioManager.Play("SwordScrape");
        swordHitbox.SetActive(true);
        yield return new WaitForSeconds(duration);
        swordHitbox.SetActive(false);
        moving = true;
        //attackReady = false;
        yield return new WaitForSeconds(attackcooldown);
       // attackReady = true;
        isCoRotineExeucating = false;
    }
    /*
    void attackCoolDownDur(float duration)
    {
        StartCoroutine(doAttackCoolDown(duration));
    }
    IEnumerator doAttackCoolDown(float duration)
    {
        attackReady = false;
        yield return new WaitForSeconds(duration);
        attackReady = true;
    }*/

    // moves enemy towards the player
    protected virtual void Update()
    {
        if (currentHealth > 0)
        {
            // AI navigation. ~ SK
            if (moving == true)
            {
                navMeshAgent.destination = movePositionTransform.position;
                //enemyAnimatior.SetBool("Run", true);
                enemyAnimWA.SetBool("Run", true);
            }
            else
            {
                //navMeshAgent.destination = gameObject.transform.position;
                //enemyAnimatior.SetBool("Run", false);
                enemyAnimWA.SetBool("Run", false);
            }
            //enemyAnimatior.SetBool("Run", moving);
            /*
            if (transform.hasChanged ==false)//checks to see enemy is moving or not, so plays the not moving animation.
            {
                enemyAnimatior.SetBool("Running", false);
            }
            else
            {
                enemyAnimatior.SetBool("Running", true);
            }*/
            // If enemy and player are close enough AND attack is ready... ~ SK
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                attackHitboxDuration(attackLingerDuration, attackCoolDown, attackDelay);// Set attack hitbox active for a time. ~ SK
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (collision.GetComponent<swordScript>() != null)
            {
                TakeDamage(collision.GetComponent<swordScript>().swordDamage, "");
            }
            /*
            else if (collision.GetComponent<bulletScript>() != null)
            {
                currentHealth -= collision.GetComponent<bulletScript>().bulletDamage;
            }*/
            //Destroy(collision.gameObject);
        }
    }
    /*
    void Victory()
    {
        winText.gameObject.SetActive(true);
        winText.text = "VICTORY";
    }*/

    public override void TakeDamage(int damage, string sound)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        audioManager.Play("BattleCry" + voice);

        if (currentHealth <= 0 && died == false)
        {
            Debug.Log("Enemy died");
            died = true;
            MoneyScript.moneyValue += 10;
            transform.parent.GetComponent<NumberOfEnemies>().enemiesDead += 1;
            audioManager.Stop("BattleCry" + voice);
            audioManager.Play(deathSound + voice);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            transform.GetChild(2).gameObject.SetActive(false);

            StartCoroutine(deathAnimation());
        }

    }
    IEnumerator deathAnimation()
    {
        //enemyAnimatior.SetBool("Death", true);
        enemyAnimWA.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        if (deathSound == "DeathRattle") StartCoroutine(MoveOverTime(transform.GetChild(0).gameObject, transform.position + new Vector3(0, -0.8f, 0), 1.8f));
        else if (deathSound == "PirateDeath") StartCoroutine(MoveOverTime(transform.GetChild(0).gameObject, transform.position + new Vector3(0, -0.5f, 0), 1.8f));
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    public IEnumerator MoveOverTime(GameObject objectToMove, Vector3 endPos, float moveTime)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;

        while (elapsedTime < moveTime)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = endPos;
    }
}
