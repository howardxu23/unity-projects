using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedEnemyScript : enemyScript
{
    [SerializeField]
    private GameObject gunObject;
    private gunScript gunscript;
    //public bool moving = true;

    protected override void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        gunscript = gunObject.GetComponent<gunScript>();
        enemyAnimatior = enemyObject.GetComponent<Animator>();//replace later with shooting animation control

        currentHealth = maxHealth;
        healthBar.MaxHealth(maxHealth);
        navMeshAgent.stoppingDistance = attackRange;
        navMeshAgent.speed = speed;

        base.deathSound = "PirateDeath";
        base.voice = Random.Range(1, 3);

    }

    protected override IEnumerator doAttackHitboxDuration(float duration, float attackcooldown, float attackDelay)
    {
        if (isCoRotineExeucating)
        {
            yield break;
        }
        isCoRotineExeucating = true;
        //enemyAnimatior.SetTrigger("Shoot");
        enemyAnimWA.SetTrigger("Shoot");
        moving = false;
        //StartCoroutine(RotateOverTime(enemyObject, transform.rotation * Quaternion.Euler(0f, 90f, 0f), duration));
        //StartCoroutine(RotateOverTime(enemyObject.transform.GetChild(0).gameObject, enemyObject.transform.rotation * Quaternion.Euler(0f, 90f, 0f), attackDelay));
        //StartCoroutine(RotateOverTime(enemyObject, Quaternion.Euler(0f, 90f, 0f), attackDelay));
        yield return new WaitForSeconds(attackDelay);
        audioManager.Play("Gunshot");
        gunscript.FireGun = true;
        //StartCoroutine(RotateOverTime(enemyObject, transform.rotation * Quaternion.Euler(0f, -90f, 0f), duration));
        //StartCoroutine(RotateOverTime(enemyObject, Quaternion.Euler(0f, -90f, 0f), duration));
        yield return new WaitForSeconds(duration);

        //attackReady = false;
        //StartCoroutine(RotateOverTime(enemyObject, transform.GetChild(0).rotation * Quaternion.Euler(0f, 0f, 0f), attackcooldown));
        yield return new WaitForSeconds(attackcooldown);
        moving = true;
        // attackReady = true;
        isCoRotineExeucating = false;
    }

    protected override void Update()
    {

        if (currentHealth > 0)
        {
            transform.LookAt(new Vector3(movePositionTransform.position.x, transform.position.y, movePositionTransform.position.z));
        }
        base.Update();

        /*
        //ANIMATION NOTES - DRS
        "Dance" - For ending credits
        "Death" - Plays death anim TBD - Combine with partical effect
        "Run" - Run animation
        "Shoot" - shoots single bullet
        */
    }

    public IEnumerator RotateOverTime(GameObject objectToRot, Quaternion endRot, float rotTime)
    {
        float elapsedTime = 0;
        Quaternion startingRot = objectToRot.transform.rotation;

        while (elapsedTime < rotTime)
        {
            objectToRot.transform.rotation = Quaternion.Lerp(startingRot, endRot, (elapsedTime / rotTime));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToRot.transform.rotation = endRot;
    }
}
