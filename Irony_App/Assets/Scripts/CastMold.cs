using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CastMold : MonoBehaviour
{
    [SerializeField] public Recipe myRecipe;
    [SerializeField] private float perfectFillValue;
    public float targetFillValue => perfectFillValue;
    [SerializeField] private float perfectFillMargin;
    public float fillMargin => perfectFillMargin;
    public float fillValue { get; private set; }
    [SerializeField] private Transform castingPosition;
    [SerializeField] private Transform startingPos;
    [SerializeField] private bool isSelected;


    [SerializeField] private CastingManager castingManager;
    [SerializeField] private Transform moltenMetal;
    [SerializeField] private float minimumY;
    [SerializeField] private float perfectY;
    [SerializeField] private float maximumY;
    public int GetGrade()
    {
        return 5;
    }

    public void Fill(float amount)
    {
        fillValue += amount;
        Debug.Log(fillValue);
        Vector3 pos = moltenMetal.transform.localPosition;
        pos.y =  Mathf.Clamp(Mathf.LerpUnclamped(minimumY,perfectY, (1/perfectFillValue)*fillValue), minimumY, maximumY);
       moltenMetal.transform.localPosition = pos;
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
        transform.DOMoveX(30, 1);
        
        yield return new WaitForSeconds(1.2f);
        Fill(-fillValue);
        castingManager.SendToolToServer(myRecipe.item, grade);
        castingManager.SelectCurrentMold(this);
        
        yield break;
    }
}
