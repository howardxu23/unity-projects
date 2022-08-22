using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Intro : MonoBehaviour
{
    // Sequence:
    // - Disable player control
    // - Poof guide next to player
    // - Guide greeting appears
    // - Camera and speech bubble fly slowly around ship while guide speaks more
    // - Guide poofs away
    // - Player regains control.

    public GameObject player;
    public GameObject playerUI;
    public GameObject mainCam;
    public GameObject guideCam;
    protected AudioManager audioManager;
    public GameObject skeleton;
    public ParticleSystem poofCloud;
    public GameObject speechBubble;
    public TextMeshPro speech;
    private Vector3 speechBubbleOffset;

    private Vector3 camStartPos;
    public Transform cameraTargets;
    public GameObject shopSkeleton;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<playerScript>().playingIntro = true;

        playerUI.SetActive(false);

        mainCam.GetComponent<Camera>().enabled = false;
        mainCam.GetComponent<AudioListener>().enabled = false;

        guideCam.transform.position = mainCam.transform.position + player.transform.position;// Move GuideCam to same starting pos as MainCam. ~ SK
        camStartPos = guideCam.transform.position;
        guideCam.GetComponent<Camera>().enabled = true;

        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        skeleton.SetActive(false);

        speechBubble.SetActive(false);

        speechBubbleOffset = speechBubble.transform.position - guideCam.transform.position;

        foreach (Transform child in cameraTargets)
        {
            child.GetComponent<MeshRenderer>().enabled = false;
        }

        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        shopSkeleton.SetActive(false);
        yield return new WaitForSeconds(2f);// Wait. ~ SK

        poofCloud.Play();
        yield return new WaitForSeconds(0.25f);// Wait for cloud to fully obscure before skeleton appears. ~ SK

        skeleton.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        speechBubble.SetActive(true);
        speech.text = "HEY!";
        audioManager.Play("SkeletonTalk");
        yield return new WaitForSeconds(2f);

        speech.text = "This ship has\nbeen boarded!";
        audioManager.Play("SkeletonTalk");
        yield return new WaitForSeconds(1f);
        MoveCamAndSpeechBubble(cameraTargets.GetChild(0).position, 4f);
        yield return new WaitForSeconds(6f);

        speech.text = "Defeat the\n Nightmare Pirates";
        audioManager.Play("SkeletonTalk");
        MoveCamAndSpeechBubble(cameraTargets.GetChild(1).position, 3f);
        yield return new WaitForSeconds(5f);

        speechBubble.SetActive(false);
        speech.text = "Choose which deck\nto fight through";
        MoveCamAndSpeechBubble(cameraTargets.GetChild(2).position, 1f);
        yield return new WaitForSeconds(1f);
        speechBubble.SetActive(true);
        audioManager.Play("SkeletonTalk");
        yield return new WaitForSeconds(1f);
        MoveCamAndSpeechBubble(cameraTargets.GetChild(3).position, 4f);
        yield return new WaitForSeconds(5f);

        speechBubble.SetActive(false);
        speech.text = "The Evil Captain\nSaw is at the helm";
        MoveCamAndSpeechBubble(cameraTargets.GetChild(4).position, 2f);
        yield return new WaitForSeconds(2f);
        speechBubble.SetActive(true);
        audioManager.Play("SkeletonTalk");
        yield return new WaitForSeconds(6f);

        speechBubble.SetActive(false);
        speech.text = "Visit my shop\nbefore fighting him.";
        MoveCamAndSpeechBubble(cameraTargets.GetChild(5).position, 1f);
        yield return new WaitForSeconds(1f);
        speechBubble.SetActive(true);
        audioManager.Play("SkeletonTalk");
        yield return new WaitForSeconds(5f);

        speechBubble.SetActive(false);
        speech.text = "Good luck!";
        MoveCamAndSpeechBubble(camStartPos, 3f);
        yield return new WaitForSeconds(3f);
        speechBubble.SetActive(true);
        audioManager.Play("SkeletonTalk");
        yield return new WaitForSeconds(3f);

        poofCloud.Play();
        speechBubble.SetActive(false);
        yield return new WaitForSeconds(0.25f);// Wait for cloud to fully obscure before skeleton disappears. ~ SK

        skeleton.SetActive(false);
        player.GetComponent<playerScript>().playingIntro = false;
        playerUI.SetActive(true);
        guideCam.GetComponent<Camera>().enabled = false;
        mainCam.GetComponent<Camera>().enabled = true;
        guideCam.GetComponent<AudioListener>().enabled = false;
        mainCam.GetComponent<AudioListener>().enabled = true;
        shopSkeleton.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            StopAllCoroutines();
            speechBubble.SetActive(false);
            speech.gameObject.SetActive(false);
            skeleton.SetActive(false);
            player.GetComponent<playerScript>().playingIntro = false;
            playerUI.SetActive(true);
            guideCam.GetComponent<Camera>().enabled = false;
            mainCam.GetComponent<Camera>().enabled = true;
            guideCam.GetComponent<AudioListener>().enabled = false;
            mainCam.GetComponent<AudioListener>().enabled = true;
            shopSkeleton.SetActive(true);
        }
    }

    void MoveCamAndSpeechBubble(Vector3 endPos, float moveTime)// This method just keeps Intro() tidier. ~ SK
    {
        StartCoroutine(MoveOverTime(guideCam, endPos, moveTime));
        StartCoroutine(MoveOverTime(speechBubble, endPos + speechBubbleOffset, moveTime));
    }

    public IEnumerator MoveOverTime(GameObject objectToMove, Vector3 endPos, float moveTime)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;

        while (elapsedTime < moveTime)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = endPos;
    }
}
