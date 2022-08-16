using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(InputField))]
public class playerNameInput : MonoBehaviour
{
    #region Private Constants


    // Store the PlayerPref Key to avoid typos
    const string playerNamePrefKey = "PlayerName";


    #endregion


    #region MonoBehaviour CallBacks

    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    void Start()
    {
        string defaultName = string.Empty;
        InputField _inputField = this.GetComponent<InputField>();//finds the input field
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {//PlayerPrefs is a simple lookup list of paired entries (like an excel sheet with two columns), one is the key, one is the Value.
             //The Key is a string, and is totally arbitrary, you decide how to name and you will need to stick to it throughout the development. 
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }


        PhotonNetwork.NickName = defaultName;
    }
    #endregion


    #region Public Methods
    public void SetPlayerName(string value)
    {
        // #Important
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;


        PlayerPrefs.SetString(playerNamePrefKey, value);
    }


    #endregion
}
