using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/OreColourHelper", order = 1)]
    public class OreHelper : ScriptableObject
    {
        [SerializeField] private List<MetalData> colors = new List<MetalData>();

        public Material GetMaterial(metals metal)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i].Metal == metal) return colors[i].mat;
            }

            return null;
        }
        
        public float GetRoughness(metals metal)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i].Metal == metal) return colors[i].roughness;
            }

            return 0;
        }

        public MetalData GetMetalData(metals metal)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i].Metal == metal) return colors[i];
            }

            return null;
        }
    }

[Serializable]
public class MetalData
{
    [Header("General")]
    public metals Metal;
    public Material mat;
    [Header("Cleaning")]
    [Tooltip("Shaking the phone will make a value increase, roughness will subtract itself from this")]
    public float roughness = .5f;
    public float perfectCleaningValue = 3.5f;
    public float cleaningGradeMargin = .5f;
    [Header("Smelting")]
    public float targetHeat;
    public float heatMargin;
    public float cookTime;

}


    
