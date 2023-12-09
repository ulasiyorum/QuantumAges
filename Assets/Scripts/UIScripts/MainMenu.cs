using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button joinButton;
    public Button backButton;
    public TMP_Dropdown sessionsDropdown;
    public TMP_Text sessionTitle;

    private void Awake()
    {
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
    
    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        joinButton.onClick.AddListener(OnJoinButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
    }
    
    private void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartButtonClick);
        joinButton.onClick.RemoveListener(OnJoinButtonClick);
        backButton.onClick.RemoveListener(OnBackButtonClick);
    }

    private void OnStartButtonClick()
    {
        throw new NotImplementedException("Requires Multiplayer Implementation");
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

        throw new NotImplementedException("Requires Multiplayer Implementation");

    }
    
    private void OnBackButtonClick()
    {
        startButton.gameObject.SetActive(true);
        sessionsDropdown.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
    
    private void StartSession()
    {
        throw new NotImplementedException("Requires Multiplayer Implementation");
    }
}
