using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using shared;
using UnityEngine;

public class CastMold : MonoBehaviour
{
    [SerializeField] public Item myItem;
    [SerializeField] private float perfectFillValue;
    public float targetFillValue => perfectFillValue;
    [SerializeField] private float perfectFillMargin;
    public float fillMargin => perfectFillMargin;
    public float fillValue { get; private set; }
    [SerializeField] private Transform castingPosition;
    [SerializeField] private Transform startingPos;
    [SerializeField] private bool isSelected;


    [SerializeField] private CastingManager castingManager;
    [SerializeField] private MoltenLevel moltenMetal;
    [SerializeField] private float minimumY;
    [SerializeField] private float perfectY;
    [SerializeField] private float maximumY;
    [SerializeField] private GameObject toolPrefab;
    public int GetGrade()
    {
        float timeUnderMargin = fillValue - (targetFillValue - fillMargin);
        float timeOverMargin = fillValue - (targetFillValue + fillMargin);
        float grade;
        if (timeOverMargin <= 0 && timeUnderMargin >= 0)
        {
            grade = 10;
        }
        else
        {
            grade = 10;
            if (timeOverMargin > 0)
            {
                grade -= timeOverMargin;
            }

            if (timeUnderMargin < 0)
            {
                grade += timeUnderMargin;
            }

        }

        return Mathf.RoundToInt(grade);
    }

    public void Fill(float amount)
    {
        fillValue += amount;
        Debug.Log(fillValue);
        moltenMetal.UpdateSubstance(1/(targetFillValue+(fillMargin*3)/fillValue));
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

    public void sendMoldOff()
    {
        
    }

    public IEnumerator SendOffScreen()
    {
        int grade = GetGrade();
        //Sequence this
        GameObject g = Instantiate(toolPrefab, transform.position,transform.rotation);
        g.transform.DOMoveY(10, 2).SetEase(Ease.InBack);
        
        transform.DOMoveX(30, 1);
        
        yield return new WaitForSeconds(1.2f);
        Fill(-fillValue);
        Fill(-fillValue);
        castingManager.SendToolToServer(myItem, grade);
        castingManager.SelectCurrentMold(this);
        
        yield break;
    }
}
