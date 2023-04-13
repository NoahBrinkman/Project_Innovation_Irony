using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AlwaysPopUP : MonoBehaviour
{
    [SerializeField] private Ease easeMode = Ease.InCirc;
    [SerializeField] private float duration = .25f;
    private void OnEnable()
    {
        transform.localScale = new Vector3();
        transform.DOScale(Vector3.one, duration).SetEase(easeMode);
    }
}
