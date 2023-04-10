using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using shared;

public class OreManager : MonoBehaviour
{
    [SerializeField,Tooltip("The host will decide these randomly")] private List<Recipe> debugRecipes;
    [SerializeField] private GameObject oreObject;

    private List<Recipe> recipes = new List<Recipe>();
    
    private List<Metal> minedOres;

    [SerializeField] private int randomExtraOres = 5;
    [SerializeField] private List<Collider> bounds;
    [SerializeField] private float minimumDistanceBetweenOres = .75f;
    private List<Vector3> takenPositions = new List<Vector3>();
    private void Start()
    {
        Initialize(debugRecipes);

    }
    
    public void Initialize(Recipe Precipe)
    {
        minedOres = new List<Metal>();
        recipes.Add( Precipe);
        for (int j = 0; j < Precipe.metalRecipe.Count; j++)
        {
            SpawnOre(Precipe.metalRecipe[j]);
        }
        for (int i = 0; i < randomExtraOres; i++)
        {
            SpawnOre((Metal)Random.Range(0,Enum.GetNames(typeof(Metal)).Length));
        }
    }
    
    public void Initialize(List<Recipe> Precipes)
    {
        minedOres = new List<Metal>();
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
            SpawnOre((Metal)Random.Range(0,Enum.GetNames(typeof(Metal)).Length));
        }
    }

    private void SpawnOre(Metal metal)
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

    void OnOreMinded(Metal metal)
    {
        minedOres.Add(metal);
        //send metal to forge
        if (recipes[0].metalRecipe.ContainsSequence(minedOres))
        {
            recipes.RemoveAt(0);
            //Remove the currect ores
            //Send "Ore sent" accuracy grade
        }
        
    }
}


