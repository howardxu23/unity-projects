using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MENU : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.Play("Song");
        audioManager.gameObject.SetActive(true);
    }

    public void ButtonSound()
    {
        audioManager.Play("ButtonCork");
    }

    public void Subtitles(bool tf)
    {
        audioManager.subtitles = tf;
    }

    public void Mute(bool tf)
    {
        /*if (audioManager.gameObject.activeInHierarchy)
        {
            audioManager.gameObject.SetActive(false);
        }
        else
        {
            audioManager.gameObject.SetActive(true);
        }*/

        audioManager.Mute(tf);
    }

    public void PlayGame()
    {
        audioManager.Stop("Song");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        Debug.Log("Game has been quit!");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
