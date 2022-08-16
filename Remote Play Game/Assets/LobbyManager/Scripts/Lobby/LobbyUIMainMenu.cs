using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bolt.Samples.Photon.Lobby
{
    public class LobbyUIMainMenu : MonoBehaviour, ILobbyUI
    {
        public event Action OnCreateButtonClick;
        public event Action OnBrowseServerClick;
        public event Action OnJoinRandomClick;

        public bool quetionGameToggleState = false;
        public bool drawingGameToggleState = false;
        public bool spaceGameToggleState = false;

        public string MatchName
        {
            get { return matchNameInput.text; }
        }
        
        [Header("Server UI")]
        [SerializeField] private InputField matchNameInput;
        [SerializeField] private Button createRoomButton;

        [Header("toggle UI")]
        [SerializeField] private Toggle quetionGameToggle;
        [SerializeField] private Toggle drawingGameToggle;
        [SerializeField] private Toggle spaceGameToggle;
        [Header("Client UI")]
        [SerializeField] private Button browseServersButton;
        [SerializeField] private Button joinRandomButton;

        
        public void OnEnable()
        {
  
            createRoomButton.onClick.RemoveAllListeners();
            createRoomButton.onClick.AddListener(() =>
            {
                if (OnCreateButtonClick != null) OnCreateButtonClick();
            });
            
            browseServersButton.onClick.RemoveAllListeners();
            browseServersButton.onClick.AddListener(() =>
            {
                if (OnBrowseServerClick != null) OnBrowseServerClick();
            });
            
            joinRandomButton.onClick.RemoveAllListeners();
            joinRandomButton.onClick.AddListener(() =>
            {
                if (OnJoinRandomClick != null) OnJoinRandomClick();
            });

            matchNameInput.text = Guid.NewGuid().ToString();


            quetionGameToggleState = quetionGameToggle.isOn;
           // print("quetion game toggle: "+quetionGameToggleState);



            drawingGameToggleState = drawingGameToggle.isOn;
           // print("draw game toggle: "+drawingGameToggleState);



            spaceGameToggleState = spaceGameToggle.isOn;
           // print("space game toggle" + spaceGameToggleState);
        }
        public void FixedUpdate()
        {

            quetionGameToggleState = quetionGameToggle.isOn;
           // print("quetion game toggle: " + quetionGameToggleState);



            drawingGameToggleState = drawingGameToggle.isOn;
            //print("draw game toggle: " + drawingGameToggleState);

            spaceGameToggleState = spaceGameToggle.isOn;
            //print("space game toggle" + spaceGameToggleState);
        }
        public void ToggleVisibility(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
