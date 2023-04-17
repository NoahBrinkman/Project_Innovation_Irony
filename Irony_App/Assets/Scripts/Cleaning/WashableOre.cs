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
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private MeshRenderer oreMesh;
    public Metal metalType;
    private float roughness = 0;
    public  float cleaningValue { get; private set; }
    private float targetCleaningValue = 1;
    private float targetCleaningMargin = .3f;
    public Action OnSend;
    public bool cleanEnoughToSend => cleaningValue >= (targetCleaningValue - targetCleaningMargin);

    private bool perfectGrade => cleaningValue >= (targetCleaningValue - targetCleaningMargin) &&
                                 cleaningValue <= (targetCleaningValue + targetCleaningMargin);

    public float maxCleaningValue => targetCleaningValue + (targetCleaningMargin * 3);
    [HideInInspector]public CleaningParticleSystem cps;
    
    public void Initialize(Metal metal, Vector3 endPosition, Ease easeMode = Ease.InQuart)
    {
        cps = FindFirstObjectByType<CleaningParticleSystem>();
        metalType = metal;
        MetalData metalInfo = helper.GetMetalData(metal);
        oreMesh.material = metalInfo.mat;
        roughness = metalInfo.roughness;
        targetCleaningValue = metalInfo.perfectCleaningValue;
        targetCleaningMargin = metalInfo.cleaningGradeMargin;
        transform.DOMove(endPosition, .5f).SetEase(easeMode);
        particleSystem.GetComponent<Renderer>().material = metalInfo.mat;
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
        cleaningValue = Mathf.Clamp(cleaningValue, 0, maxCleaningValue);
        
        if(targetCleaningValue <= cleaningValue)
        {
            particleSystem.maxParticles = 0;
        }
        else
        {
            particleSystem.maxParticles = Mathf.RoundToInt(targetCleaningValue / cleaningValue);
        }
    }

    private void OnSwipeUp()
    {
        if (cleanEnoughToSend)
        {
            transform.DOMoveY(10, .6f).SetEase(Ease.InBack);
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

    public int GetGrade()
    {
        float timeUnderMargin = cleaningValue - (targetCleaningValue - targetCleaningMargin);
            float timeOverMargin = cleaningValue - (targetCleaningValue + targetCleaningMargin);
            
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
}
