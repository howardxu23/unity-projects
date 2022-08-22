using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour

{
    protected Animator MeleeAnim;


    protected virtual void Start()
    {
        MeleeAnim = GetComponent<Animator>();

       
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



        if (Input.GetKey("d"))
        {
            MeleeAnim.SetBool("Dance", true);
            Debug.Log("Dance mode");
        }
        else
        {
            MeleeAnim.SetBool("Dance", false);
        }




    }
}
