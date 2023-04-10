using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using shared;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    public shared.Recipe recipe;
}
