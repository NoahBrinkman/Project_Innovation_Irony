using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/OreColourHelper", order = 1)]
    public class OreColourHelper : ScriptableObject
    {
        [SerializeField] private List<MetalColor> colors = new List<MetalColor>();

        public Material GetMaterial(metals metal)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i].Metal == metal) return colors[i].mat;
            }

            return null;
        }
    }

[Serializable]
public class MetalColor
{
    public metals Metal;
    public Material mat;
}


    
