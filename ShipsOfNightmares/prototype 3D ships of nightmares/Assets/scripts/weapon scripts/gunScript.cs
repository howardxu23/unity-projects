using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*for an AI to fire this, the when the AI "fires" it will need to get the gun gameobject, 
 * then get this script, and then set the "FireGun" varible to true, and the gun would fire once.
 */
public class gunScript : MonoBehaviour
{
    public GameObject bulletObject;
    private Rigidbody bullet;
    bulletScript bulletscript;
    public bool FireGun = false;
    private Transform GunTransform;
    [SerializeField]
    private float bulletSpeed=10;
    public int bulletDamage = 10;
    // Start is called before the first frame update
    void Start()
    {
        bullet = bulletObject.GetComponent<Rigidbody>();
        GunTransform = gameObject.transform;
        bulletscript=bullet.GetComponent<bulletScript>();
        bulletscript.bulletDamage = bulletDamage;
    }
    private void fire()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, GunTransform.position, GunTransform.rotation);
        bulletClone.velocity = GunTransform.forward * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (FireGun == true)
        {
            fire();
            FireGun = false;
        }
    }
}
