using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starpunchExplosion : MonoBehaviour
{
    public GameObject player;
    brachycontrol Player;
    Animator animator;
    // Update is called once per frame
    void Update()
    {
        animator = GetComponent<Animator>();
        Player = player.GetComponent<brachycontrol>();
        if (Player.starPunched == true)
        {
            animator.SetTrigger("star punch");
        }
    }
}
