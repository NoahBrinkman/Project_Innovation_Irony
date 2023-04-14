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
    
    [Header("UI References")]
    [Space]
    [Header("Player Ready Transforms")]
    [SerializeField] private RectTransform miningTransform;
    [SerializeField] private RectTransform cleaningTransform;
    [SerializeField] private RectTransform smeltingTransform;
    [SerializeField] private RectTransform castingTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float lockedInYValue;
    [SerializeField] private float noPlayerYValue;

    [Space] 
    [Header("Results")] 
    [SerializeField] private RectTransform resultsParent;
    [SerializeField] private List<RoomWithText> resultTexts = new List<RoomWithText>();
    [SerializeField] private Ease easeMode = Ease.Linear;
    [SerializeField] private TMP_Text totalText;
    [Header("Game Set up")]
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();
    private List<Recipe> recipePool;
    private List<Recipe> openOrders = new List<Recipe>();
    [SerializeField] private GameObject orderDisplay;
    private List<GameObject> stillOpenOrderDisplays;
    [SerializeField] private float spawnY = -60;
    [SerializeField] private float spawnX = -60;
    [SerializeField] private float startY = 275;
    [SerializeField] private float spaceIncrements = 50;
    
    [Space]
    [Header("GameTime")]
    [SerializeField] private float timePerRoundInSeconds = 180.0f;
    [SerializeField] private float bufferSeconds = 3;
    private float timer;
    [SerializeField] private TMP_Text timerText;
    private bool gameEnded = false;
    
    
    
    private void Start()
    {
        recipePool = new List<Recipe>(recipes);
        stillOpenOrderDisplays = new List<GameObject>();
        GameConnecter.Instance.OnMinigameChosen += MoveUp;
        GameConnecter.Instance.OnMinigameUnChosen += moveDown;
        GameConnecter.Instance.OnGameRoomStarted += SendRandomRecipe;
        GameConnecter.Instance.OnItemFinished += CloseOrder;
        GameConnecter.Instance.OnGameEnded += ShowEndGameUI;
    }

    private void ShowEndGameUI(EndGameEvent msg)
    {
        timerText.gameObject.SetActive(false);
        miningTransform.DOMoveY(noPlayerYValue * 2, .5f).SetEase(Ease.InBounce);
        cleaningTransform.DOMoveY(noPlayerYValue * 2, .5f).SetEase(Ease.InBounce);
        smeltingTransform.DOMoveY(noPlayerYValue * 2, .5f).SetEase(Ease.InBounce);
        castingTransform.DOMoveY(noPlayerYValue * 2, .5f).SetEase(Ease.InBounce);
        int totalGrade = 0;
        for (int i = 0; i < msg.grades.Count; i++)
        {
            for (int j = 0; j < resultTexts.Count; j++)
            {
                if (resultTexts[j].room == msg.grades[i].room)
                {
                    resultTexts[j].text.text = $"${msg.grades[i].grade}";
                    totalGrade += msg.grades[i].grade;
                    break;
                }
            }
        }

        totalText.text = $"${totalGrade}";
        resultsParent.localScale = new Vector3();
        resultsParent.gameObject.SetActive(true);
        
        resultsParent.DOScale(Vector3.one, .25f).SetEase(easeMode);

    }

    private void CloseOrder(FinishItemResponse response)
    {
        Debug.Log("Received closeOrder");
        for (int i = 0; i < openOrders.Count; i++)
        {
            if (response.recipe.item == openOrders[i].recipe.item)
            {
                openOrders.Remove(openOrders[i]);
                for (int j = 0; j < stillOpenOrderDisplays.Count; j++)
                {
                    if (stillOpenOrderDisplays[j].GetComponent<OrderDisplay>().recipe == openOrders[i])
                    {
                        stillOpenOrderDisplays[j].GetComponent<OrderDisplay>().SendOffScreen();
                        
                    }
                }
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

        GameObject newOrderDisplay = Instantiate(orderDisplay, new Vector3(spawnX, spawnY,0),Quaternion.identity);
        openOrders.Add(r);
        newOrderDisplay.GetComponent<OrderDisplay>().
            Initialize(r, openOrders.Count, startY - (stillOpenOrderDisplays.Count * spaceIncrements));
        newOrderDisplay.transform.parent = canvas.transform;
        stillOpenOrderDisplays.Add(newOrderDisplay);
        GameConnecter.Instance.SendRecipe(r);
        
    }

    private void Update()
    {
        if (timerText.gameObject.activeInHierarchy)
        {
            timer -= Time.deltaTime;

            if (timer <= 0 && !gameEnded)
            {
                if (timer + bufferSeconds <= 0)
                {
                    GameConnecter.Instance.EndGame();
                    gameEnded = true;
                }
            }
            else
            {
                float minutes = Mathf.FloorToInt(timer / 60);
                float seconds = Mathf.FloorToInt(timer % 60);
                timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
            }

        }

    }
    
    
    
}

[Serializable]
public class RoomWithText
{
    public MinigameRoom room;
    public TMP_Text text;
}

