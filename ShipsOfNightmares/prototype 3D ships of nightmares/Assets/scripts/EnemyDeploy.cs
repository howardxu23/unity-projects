using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeploy : MonoBehaviour
{
    /*
     * deploys the enmeyies when a player enters a trigger zone
    needs: 
    1) a list of enemies
    2) trigger zone where a player would walk into to activate thier AI
    */
    [SerializeField]
    [Tooltip("put the enemies in the zone in here")]
    private GameObject[] Enemies;
    
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;// Hide trigger area. ~ SK

        foreach (GameObject enemy in Enemies)//iterates though each enemy and turns off their AI
        {
            if (enemy.name == "boss")
            {
                var bossAI = enemy.GetComponent<BossAi>();
                bossAI.enabled = false;
            }
            else
            {
                var enemyAI = enemy.GetComponent<enemyScript>();
                enemyAI.enabled = false;
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (GameObject enemy in Enemies)//iterates though each enemy and turns off their AI
            {
                if (enemy == null)// Enemy already killed somehow. ~ SK
                {
                    Debug.LogWarning("Enemy to be spawned not found! (may have already be killed)");
                }
                else
                {
                    if (enemy.name == "boss")
                    {
                        var bossAI = enemy.GetComponent<BossAi>();
                        bossAI.enabled = true;
                    }
                    else
                    {
                        var enemyAI = enemy.GetComponent<enemyScript>();
                        enemyAI.enabled = true;
                    }
                }
            }
            gameObject.SetActive(false);
        }
        
    }
}
