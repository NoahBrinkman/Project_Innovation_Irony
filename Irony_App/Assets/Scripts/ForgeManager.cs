using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using shared;
using TMPro;
using UnityEngine;

public class ForgeManager : MonoBehaviour
{ 
    
    [SerializeField] private OreHelper helper;
   [SerializeField] private List<Metal> forgeBacklog;
   [SerializeField] private Metal currentlyInForge;

   [SerializeField] private float gradeSubtractionMultiplier = 2.0f;
   private ReadMicInput micInput;
   private float currentTargetHeat;
   private float currentHeatMargin;
   private float currentCookTime;
   private float timer;
   private float micVolume = 0;
   [SerializeField] private float dropOffRate = 1f;
   [SerializeField] private Transform moltenMetal;

   [SerializeField] private HeatValueImageIndicator indicator;
   [SerializeField] [NotNull] private TMP_Text debugText;
    // Start is called before the first frame update
    void Start()
    {
        micInput = GetComponent<ReadMicInput>();
        ReadSwipeInput.Instance.OnSwipeRight += OnSwipeRight;
       if(MobileNetworkClient.Instance != null) MobileNetworkClient.Instance.OnMetalReceived += OnMetalReceived;
    }

    private void OnMetalReceived(SendMetalResponse obj)
    {
        if (obj.to == MinigameRoom.Smelting)
        {
            if (currentlyInForge == Metal.None)
            {
                currentlyInForge = obj.metal;
            }
            else
            {
                forgeBacklog.Add(obj.metal);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (micVolume >= currentTargetHeat - currentHeatMargin &&
            micVolume <= currentTargetHeat + currentHeatMargin)
        {
            timer += Time.deltaTime;
        }

        if (micInput.volume > 0.1f)
        {
            micVolume += micInput.volume;
        }
        else
        {
            micVolume -= dropOffRate;
        }
        indicator.SetIndicator(micVolume/10);
        debugText.text = $"Heat value: {micInput.volume}\nTimeTaken: {timer.ToString("F2")}";
        
        if (currentlyInForge == Metal.None && forgeBacklog.Count > 0)
        {
            InitializeNewMetalInForge();
        }   
    }

    private void OnSwipeRight()
    {
        // timer = 5 
        // cooktime = 5
        // margin is 2
        if (timer >= currentCookTime - (currentHeatMargin*2))
        {
            float timeUnderMargin = timer - (currentCookTime - currentHeatMargin);
            float timeOverMargin = timer - (currentCookTime + currentHeatMargin);
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
            SendMetalRequest request = new SendMetalRequest();
            request.from = MinigameRoom.Smelting;
            request.to = MinigameRoom.Casting;
            request.metal = currentlyInForge;
            request.grade = Mathf.RoundToInt(grade);
            MobileNetworkClient.Instance.SendMetal(request);
            Debug.Log($"Grade: {grade.ToString("F0")}");
            StartCoroutine(RemoveMetal());  
            //Remove current metal
            //Add new Metal;
            
            //grade would be 10 - (timer - currentCooktime)

        }
    }
    IEnumerator RemoveMetal()
    {
     
        //Start tween
        moltenMetal.transform.DOMoveY(-11, 2);
        yield return new WaitForSeconds(2);
        currentlyInForge = Metal.None;
        if (forgeBacklog.Count > 0)
        {
            InitializeNewMetalInForge();
        }
        yield break;
    }
    
    private void InitializeNewMetalInForge()
    {
        currentlyInForge = forgeBacklog[0];
        forgeBacklog.RemoveAt(0);
        MetalData mD = helper.GetMetalData(currentlyInForge);
        timer = 0;
        currentTargetHeat = mD.targetHeat;
        currentHeatMargin = mD.heatMargin;
        currentCookTime = mD.cookTime;
        moltenMetal.GetComponent<MeshRenderer>().material = helper.GetMaterial(currentlyInForge);
        StartCoroutine(AddNewMetal());
        
    }
    IEnumerator AddNewMetal()
    {
        moltenMetal.transform.DOMoveY(-6, 2);
        yield return new WaitForSeconds(2);
        yield break;
    }
}
