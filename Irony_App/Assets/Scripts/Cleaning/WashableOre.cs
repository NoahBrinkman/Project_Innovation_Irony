using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using shared;
using UnityEngine;

public class WashableOre : MonoBehaviour
{
    [SerializeField] private OreHelper helper;
    [SerializeField] private float cleaningSpeed = .3f;
    public Metal metalType;
    private float roughness = 0;
    public  float cleaningValue { get; private set; }
    private float targetCleaningValue = 1;
    private float targetCleaningMargin = .3f;
    public Action OnSend;
    public bool cleanEnoughToSend => cleaningValue >= (targetCleaningValue - targetCleaningMargin);

    private bool perfectGrade => cleaningValue >= (targetCleaningValue - targetCleaningMargin) &&
                                 cleaningValue <= (targetCleaningValue + targetCleaningMargin);

    public CleaningParticleSystem cps;

    public void Initialize(Metal metal, Vector3 endPosition, Ease easeMode = Ease.InQuart)
    {
        cps = FindFirstObjectByType<CleaningParticleSystem>();
        metalType = metal;
        MetalData metalInfo = helper.GetMetalData(metal);
        GetComponent<MeshRenderer>().material = metalInfo.mat;
        roughness = metalInfo.roughness;
        targetCleaningValue = metalInfo.perfectCleaningValue;
        targetCleaningMargin = metalInfo.cleaningGradeMargin;
        transform.DOMove(endPosition, .5f).SetEase(easeMode);
        ReadAccelerometerInput.Instance.OnShake += OnShaken;
        ReadSwipeInput.Instance.OnSwipeUp += OnSwipeUp;
        
    }

    private void OnDestroy()
    {
        ReadAccelerometerInput.Instance.OnShake -= OnShaken;

        ReadSwipeInput.Instance.OnSwipeUp -= OnSwipeUp;
    }

    private void OnShaken()
    {
        cleaningValue += cleaningSpeed - roughness;
        cps.SoundValue = cleaningValue;
        cps.SoundMarg = targetCleaningMargin;
        cps.SoundMax = targetCleaningValue;
      
    }
    private void OnSwipeUp()
    {
        if (cleanEnoughToSend)
        {
            transform.DOMoveY(4, .6f).SetEase(Ease.InBack);
            if (perfectGrade)
            {
                Debug.Log("very nice");
            }
            else
            {
                Debug.Log("Less nice");
            }

            StartCoroutine(waitUntilInvoke());
        }
    }

    IEnumerator waitUntilInvoke()
    {
        yield return new WaitForSeconds(1f);
        OnSend?.Invoke();
    }
}
