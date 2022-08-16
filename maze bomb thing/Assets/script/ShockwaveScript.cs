using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveScript : MonoBehaviour
{

     int rateOfExpamnsion=9;
    float blastwaveCurrentsize=3;
    float maxBlastwavesize = 19.5858f;
    bool hasExpanded = false;
    SpriteRenderer shockRenderer;
    bool freezeblast = false;
    float blastWaveFrozensize;
    private void Start()
    {
        shockRenderer=GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        
        if (blastwaveCurrentsize <= maxBlastwavesize && !hasExpanded )
        {
            if (!freezeblast)
            {

                blastwaveCurrentsize += rateOfExpamnsion * Time.deltaTime;
                blastWaveFrozensize = blastwaveCurrentsize;//to stop the blast wave from going though indestrutable wall

                transform.localScale = new Vector3(blastwaveCurrentsize, 2.89902f, 2.89902f);
            }else
            {
                blastwaveCurrentsize += rateOfExpamnsion * Time.deltaTime;
            }

        }
        else
        {
            hasExpanded = true;
            blastwaveCurrentsize -= rateOfExpamnsion * Time.deltaTime;
            if (blastwaveCurrentsize <= blastWaveFrozensize)
            {
                transform.localScale = new Vector3(blastwaveCurrentsize, 2.89902f, 2.89902f);
            }
        }
        


        if (blastwaveCurrentsize <= 0)
        {
            shockRenderer.enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "full metal wall")
        {
            freezeblast = true;
        }
        else if(collision.gameObject.tag=="wood wall")
        {
            Destroy(collision.gameObject);

        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "trophy")
        {
            Destroy(collision.gameObject);
        }
    }
}
