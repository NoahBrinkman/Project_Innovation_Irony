using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using shared;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerLobbyHandler : MonoBehaviour
{

    [SerializeField] private RectTransform miningTransform;
    [SerializeField] private RectTransform cleaningTransform;
    [SerializeField] private RectTransform smeltingTransform;
    [SerializeField] private RectTransform castingTransform;
    [SerializeField] private float lockedInYValue;
    [SerializeField] private float noPlayerYValue;

    [SerializeField] private float timePerRoundInSeconds = 180.0f;
    [SerializeField] private float bufferSeconds = 3;
    private float timer;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private List<Recipe> recipes = new List<Recipe>();
    private List<Recipe> recipePool;
    private List<Recipe> openOrders = new List<Recipe>();
    private void Start()
    {
        recipePool = new List<Recipe>(recipes);
       GameConnecter.Instance.OnMinigameChosen += MoveUp;
       GameConnecter.Instance.OnMinigameUnChosen += moveDown;
       GameConnecter.Instance.OnGameRoomStarted += SendRandomRecipe;
       GameConnecter.Instance.OnItemFinished += CloseOrder;
    }

    private void CloseOrder(FinishItemResponse response)
    {
       Debug.Log("Received closeOrder");
        for(int i = 0; i < openOrders.Count; i++)
        {
             if(response.recipe.item == openOrders[i].recipe.item)
             {
                 openOrders.Remove(openOrders[i]);
                 Debug.Log("JOB DONE YAY");
             }
        }
        
    }


    private void MoveUp(MinigameRoom room)
    {
        switch (room)
        {
            case MinigameRoom.Mining:
                miningTransform.DOMoveY(miningTransform.position.y + lockedInYValue, 1).SetEase(Ease.OutBounce);
                break;
            case MinigameRoom.Cleaning:
                cleaningTransform.DOMoveY(cleaningTransform.position.y + lockedInYValue, 1).SetEase(Ease.OutBounce);
                break;
            case MinigameRoom.Smelting:
                smeltingTransform.DOMoveY(smeltingTransform.position.y + lockedInYValue, 1).SetEase(Ease.OutBounce);
                break;
            case MinigameRoom.Casting:
                castingTransform.DOMoveY(castingTransform.position.y + lockedInYValue, 1).SetEase(Ease.OutBounce);
                break;
                
        }
       
    }

    private void moveDown(MinigameRoom room)
    {
        Debug.Log("Unchosen found");
        switch (room)
        {
            case MinigameRoom.Mining:
                miningTransform.DOMoveY(miningTransform.position.y - lockedInYValue, 1).SetEase(Ease.InQuad);
                break;
            case MinigameRoom.Cleaning:
                cleaningTransform.DOMoveY(cleaningTransform.position.y - lockedInYValue, 1).SetEase(Ease.InQuad);
                break;
            case MinigameRoom.Smelting:
                smeltingTransform.DOMoveY(smeltingTransform.position.y - lockedInYValue, 1).SetEase(Ease.InQuad);
                break;
            case MinigameRoom.Casting:
                castingTransform.DOMoveY(castingTransform.position.y - lockedInYValue, 1).SetEase(Ease.InQuad);
                break;
                
        }
    }

    public void StartGame()
    {
        GameConnecter.Instance.StartGame(recipes[0]);
        timerText.gameObject.SetActive(true);
        timer = timePerRoundInSeconds;
    }

    private void SendRandomRecipe()
    {
        Recipe r = recipePool[Random.Range(0, recipePool.Count)];
        recipePool.Remove(r);
        if (recipePool.Count == 0)
        {
            recipePool = new List<Recipe>(recipes);
        }
        openOrders.Add(r);
        GameConnecter.Instance.SendRecipe(r);
    }
    private void Update()
    {
        if (timerText.gameObject.activeInHierarchy)
        {
            timer -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00} : {1:00}",minutes,seconds);
            if (timer + bufferSeconds <= 0)
            {
                
            }
        }
    }

}



