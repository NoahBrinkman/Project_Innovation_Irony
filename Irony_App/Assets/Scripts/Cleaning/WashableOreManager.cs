using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using shared;
using TMPro;
using UnityEngine;

public class WashableOreManager : MonoBehaviour
{
    [SerializeField] private GameObject washableOrePrefab;
    [SerializeField] private List<Metal> washableOreBacklog;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform endPosition;
    private WashableOre currentOre;
    [SerializeField] private Ease easeMode;

    [SerializeField] private TMP_Text cleanLevelText;

    private void Start()
    {
        MobileNetworkClient.Instance.OnMetalsReceived += OnMetalsReceived;
    }

    private void OnMetalsReceived(SendMetalsResponse msg)
    {
        if (msg.to == MinigameRoom.Cleaning)
        {
            if (currentOre == null)
            {
                CreateNewOre(msg.metals[0], false);
                for (int i = 1; i < msg.metals.Count; i++)
                {
                    washableOreBacklog.Add(msg.metals[i]);
                }
            }
            else
            {
                for (int i = 0; i < msg.metals.Count; i++)
                {
                    washableOreBacklog.Add(msg.metals[i]);
                }
            }
        }
    }

    private void Update()
    {
        if (washableOreBacklog.Count > 0 && currentOre == null)
        {
            //Get oldest in list
            CreateNewOre(washableOreBacklog[0]);
            //Send message to server
        }

        if (currentOre != null)
            cleanLevelText.text =
                $"Clean Level: {(currentOre.cleanEnoughToSend ? "<color=green>" : "")}{currentOre.cleaningValue.ToString("F2")}{(currentOre.cleanEnoughToSend ? "</color>" : "")}";
        else cleanLevelText.text = "All done for now!";
    }

    private void OnOreSent()
    {
        SendMetalRequest request = new SendMetalRequest();
        request.from = MinigameRoom.Cleaning;
        request.to = MinigameRoom.Smelting;
        request.grade = 5;
        request.metal = currentOre.metalType;
        MobileNetworkClient.Instance.SendMetal(request);
        Destroy(currentOre.gameObject,3);
        currentOre.OnSend -= OnOreSent;
        currentOre = null;
        if (washableOreBacklog.Count > 0)
        {
            CreateNewOre(washableOreBacklog[0]);
        }
    }


    private void CreateNewOre(Metal metal, bool removeFromBacklog = true)
    {
        //Spawn new ore 
        GameObject newOre = Instantiate(washableOrePrefab, spawnPosition.position, Quaternion.identity);
        WashableOre wA = newOre.GetComponent<WashableOre>();
        //Initialize new ore with correct metal
        wA.Initialize(metal, endPosition.position, easeMode);
        //Remove from list
       if(removeFromBacklog) washableOreBacklog.RemoveAt(0);
        currentOre = wA;
        
        wA.OnSend += OnOreSent;
    }
    
}
