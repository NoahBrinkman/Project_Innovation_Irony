using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using shared;
using UnityEngine.SceneManagement;


public class GameConnecter : MonoBehaviour
{
    private TcpMessageChannel _channel;
    
    [SerializeField] private int port = 55555;

    private static GameConnecter _instance;
    public static  GameConnecter Instance => _instance;
    private string passCode;
    public string PassCode => passCode;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _channel = new TcpMessageChannel();
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        if (_channel.Connected) receiveAndProcessNewMessages();
    }

    private void receiveAndProcessNewMessages()
    {
        if(!_channel.Connected) Debug.LogWarning("You are no longer connected please seek help");
        while (_channel.HasMessage() && gameObject.activeSelf)
        {
            ASerializable message = _channel.ReceiveMessage();
            HandleMessage(message);
        }
        
    }

    
    private void HandleMessage(ASerializable message)
    {
        if(message is HostJoinResponse) HandleHostJoinResponse(message as HostJoinResponse);
        else if (message is MinigameChosenRequest) HandleMinigameChosenRequest(message as MinigameChosenRequest);
    }


    public void Connect()
    {

        try
        {
            _channel.Connect("192.168.137.1", port);
            HostJoinRequest hostJoinRequest = new HostJoinRequest();
            _channel.SendMessage(hostJoinRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void HandleHostJoinResponse(HostJoinResponse response)
    {
        Debug.Log($"Received join Response passcode: {response.passcode}");
        passCode = response.passcode;
        SceneManager.LoadSceneAsync(1);
    }
    
    private void HandleMinigameChosenRequest(MinigameChosenRequest message)
    {
        throw new NotImplementedException();
    }
    
}
