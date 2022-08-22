using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//deactuvated script

public class meleeanimcontrol : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform movePositionTransform;
    private Rigidbody rigidbody;
    private GameObject player;
    [SerializeField]
    [Tooltip("how long the sword hitbox should linger in seconds")]
    public float attackRange = 2f;
    private bool attackReady = true;
    public float attackCoolDown = 0.5f;
    private float attackLingerDuration = 0.25f;
    Animator animator;
    public int speed = 1;
    public bool Attack = false;
    public bool Running;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {


        //Animator Test code -DRS

        //Debug.Log("Attack" + Attack);

        if (Input.GetKey("t"))
        {
            animator.SetBool("Attack", true);
            Attack = true;
        }

        //Animator Test code -DRS
        if (Input.GetKey("y"))
        {
            animator.SetBool("Attack", false);
           
            Attack = false;
        }
        animator.SetBool("Attack", Attack);
    }
}
