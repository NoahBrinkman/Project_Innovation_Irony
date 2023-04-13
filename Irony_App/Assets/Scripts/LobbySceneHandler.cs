using System;
using System.Collections;
using System.Collections.Generic;
using shared;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneHandler : MonoBehaviour
{
    [SerializeField] private List<RoomButton> roomButtons;
    private bool minigameChosen = false;
    [SerializeField] private RectTransform minigameChosenButtons;
    private void Awake()
    {
        
    }

    private void Start()
    {

        CheckChosenRooms(MobileNetworkClient.Instance.chosenRooms);
        
        for (int i = 0; i < roomButtons.Count; i++)
        {
            roomButtons[i].button.onClick.AddListener(roomButtons[i].OnPressed);
        }

        MobileNetworkClient.Instance.OnMinigameChosenEvent += OnMinigameChosen;
        MobileNetworkClient.Instance.OnMinigameUnChosenEvent += OnMinigameUnChosen;
    }
    

    private void CheckChosenRooms(List<MinigameRoom> chosenRooms)
    {
        Debug.Log("Checking");
        for (int i = 0; i < roomButtons.Count; i++)
        {
            roomButtons[i].button.interactable = !chosenRooms.Contains(roomButtons[i].room);
        }
    }
    
    private void OnMinigameChosen(MinigameChosenEvent message)
    {
        Debug.Log("MinigameChosenEvent Received " + message.room);
        for (int i = 0; i < roomButtons.Count; i++)
        {
            if (roomButtons[i].room == message.room)
            {
                roomButtons[i].button.interactable = false;
            }
        }

        minigameChosen = true;
        //minigameChosenButtons.gameObject.SetActive(true);
    }

    private void OnMinigameUnChosen(MinigameUnChosenEvent message)
    {
        Debug.Log("Unchosen found");
        for (int i = 0; i < roomButtons.Count; i++)
        {
            if (message.room == roomButtons[i].room)
            {
                
                roomButtons[i].button.interactable = true;
            }
        }
    }

    public void OnSendReadyButton()
    {
        MobileNetworkClient.Instance.ReadiedUp(true);
    }
    
}

[Serializable]
public class RoomButton
{
    public Button button;
    public MinigameRoom room;

    public void OnPressed()
    {
        MinigameChosenRequest request = new MinigameChosenRequest();
        request.chosenRoom = room;
        MobileNetworkClient.Instance.channel.SendMessage(request);
    }
}

