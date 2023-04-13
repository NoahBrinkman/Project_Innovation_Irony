using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using shared;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Room
{
    login,
    lobby,
    game
}

public class MobileNetworkClient : MonoBehaviour
{
    private TcpMessageChannel _channel;
    public TcpMessageChannel channel => _channel;
    private static MobileNetworkClient _instance;
    public static MobileNetworkClient Instance => _instance;
    
    [SerializeField] private int port = 55555;
    [SerializeField] private int lobbySceneIndex = 1;
    [SerializeField] private int miningSceneIndex = 2;
    [SerializeField] private int cleaningSceneIndex = 3;
    [SerializeField] private int smeltingSceneIndex = 4;
    [SerializeField] private int castingSceneIndex = 5;
    private string passCode;
    private Room currentRoom;
    public Action<List<MinigameRoom>> OnLobbyJoinedEvent;
    public Action<MinigameChosenEvent> OnMinigameChosenEvent;
    public Action<MinigameUnChosenEvent> OnMinigameUnChosenEvent;
    public Action<Recipe> OnRecipeReceived;
    public Action<List<Recipe>> OnRecipesReceived;
    public Action<SendMetalResponse> OnMetalReceived;
    public Action<SendMetalsResponse> OnMetalsReceived;
    public List<MinigameRoom> chosenRooms { get; private set; }


   [HideInInspector] public Recipe recipeBacklog = null;
    
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
            chosenRooms = new List<MinigameRoom>();
            _instance = this;
            currentRoom = Room.login;
            DontDestroyOnLoad(gameObject);
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
        if(message is PlayerJoinResponse) HandlePlayerJoinResponse(message as PlayerJoinResponse);
        else if (message is MinigameChosenEvent) HandleMinigameChosenEvent(message as MinigameChosenEvent);
        else if (message is RoomJoinedEvent) handleRoomJoinedEvent(message as RoomJoinedEvent);
        else if (message is MinigameUnChosenEvent) HandleMinigameUnChosenEvent(message as MinigameUnChosenEvent);
        else if (message is RecipeAddedMessage) handleRecipeAddedMessage(message as RecipeAddedMessage);
        else if(message is SendMetalResponse) OnMetalReceived?.Invoke(message as SendMetalResponse);
        else if(message is SendMetalsResponse) OnMetalsReceived?.Invoke(message as SendMetalsResponse);
       
    }

    private void handleRecipeAddedMessage(RecipeAddedMessage message)
    {
        Debug.Log("Recipe Received");
        if (OnRecipeReceived == null)
        {
            Debug.Log("Recipe is null so adding it to backlog");
            recipeBacklog = message.recipe;
        }
        OnRecipeReceived?.Invoke(message.recipe);
    }

    private void HandleMinigameUnChosenEvent(MinigameUnChosenEvent message)
    {
        OnMinigameUnChosenEvent?.Invoke(message);
    }

    private void handleRoomJoinedEvent(RoomJoinedEvent message)
    {
        if (message.room == RoomJoinedEvent.Room.LOBBY_ROOM)
        {
            if(message.miningGameChosen) chosenRooms.Add(MinigameRoom.Mining);
            if(message.cleaningGameChosen) chosenRooms.Add(MinigameRoom.Cleaning);
            if(message.smeltingGameChosen) chosenRooms.Add(MinigameRoom.Smelting);
            if(message.castingGameChosen) chosenRooms.Add(MinigameRoom.Casting);
        }
        else if(message.room == RoomJoinedEvent.Room.GAME_ROOM)
        {
            if(message.miningGameChosen)  SceneManager.LoadScene(miningSceneIndex);
            if(message.cleaningGameChosen)  SceneManager.LoadScene(cleaningSceneIndex);
            if(message.smeltingGameChosen)  SceneManager.LoadScene(smeltingSceneIndex);
            if(message.castingGameChosen)  SceneManager.LoadScene(castingSceneIndex);;
        }


    }

    private void HandleMinigameChosenEvent(MinigameChosenEvent message)
    {
        if (currentRoom != Room.lobby) return;
        OnMinigameChosenEvent?.Invoke(message);
    }

    private void HandlePlayerJoinResponse(PlayerJoinResponse message)
    {
        if(message.result == PlayerJoinResponse.RequestResult.DENIED) Debug.LogWarning("INCORRECT PASSCODE");
        else
        {
            Debug.Log("Connected, loading into a lobby");
            SceneManager.LoadScene(lobbySceneIndex);
            currentRoom = Room.lobby;
        }
    }

    public void EditPasscode(string pc)
    {
        if (currentRoom != Room.login) return;
        passCode = pc;
    }

    public void Connect()
    {
        Debug.Log("Trying to connect with passcode: " + passCode);

        try
        {
            _channel.Connect("192.168.137.1", port);
            PlayerJoinRequest request = new PlayerJoinRequest();
            request.passCode = passCode;
            _channel.SendMessage(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void ReadiedUp(bool isReady)
    {
        ChangeReadyStatusRequest request = new ChangeReadyStatusRequest();
        request.ready = isReady;
        _channel.SendMessage(request);
    }

    public void SendMetal(SendMetalRequest request)
    {
        _channel.SendMessage(request);
    }
    public void SendMetal(SendMetalsRequest request)
    {
        _channel.SendMessage(request);
    }
}
