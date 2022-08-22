using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerScript : entityParentClass
{
    private Rigidbody rigidbody;
    [SerializeField]
    [Tooltip("how long the light attack  should linger in seconds")]
    private float lightAttackLingerDuration = 0.25f;
    [SerializeField]
    private float lightAttackCoolDown = 0.2f;
    [SerializeField]
    [Tooltip("how long heavy attack should linger in seconds")]
    private float heavyAttackLingerDuration = 0.25f;
    [SerializeField]
    private float HeavyAttackCoolDown = 0.5f;

    [Tooltip("Iframe duration in seconds")]
    [SerializeField]
    private float IframeDuration;
    [SerializeField]
    private float dashCooldown;
    public bool InIframe;
    [SerializeField]
    private float slidespeed = 20f;
    [SerializeField]
    private GameObject playerObject;

    
    Animator playerAnimator;


    private Vector3 jump;
    public float jumpForce = 2;
    public bool isGrounded;
    public GameObject attackHitboxHeavy;
    public GameObject attackHitboxLight;
    public GameObject gunPrefab;
    private gunScript gun;
    public GameObject wall;
    public GameObject wall2;
    public GameObject wall3;
    public Text infoText;
    public Text loseText;
    public HealthBar healthBar;
    private Vector3 OGplayermodel;
    private Quaternion OGplayerrotation;
    string game;
    private string[] cheatCode;
    private int index;
    protected AudioManager audioManager;
    private bool walking;// Track if the player was walking (for walking sound effect). ~ SK
    public bool playingIntro;

    [SerializeField]
    private bool isImmortal = false;//cheat mode
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //disables attack hitboxes
        attackHitboxHeavy.SetActive(false);
        attackHitboxLight.SetActive(false);

        jump = new Vector3(0.0f, 2.0f, 0);
        gun = gunPrefab.GetComponent<gunScript>();
        game = SceneManager.GetActiveScene().name;
        currentHealth = maxHealth;
        healthBar.MaxHealth(maxHealth);
        playerAnimator=playerObject.GetComponent<Animator>();
        OGplayermodel = playerObject.transform.localPosition;
        OGplayerrotation = playerObject.transform.rotation;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.Play("ShipTheme");
        loseText.gameObject.SetActive(true);
        cheatCode = new string[] { "i", "a", "m", "d", "u", "m", "b" };
        index = 0;
    }
    //checks to see if player is touching the ground, so that air jumps is impossible
    void OnCollisionStay(Collision collider)
    {
        if (collider.gameObject.layer == 8)
        {
            isGrounded = true;
        }

    }
    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.layer == 8)
        {
            isGrounded = false;
        }
    }
    bool isMeleeRunning = false;
    //light attack
    void lightAttackHitboxDuration(float duration)
    {
        StartCoroutine(doLightAttackHitboxDuration(duration));
    }
    IEnumerator doLightAttackHitboxDuration(float duration)
    {
        if (isMeleeRunning)
        {
            yield break;
        }
        playerAnimator.SetTrigger("Light attack");
        audioManager.Play("SwordClang");
        isMeleeRunning = true;
        attackHitboxLight.SetActive(true);
        yield return new WaitForSeconds(duration);
        attackHitboxLight.SetActive(false);
        yield return new WaitForSeconds(lightAttackCoolDown);
        isMeleeRunning = false;
    }
    //heavy attack
    void HeavyAttackHitboxDuration(float duration)
    {
        StartCoroutine(doHeavyAttackHitboxDuration(duration));
    }
    IEnumerator doHeavyAttackHitboxDuration(float duration)
    {
        if (isMeleeRunning)
        {
            yield break;
        }
        playerAnimator.SetTrigger("Heavy attack");
        audioManager.Play("SwordClang");
        isMeleeRunning = true;
        attackHitboxHeavy.SetActive(true);
        yield return new WaitForSeconds(duration);
        attackHitboxHeavy.SetActive(false);
        yield return new WaitForSeconds(HeavyAttackCoolDown);
        isMeleeRunning = false;
    }

    private void OnTriggerEnter(Collider other) // Arif
    {
        if (other.gameObject.layer == 11)// Enemy attack hitbox. ~ SK
        {
                if (other.GetComponent<swordScript>() != null)
                {
                    TakeDamage( other.GetComponent<swordScript>().swordDamage, "SwordThud");
                }
            /*
            else if (other.GetComponent<bulletScript>() != null)
            {
                currentHealth -= other.GetComponent<bulletScript>().bulletDamage;
            }*/
            //Destroy(collision.gameObject);
            
        }
    }

    public IEnumerator OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "water")
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            audioManager.Stop("ShipTheme");
            loseText.gameObject.SetActive(true);
            loseText.text = "DEFEAT";
            yield return new WaitForSeconds(2);
            loseText.gameObject.SetActive(false);
            Debug.Log("Player drowned!");
            SceneManager.LoadScene(game); // reloads the game
        }
        if (collision.gameObject.tag == "upper")
        {
            wall.gameObject.SetActive(true);
            wall3.gameObject.SetActive(true);
        }
        if (collision.gameObject.tag == "below")
            wall2.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        playerObject.transform.localPosition = OGplayermodel;
        playerObject.transform.localRotation = OGplayerrotation;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(game);
        }

        if (playingIntro == false)
        {
            //activates debug mode
            if (isImmortal == false)
            {
                if (Input.anyKeyDown)
                {
                    // Check if the next key in the code is pressed
                    if (Input.GetKeyDown(cheatCode[index]))
                    {
                        if (index < cheatCode.Length)
                        {
                            // Add 1 to index to check the next key in the code
                            index++;
                        }
                    }
                    // Wrong key entered, we reset code typing
                    else
                    {
                        index = 0;
                    }
                }
            }
            // If index reaches the length of the cheatCode string, 
            // the entire code was correctly entered
            if (index == cheatCode.Length)
            {
                // Cheat code successfully inputted!
                isImmortal = true;
                index = 0;
            }
            //control for player
            if (Input.GetMouseButtonDown(0))//LMB pressed?
            {
                lightAttackHitboxDuration(lightAttackLingerDuration);


            }
            if (Input.GetMouseButtonDown(1))//RMB presed?
            {
                HeavyAttackHitboxDuration(heavyAttackLingerDuration);
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)//press space to jumpe
            {

                rigidbody.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))//makes the player dash
            {
                Dash();
            }
            if (InIframe == true)//simualtes dashing
            {
                rigidbody.AddForce(transform.forward * -slidespeed, ForceMode.VelocityChange);
            }

            //rigidbody.velocity = new Vector3(speed * h, 0, speed * v);//moves the player
            rigidbody.velocity = new Vector3(speed * h, rigidbody.velocity.y, speed * v);// Movement that accounts for gravity ~ SK
            /*//if(rigidbody.velocity.x>=0.01f || rigidbody.velocity.z>=0.01f)
            if(Mathf.Abs(rigidbody.velocity.x) >= 0.01f || Mathf.Abs(rigidbody.velocity.z) >= 0.01f)
            {
                playerAnimator.SetBool("Walkfoward", true);
            }
            else
            {
                playerAnimator.SetBool("Walkfoward", false);
            }*/

            if (h != 0 || v != 0)
            {
                playerAnimator.SetBool("Run", true);

                if (walking == false)// If the player wasn't already walking. ~ SK
                {
                    audioManager.Play("Footstep");
                    walking = true;
                }
            }
            else
            {
                playerAnimator.SetBool("Run", false);
                audioManager.Stop("Footstep");
                walking = false;
            }
        }
    }
    private void Dash()//dash function
    {
        
        StartCoroutine(doDash(IframeDuration,dashCooldown));
    }
    private bool dashCorotineRunning=false;
    IEnumerator doDash(float IframeDuration, float dashCooldown)
    {
        if (dashCorotineRunning)
        {
            yield break;
        }
        playerAnimator.SetTrigger("Doge");
        dashCorotineRunning = true;
        InIframe = true;

        Vector3 dash = new Vector3(0.0f, 0.0f, 1.0f);//adds a dash
        

        yield return new WaitForSeconds(IframeDuration);
        InIframe = false;
        yield return new WaitForSeconds(dashCooldown);
        dashCorotineRunning = false;
    }

    IEnumerator Death()
    {
        loseText.gameObject.SetActive(true);
        loseText.text = "DEFEAT";
        yield return new WaitForSeconds(2);
        loseText.gameObject.SetActive(false);
        SceneManager.LoadScene(game);
    }

    public override void TakeDamage(int damage, string sound)
    {
        if (!InIframe&&!isImmortal)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);

            audioManager.Play(sound);

            if (currentHealth <= 0)
            {
                audioManager.Stop("ShipTheme");     
                Debug.Log("You died");
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                StartCoroutine(Death());
            }
        }
    }

    IEnumerator TextDisappear()
    {
        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        infoText.gameObject.SetActive(false);
    }

    public void AddHealth(int health)
    {
        if (MoneyScript.moneyValue < 50)
        {
            infoText.gameObject.SetActive(true);
            infoText.text = "You do not have enough\ncorks to buy health";
            StartCoroutine(TextDisappear());
            Debug.Log("Not enough Corks");
            return;
        }
        else
        {
            currentHealth += health;
            healthBar.SetHealth(currentHealth);
            MoneyScript.moneyValue -= 50;
            infoText.gameObject.SetActive(true);
            infoText.text = "You bought health for 50 corks";
            StartCoroutine(TextDisappear());
            audioManager.Play("SkeletonTalk");
            Debug.Log("Health bought for 50 Corks");
        }
    }
    public void AddSpeed(int Speed)
    {
        if (MoneyScript.moneyValue < 60)
        {
            infoText.gameObject.SetActive(true);
            infoText.text = "You do not have enough\ncorks to buy speed";
            StartCoroutine(TextDisappear());
            Debug.Log("Not enough Corks");
            return;
        }
        else
        {
            speed += Speed;
            MoneyScript.moneyValue -= 60;
            infoText.gameObject.SetActive(true);
            infoText.text = "You bought speed for 60 corks";
            StartCoroutine(TextDisappear());
            audioManager.Play("SkeletonTalk");
            Debug.Log("Speed bought for 60 Corks");
        }
    }
}
