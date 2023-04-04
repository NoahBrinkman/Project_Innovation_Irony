using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CastMold : MonoBehaviour
{
    [SerializeField] private Recipe myRecipe;
    [SerializeField] private float perfectFillValue;
    [SerializeField] private float perfectFillMargin;
    private float fillValue;
    [SerializeField] private Transform castingPosition;
    [SerializeField] private Transform startingPos;
    [SerializeField] private bool isSelected;


    [SerializeField] private CastingManager castingManager;
    
    public int GetGrade()
    {
        return 5;
    }

    public void Fill(float amount)
    {
        fillValue += amount;
        Debug.Log(fillValue);
    }

    private void OnMouseDown()
    {
       castingManager.SelectCurrentMold(this);
    }

    public void MoveToStartPosition()
    {
        transform.DOMove(startingPos.position, 1);
        transform.DORotate(startingPos.rotation.eulerAngles, 1);
    }
    
    public void MoveToPouringPosition()
    {
        transform.DOMove(castingPosition.position, 1);
        transform.DORotate(castingPosition.rotation.eulerAngles, 1);
    }
}
