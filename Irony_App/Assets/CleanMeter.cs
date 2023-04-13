using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanMeter : MonoBehaviour
{
    [SerializeField] private Image indicator;

    public void UpdateIndicatorPosition(WashableOre ore)
    {
        Vector3 pos = indicator.transform.localPosition;
        pos.x = Mathf.Lerp(-100, 100, 1 / (ore.maxCleaningValue / ore.cleaningValue));

        indicator.transform.localPosition = pos;
    }
}
