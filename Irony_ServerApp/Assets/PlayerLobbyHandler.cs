using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using shared;
using UnityEngine;

public class PlayerLobbyHandler : MonoBehaviour
{

    [SerializeField] private RectTransform miningTransform;
    [SerializeField] private RectTransform cleaningTransform;
    [SerializeField] private RectTransform smeltingTransform;
    [SerializeField] private RectTransform castingTransform;
    [SerializeField] private float lockedInYValue;
    [SerializeField] private float noPlayerYValue;


    private void Start()
    {
        GameConnecter.Instance.OnMinigameChosen += MoveUp;
        GameConnecter.Instance.OnMinigameUnChosen += moveDown;
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
        GameConnecter.Instance.StartGame();
    }
    
}



