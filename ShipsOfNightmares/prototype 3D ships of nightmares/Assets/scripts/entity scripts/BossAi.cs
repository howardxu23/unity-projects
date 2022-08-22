using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossAi : entityParentClass
{
    [SerializeField]
    [Tooltip("distance on how the boss would choose to use ranged or meele, also influences charge attacks")]
    private float meeleOrRangedTranisitionDistance = 5f;
    [SerializeField]
    [Tooltip("how far the boss should attempt to strafe around the player, is the transition distance+listed value")]
    private float strafeRadius;
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject swordHitbox;
    
    [SerializeField]
    private GameObject bossObject;
    [SerializeField]
    [Tooltip("how long the sword hitbox should linger in seconds")]
    protected float attackLingerDuration = 0.2f;
    [SerializeField]
    [Tooltip("delay before attack actally connects in seconds")]
    protected float attackDelay = 0;
    [SerializeField]
    private float attackCoolDown = 0.5f;
    [SerializeField]
    [Tooltip("how often the boss should charge in seconds")]
    private float ChargeCooldown;
    [SerializeField]
    [Tooltip("how often the boss would choose to charge at the player when conditions are met, takes int values only")]
    private int chargeChance;
    [SerializeField]
    [Tooltip("how long the boss should charge for in seconds")]
    private float chargeDuration=0.3f;
    [SerializeField]
    [Tooltip("delay before it commits to charge")]
    private float chargeDelay = 1.8f;
    [SerializeField]
    [Tooltip("how fast should the boss charge, is speed+value set")]
    private float chargeSpeed;
    //strafe duration
    [SerializeField]
    private float t_minstrafe=0.1f;
    [SerializeField]
    private float t_maxstrafe=0.4f;
    [SerializeField]
    [Tooltip("how many bullets are fired in a volley")]
    private int NobulletVolley=3;
    [SerializeField]
    [Tooltip("delay on how often the boss fires off a volley")]
    private float gunVolleyCooldown=0.5f;
    [SerializeField]
    [Tooltip("time between each shot in a volley")]
    private float burstDelay=0.1f;
    [SerializeField]
    private int attackRange=2;
    public HealthBar healthBar;
    public Text winText;

    private Rigidbody rigidbody;
    private Transform movePosTransform;
    private GameObject player;
    private Vector3 playerPos;
    protected NavMeshAgent navMeshAgent;
    private float randomStrafeStartTime;
    private float waitStrafeTime;
    private gunScript gunscript;
    [SerializeField]
    private Transform strafeRight;
    [SerializeField]
    private Transform strafeLeft;
    [SerializeField]
    private Transform ChargeLocation;
    private int randStrafeDir;
    private AudioManager audioManager;
    private Animator bossAnimation;
    private Vector3 OGBossPos;
    private Quaternion OGBossRot;
    [SerializeField]
    private bool isCharging = false;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        chargeSpeed = speed + chargeSpeed;
        swordHitbox.SetActive(false);
        navMeshAgent.speed = speed;//sets the object's speed
        strafeRadius += meeleOrRangedTranisitionDistance;
        gunscript = gun.GetComponent<gunScript>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        currentHealth = maxHealth;
        healthBar.MaxHealth(maxHealth);
        OGBossPos = bossObject.transform.localPosition;
        OGBossRot = bossObject.transform.localRotation;
    }
    IEnumerator trackPlayer()//tracks the player movements
    {
        while (true)
        {
            playerPos = player.transform.position;
            yield return null;
        }
    }

    public void Start()
    {
        winText.gameObject.SetActive(true);
        StartCoroutine(trackPlayer());
        bossAnimation = bossObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);//checks distance from player
        //prevents boss from drifting
        
        if (chargeCorotinerunning==false)//regular behavoir
        {
            bossObject.transform.localPosition = OGBossPos;
            bossObject.transform.localRotation = OGBossRot;
            if (distance > meeleOrRangedTranisitionDistance)//use ranged attacks and strafe when the player is away from the boss
            {
                rangedAttackPattern();
                chargeAttack(chargeChance*2);
                gameObject.transform.LookAt(new Vector3(playerPos.x, 0, playerPos.z));
            }
            else if (distance <= meeleOrRangedTranisitionDistance)//go into melee mode when player is close
            {
                meleeAttackPattern();
                chargeAttack(chargeChance);

            }
        }      
        if (isCharging == true) //when charging it locks the boss on a straight path directly ahead
        {
            
            
            StartCoroutine(doRegularMelee(attackLingerDuration,0.05f,0.2f));//swings sword rapidly while charging
        }
        else
        {
            bossAnimation.SetBool("Charge", false);
        }
    }
    //ranged attacks
    void rangedAttackPattern()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        bossAnimation.SetBool("Shoot", true);
        if (distance > meeleOrRangedTranisitionDistance)
        {
            
            randStrafeDir = Random.Range(0, 2);
            randomStrafeStartTime = Random.Range(t_minstrafe, t_maxstrafe);

            if (waitStrafeTime <= 0)
            {
                if (randStrafeDir == 0)
                {
                    navMeshAgent.SetDestination(strafeLeft.position);
                }
                else if (randStrafeDir == 1)
                {
                    navMeshAgent.SetDestination(strafeRight.position);
                }
                waitStrafeTime = randomStrafeStartTime;
            }
            else
            {
                waitStrafeTime -= Time.deltaTime;
            }
        }
        fireVolley();
    }
    void fireVolley()
    {
       StartCoroutine ( doFireVolley(burstDelay, NobulletVolley, gunVolleyCooldown));
    }
     bool isfiringvolley = false;
    IEnumerator doFireVolley(float burstdelay, int bullets, float cooldown)
    {
        
        if (isfiringvolley==true)
        {
            yield break;
        }
        isfiringvolley = true;
        for (int i=0; i < bullets; i++)
        {
            gunscript.FireGun = true;
            audioManager.Play("Gunshot");
            yield return new WaitForSeconds(burstDelay);
        }
        yield return new WaitForSeconds(cooldown);
        isfiringvolley = false;
        
    }
    //meelee functions
    void meleeAttackPattern()
    {
        bossAnimation.SetBool("Shoot", false);
        if (isMeleeRunning == false)
        {
            bossAnimation.SetBool("Run", true);
            navMeshAgent.SetDestination(player.transform.position);//moves towards player
        }
        else
        {
            bossAnimation.SetBool("Run", false);
        }
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            meleeAttack();// Set attack hitbox active for a time. ~ SK
        }
        
    }
    bool isMeleeRunning = false;
    protected virtual IEnumerator doRegularMelee(float duration, float attackcooldown, float attackDelay)
    {
        if (isMeleeRunning)
        {
            yield break;
        }

        if (isCharging == false) { 
            bossAnimation.SetTrigger("Attack");
        }      
        isMeleeRunning = true;
        yield return new WaitForSeconds(attackDelay);
        swordHitbox.SetActive(true);
        audioManager.Play("SwordScrape");
        yield return new WaitForSeconds(duration);
        swordHitbox.SetActive(false);
        yield return new WaitForSeconds(attackcooldown);
        isMeleeRunning = false;
        
    }
    void meleeAttack()
    {
        StartCoroutine(doRegularMelee(attackLingerDuration, attackCoolDown, attackDelay));
    }
    //charge attack system
    void chargeAttack(int chargeChance)
    {
        float chanceValue=Random.Range(0, chargeChance);
        if (chanceValue == 0)//if chance is met initate charge pattern
        {
            StartCoroutine(dochargeattack());
        }
    }
    bool chargeCorotinerunning = false;
    IEnumerator dochargeattack()
    {
        if (chargeCorotinerunning)
        {
            yield break;
        }
        chargeCorotinerunning = true;
        bossAnimation.SetTrigger("battle cry");
        audioManager.Play("BattleCry1");
        bossAnimation.SetBool("Shoot", false);
        bossAnimation.SetBool("Run", false);
        navMeshAgent.SetDestination(transform.position);
        yield return new WaitForSeconds(chargeDelay);
        navMeshAgent.SetDestination(ChargeLocation.position);//advances forwards
        //transform.LookAt(ChargeLocation);
        bossAnimation.SetBool("Charge", true);
        isCharging = true;
        navMeshAgent.speed = chargeSpeed;
        yield return new WaitForSeconds(chargeDuration);
        isCharging = false;
        navMeshAgent.speed = speed;
        yield return new WaitForSeconds(ChargeCooldown);
        chargeCorotinerunning = false;
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

    IEnumerator Victory()
    {
        winText.gameObject.SetActive(true);
        winText.text = "VICTORY";
        audioManager.Stop("DeathRattle");
        audioManager.Stop("BattleCry");
        audioManager.Stop("BattleCry1");
        audioManager.Stop("SwordScrape");
        audioManager.Stop("Gunshot");
        yield return new WaitForSeconds(5);
        winText.gameObject.SetActive(false);
        SceneManager.LoadScene("Credits");
    }

    public override void TakeDamage(int damage, string sound)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        //audioManager.Play("BattleCry");

        if (currentHealth <= 0)
        {           
            StopAllCoroutines();
            MoneyScript.moneyValue += 10;
            Debug.Log("Boss died");
            gun.SetActive(false);
            swordHitbox.SetActive(false);
            bossObject.GetComponent<Animator>().enabled = false;
            StartCoroutine(Victory());
        }
    }
}
