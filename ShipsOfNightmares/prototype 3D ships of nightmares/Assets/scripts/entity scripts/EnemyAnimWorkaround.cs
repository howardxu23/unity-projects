using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimWorkaround: MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();//replace later with shooting animation control
    }

    public void SetBool(string parameter, bool doAnim)
    {
        animator.SetBool(parameter, doAnim);
    }

    public void SetTrigger(string parameter)
    {
        animator.SetTrigger(parameter);
    }
}
