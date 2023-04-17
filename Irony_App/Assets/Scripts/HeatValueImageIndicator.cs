using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatValueImageIndicator : MonoBehaviour
{
    [SerializeField] private Image fillableImage;

    public void SetIndicator(float f)
    {
        fillableImage.fillAmount = f;
    }
}
