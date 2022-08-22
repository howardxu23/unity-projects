using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxScript : MonoBehaviour
{
    //attach to the background that needs paralax scrolling
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    [SerializeField]
    [Tooltip("How much 'lag' do you want per background. 0 is camera speed")]
    private Vector2 parallaxEffectMutiplier;
    private float textureUnitSizeX;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite= GetComponent<SpriteRenderer>().sprite;//grabs background sprite from renderer
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;//get texture size by dividing width by pixels per unit
    }
    private void LateUpdate()
    {
        Vector3 deltamovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(deltamovement.x*parallaxEffectMutiplier.x, deltamovement.y * parallaxEffectMutiplier.y);
        lastCameraPosition = cameraTransform.position;

        if(Mathf.Abs( cameraTransform.position.x-transform.position.x)>= textureUnitSizeX)//check difference between this transform position and current camera position
        {
            //offsets the background so that it is more precice 
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            //relocate transform of background
            transform.position = new Vector3(cameraTransform.position.x+offsetPositionX, transform.position.y);
        }
    }
}
