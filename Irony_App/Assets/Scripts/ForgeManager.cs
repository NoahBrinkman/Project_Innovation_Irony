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

   [SerializeField] private Transform moltenMetal;

   [SerializeField] [NotNull] private TMP_Text debugText;
    // Start is called before the first frame update
    void Start()
    {
        micInput = GetComponent<ReadMicInput>();
        ReadSwipeInput.Instance.OnSwipeRight += OnSwipeRight;
    }

    // Update is called once per frame
    void Update()
    {
        if (micInput.volume >= currentTargetHeat - currentHeatMargin &&
            micInput.volume <= currentTargetHeat + currentHeatMargin)
        {
            timer += Time.deltaTime;
        }

        debugText.text = $"Heat value: {micInput.volume}\nTimeTaken: {timer.ToString("F2")}";
        
        if (currentlyInForge == Metal.None && forgeBacklog.Count > 0)
        {
            InitializeNewMetalInForge();
        }   
    }

    private void OnSwipeRight()
    {
        if (timer >= currentCookTime)
        {
            float grade = 10 - (timer - currentCookTime) * gradeSubtractionMultiplier;
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
