using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class OrderDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text typeText;
    [HideInInspector] public Recipe recipe;
    
    public void Initialize(Recipe recipe, int orderNumber, float targetY)
    {
        titleText.text = $"Order - #{orderNumber}";
        this.recipe = recipe;
        if (recipe.isWeapon)
        {
            typeText.text = $"Type: Weapon\nName: {recipe.name}";
        }
        else
        {
            typeText.text = $"Type: Tool\nName: {recipe.name}";
        }

        transform.DOMoveY(targetY, 1).SetEase(Ease.OutCirc);
    }

    public void SendOffScreen()
    {
        Destroy(gameObject,3.2f);
        transform.DOMoveX(500, 3).SetEase(Ease.OutBack);
    }
    
}
