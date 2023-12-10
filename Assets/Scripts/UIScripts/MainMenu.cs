using System;
using System.Net.Http;
using Helpers;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public Button startButton;
    public Button joinButton;
    public Button backButton;
    public TMP_Dropdown sessionsDropdown;
    public TMP_Text sessionTitle;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();

        GenerateUsername();
    }

    void Start()
    {
        backButton.gameObject.SetActive(false);
        sessionsDropdown.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    private void GenerateUsername()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", Consts.ApiConsts.X_API_KEY);
        var response = client.GetAsync(Consts.ApiConsts.RandomUserApiUrl).Result;
        var responseContent = response.Content.ReadAsStringAsync().Result;
        var randomUserResponseModel = JsonUtility.FromJson<Models.RandomUserResponseModel>(responseContent);
        var username = randomUserResponseModel.username;
        PlayerPrefs.SetString("username", username);
        sessionTitle.text = $"Welcome, {username}!";
    }

    public override void OnEnable()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        joinButton.onClick.AddListener(OnJoinButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        base.OnEnable();
    }

    public override void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartButtonClick);
        joinButton.onClick.RemoveListener(OnJoinButtonClick);
        backButton.onClick.RemoveListener(OnBackButtonClick);
        base.OnDisable();
    }

    private void OnStartButtonClick()
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InLobby)
        {
            PopUp.Error("Please wait for connection to be established");
            return;
        }
        StartSession();
    }
    
    private void OnJoinButtonClick()
    {
        if (startButton.gameObject.IsActive())
        {
            startButton.gameObject.SetActive(false);
            sessionsDropdown.gameObject.SetActive(true);
            backButton.gameObject.SetActive(true);
            return;
        }

        JoinSession();
    }
    
    private void OnBackButtonClick()
    {
        startButton.gameObject.SetActive(true);
        sessionsDropdown.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
    
    private void StartSession()
    {
        string roomName = PlayerPrefs.GetString("username");

        if (roomName is null)
        {
            PopUp.Error("Please wait for username to be generated");
        }
        PhotonNetwork.CreateRoom(roomName, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });

    }
    
    private void JoinSession()
    {
        PopUp.Warning("Please wait");
        string roomName = sessionsDropdown.options[sessionsDropdown.value]?.text;
        if(roomName is null)
        {
            PopUp.Error("Please select a session");
            return;
        }
        PhotonNetwork.JoinRoom(roomName);
    }
    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PopUp.Success("Connected to lobby");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PopUp.Error(message);
    }
    
    public override void OnCreatedRoom()
    {
        PopUp.Success("Room created successfully");
        PhotonNetwork.LoadLevel("Game");
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PopUp.Error(message);
    }
    
    public override void OnRoomListUpdate(System.Collections.Generic.List<Photon.Realtime.RoomInfo> roomList)
    {
        sessionsDropdown.ClearOptions();
        foreach (var room in roomList)
        {
            sessionsDropdown.options.Add(new TMP_Dropdown.OptionData(room.Name));
        }
    }
}
