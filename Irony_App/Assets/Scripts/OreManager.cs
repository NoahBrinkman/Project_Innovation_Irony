using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum metals
{
    none,
    copper,
    iron,
    gold,
    diamond,
}


public class OreManager : MonoBehaviour
{
    [SerializeField,Tooltip("The host will decide these randomly")] private List<Recipe> debugRecipes;
    [SerializeField] private GameObject oreObject;

    private List<Recipe> recipes;
    
    private List<metals> minedOres;

    [SerializeField] private int randomExtraOres = 5;
    [SerializeField] private List<Collider> bounds;
    [SerializeField] private float minimumDistanceBetweenOres = .75f;
    private List<Vector3> takenPositions = new List<Vector3>();
    private void Start()
    {
        Initialize(debugRecipes);

    }

    public void Initialize(List<Recipe> Precipes)
    {
        minedOres = new List<metals>();
        recipes = Precipes;
        for (int i = 0; i < recipes.Count; i++)
        {
            for (int j = 0; j < recipes[i].metalRecipe.Count; j++)
            {
                //
                SpawnOre(recipes[i].metalRecipe[j]);
            }
        }
        for (int i = 0; i < randomExtraOres; i++)
        {
            SpawnOre((metals)Random.Range(0,Enum.GetNames(typeof(metals)).Length));
        }
    }

    private void SpawnOre(metals metal)
    {
       Vector3 spawnPosition = Vector3.zero;
       
        Collider spawnBound = bounds[Random.Range(0, bounds.Count)];

        do
        {
           float x = Random.Range(spawnBound.bounds.min.x, spawnBound.bounds.max.x);
            float y = Random.Range(spawnBound.bounds.min.y, spawnBound.bounds.max.y);
            float z = Random.Range(spawnBound.bounds.min.z, spawnBound.bounds.max.z );
            spawnPosition = new Vector3(x, y, z);

        } while (takenPositions.Any(p => Vector3.Distance(p,spawnPosition) < minimumDistanceBetweenOres));
        
     
       GameObject newOre = Instantiate(oreObject, spawnPosition,  
           spawnBound.transform.rotation);
       newOre.GetComponent<Ore>().Initialize(metal).onMined += OnOreMinded;
    }

    void OnOreMinded(metals metal)
    {
        minedOres.Add(metal);
        if (recipes[0].metalRecipe.ContainsSequence(minedOres))
        {
            recipes.RemoveAt(0);
            //Remove the currect ores
            //Send accuracy grade
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
}


