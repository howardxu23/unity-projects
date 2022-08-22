using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("how long bullet should persist after being fired in seconds")]
    private float bulletDuration = 2;

    public int bulletDamage=0;
    IEnumerator dobulletLifetime(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
    private void destroyBulletInSeconds(float duration)
    {
        StartCoroutine(dobulletLifetime(duration));
    }
    public void Awake()
    {
        destroyBulletInSeconds(bulletDuration);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer==6||other.gameObject.layer==9)
        {
            //print(other.gameObject.GetComponent<entityParentClass>() != null);
            if (other.gameObject.GetComponent<entityParentClass>() != null)
            {
                other.gameObject.GetComponent<entityParentClass>().TakeDamage(bulletDamage, "");
            }
            Destroy(gameObject);
        }
    }
}
