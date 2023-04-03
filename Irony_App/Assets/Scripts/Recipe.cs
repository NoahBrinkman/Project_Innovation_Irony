using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    public Item item = Item.Axe;
    public int recipeSize = 3;
    [Tooltip("if this is true make sure certain parts of this are considered, none")]public bool isRandom = true;
    public List<metals> metalRecipe = new List<metals>();
}
