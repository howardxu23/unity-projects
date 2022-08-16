using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameversion = "1";
    public GameObject startGameButton;
    public InputField nameInputField;
    public GameObject quitButton;
    public InputField roomNameInput;

    private string RoomName;
    [Tooltip("individual room info and join button panel")]
    public GameObject roomInfoItem;
    [Tooltip("room browser panel")]
    [SerializeField]
    private GameObject RoomPanel;
    [Tooltip("the main panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject mainPanel;
    [Tooltip("the UI laber to inform that connetion is in progress")]
    [SerializeField]
    private GameObject progressLabel;
    [Tooltip("max amount of players")]
    [SerializeField]
    private int maxPlayers=4;
    [SerializeField]
    private GameObject[] playerListPositions;
    private Vector3[] playerlistpos;
    public GameObject playerListItemPrefab;

    private GameObject[] playerlistprefabs;
    private List<RoomInfo> RoomList;

    private bool Createroompressed = false;
    private bool randomjoinpressed = false;
    private bool roomlistpressed = false;
    private bool refeshPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        RoomList = new List<RoomInfo>();
        close_all_tabs();
        mainPanel.SetActive(true);
        startGameButton.SetActive(false);
        quitButton.SetActive(false);

        playerlistpos = new Vector3[playerListPositions.Length];
        for(int i=0; i< playerListPositions.Length; i++)
        {
            var listPos = playerListPositions[i];
            playerlistpos[i] = listPos.transform.position;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //sorts the lists 
        playerlistprefabs = GameObject.FindGameObjectsWithTag("player list entity");
        //print(playerlistprefabs.Length);
        for (int i = 0; i < playerlistprefabs.Length; i++)
        {
            GameObject item = playerlistprefabs[i];
            item.transform.position = playerlistpos[i];
        }
        
        /*
        if (roomlistpressed == true&&RoomList!=null)
        {
            clearRoomList();
            Transform listcontent = RoomPanel.transform.Find("Scroll View/Viewport/Content");
            Debug.Log("Room count: " + RoomList.Count);
            foreach (RoomInfo oneroomdata in RoomList)
            {
                if (oneroomdata.IsOpen == true && oneroomdata.IsVisible == true && oneroomdata.RemovedFromList == false)
                {
                    Debug.LogWarning("creating new room panel");
                    GameObject newRoomPanel = Instantiate(roomInfoItem, listcontent) as GameObject;//creates new room data panel

                    newRoomPanel.transform.Find("room name").GetComponent<Text>().text = oneroomdata.Name;//sets the name
                    newRoomPanel.transform.Find("players in room").GetComponent<Text>().text = oneroomdata.PlayerCount + "/" + oneroomdata.MaxPlayers;//sets no. of players in room currently
                    newRoomPanel.transform.Find("join room Button").GetComponent<Button>().onClick.AddListener(delegate { joinRoom(newRoomPanel.transform); });//allows the button to be used to connect
                }
            }
            roomlistUpdate = false;
        }*/
    }
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;        
    }
    public void RandomConnect()
    {
        progressLabel.SetActive(true);//puts on connecting in progress
        mainPanel.SetActive(false);//disables the main panel

        if (PhotonNetwork.IsConnected)//once connected
        {
            PhotonNetwork.JoinRandomRoom();//join random room
        }
        else
        {
            //if not connected, make a connection attempt
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameversion;
        }
        randomjoinpressed = true;
    }
    public override void OnConnectedToMaster()
    {
        print("onConnectedtoMaster was called y pun");
        //PhotonNetwork.JoinRandomRoom();

        if (Createroompressed == true)//creates a new room if create room is presssed
        {
            Debug.Log("create room");
            close_all_tabs();
            create();           
        }
        else if (roomlistpressed == true)
        {
            close_all_tabs();
            OpenRooms();
        }
        else if (randomjoinpressed == true)
        {
            close_all_tabs();
            RandomConnect();
        }
        base.OnConnectedToMaster();
    }
    public override void OnJoinedRoom()//called once player enters room
    {
        close_all_tabs();
        progressLabel.SetActive(false);//disables the progress panel once room is founnd
        quitButton.SetActive(true);
        
        Debug.Log("onJoineroom was called, client has joined a room!");

        createPlayer((int)PhotonNetwork.CurrentRoom.PlayerCount - 1);

        if (PhotonNetwork.IsMasterClient)//checks to see if client is master, which then it will be responsible for core elements of the gameplay that is replicated across all connected clients
        {
            startGameButton.SetActive(true);
        }
    }
    
    public void OpenRooms()//opens the room list
    {
        mainPanel.SetActive(false);
        progressLabel.SetActive(true);
        if (PhotonNetwork.IsConnected)//once connected open room panel
        {
            close_all_tabs();
            RoomPanel.SetActive(true);
            quitButton.SetActive(true);
            PhotonNetwork.JoinLobby();
        }
        else
        {
            //if not connected, make a connection attempt
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameversion;
        }
        roomlistpressed = true;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("onjoinrandomfailed was called. no open rooms avliable. Disconnecting");
        PhotonNetwork.Disconnect();
    }
    public void create()//creates new room and joins it
    {
        mainPanel.SetActive(false);
        progressLabel.SetActive(true);
        if (PhotonNetwork.IsConnected)//once connected open room panel
        {
            //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)maxPlayers });
            createWithName();
        }
        else
        {
            //if not connected, make a connection attempt
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameversion;
        }
        Createroompressed = true;
    }
    private void createWithName()//creates new room with name and joins it
    {
        string roomname = roomNameInput.text;
        PhotonNetwork.CreateRoom(roomname, new RoomOptions { MaxPlayers = (byte)maxPlayers });
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected was called by PUN with reason {0}", cause);
        PhotonNetwork.LoadLevel("Launcher");
    }
    private void close_all_tabs()//closes all tabs
    {
        mainPanel.SetActive(false);
        RoomPanel.SetActive(false);
        progressLabel.SetActive(false);
    }
    public void startGame()//starts the match, taking from the lobby waiting room to game
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Main level");
            PhotonNetwork.CurrentRoom.IsOpen = false;//locks the room so no one else can drop in.
        }
        
    }
    public void SetPlayerName(string value)
    {
        // #Important
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;


        //PlayerPrefs.SetString(playerNamePrefKey, value);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)//player joins
    {
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)//player leaves
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        
     
    }
    private void createPlayer(int playerPos)//player pos is the position of the list that it spawns
    {
        if (playerListDisplayItem.localPlayerInstance == null)//check if this client has a player setup
        {
            //if not, we need to make them one, spawn a char for the local player
            print("create listing item");
            PhotonNetwork.Instantiate(this.playerListItemPrefab.name,playerlistpos[playerPos] , Quaternion.identity);

        }
    }


    public override void OnRoomListUpdate(List<RoomInfo> p_list)//when the server sends a update to the roomlist
    {
        base.OnRoomListUpdate(p_list);

        bool matchfound = false;
        //if (refeshPressed == true)
        //{
        foreach (var oneroomdata in p_list)
        {
            if (oneroomdata.RemovedFromList == true || oneroomdata.IsOpen == false)
            {
                RoomList.Remove(oneroomdata);
                continue;
            }
            for (int i = 0; i < RoomList.Count; i++)
            {
                if (RoomList[i].Name == oneroomdata.Name)
                {
                    RoomList[i] = oneroomdata;
                    matchfound = true;
                    continue;
                }
            }
            if (matchfound == false)
            {
                RoomList.Add(oneroomdata);
            }
            matchfound = false;
        }
        Debug.Log("P_list count: " + p_list.Count);
        //RoomList = new List<RoomInfo>();

        clearRoomList();//clears the old list in display

        Transform listcontent = RoomPanel.transform.Find("Scroll View/Viewport/Content");
        Debug.Log("roomList count: " + RoomList.Count);

        foreach (var room in RoomList)
        {
            Debug.LogWarning("creating new room panel");
            GameObject newRoomPanel = Instantiate(roomInfoItem, listcontent) as GameObject;//creates new room data panel

            newRoomPanel.transform.Find("room name").GetComponent<Text>().text = room.Name;//sets the name
            newRoomPanel.transform.Find("players in room").GetComponent<Text>().text = room.PlayerCount + "/" + room.MaxPlayers;//sets no. of players in room currently
            newRoomPanel.transform.Find("join room Button").GetComponent<Button>().onClick.AddListener(delegate { joinRoom(newRoomPanel.transform); });//allows the button to be used to connect
        }
        refeshPressed = false;
        
    }
    public void refreshLobby()//refreshes the lobby
    {
        PhotonNetwork.LeaveLobby();
        refeshPressed = true;     
    }
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        
    }
    /*
    public override void OnRoomListUpdate(List<RoomInfo> p_list)//when the server sends a updated roomlist
    {
        base.OnRoomListUpdate(p_list);

        Transform listcontent = RoomPanel.transform.Find("Scroll View/Viewport/Content");
        Debug.Log("P_list count: " + p_list.Count);
        //RoomList = new List<RoomInfo>();
        RoomList = p_list;
        Debug.Log("roomList count: " + RoomList.Count);
    }
    
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Transform listcontent = RoomPanel.transform.Find("Scroll View/Viewport/Content");
        clearRoomList();
        foreach (var room in RoomList)
        {
            Debug.LogWarning("creating new room panel");
            GameObject newRoomPanel = Instantiate(roomInfoItem, listcontent) as GameObject;//creates new room data panel

            newRoomPanel.transform.Find("room name").GetComponent<Text>().text = room.Name;//sets the name
            newRoomPanel.transform.Find("players in room").GetComponent<Text>().text = room.PlayerCount + "/" + room.MaxPlayers;//sets no. of players in room currently
            newRoomPanel.transform.Find("join room Button").GetComponent<Button>().onClick.AddListener(delegate { joinRoom(newRoomPanel.transform); });//allows the button to be used to connect
        }
    }*/
    public void joinRoom(Transform panel)
    {      
        string roomName = panel.transform.Find("room name").GetComponent<Text>().text;//grabs the room name
        Debug.LogWarningFormat("joining room {0}", roomName);
        PhotonNetwork.JoinRoom(roomName);//intiate connection with room
    }
    private void clearRoomList()//clears the server browser list
    {
        Transform content = RoomPanel.transform.Find("Scroll View/Viewport/Content");
        Debug.LogWarning("clearing room list");
        foreach (Transform item in content)
        {
            Destroy(item.gameObject);         
        }
    }
}
