using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    [SerializeField] private Vector3 oreScale = new Vector3(0.1f, 0.1f, 0.1f);
    private List<Vector3> takenPositions = new List<Vector3>();
    private void Start()
    {
        //Initialize(debugRecipes);
        Debug.Log("Adding recipe Handling");
        if(MobileNetworkClient.Instance != null)
        {
            MobileNetworkClient.Instance.OnRecipeReceived += OnRecipeReceived;
            if (MobileNetworkClient.Instance.recipeBacklog != null)
            {
                OnRecipeReceived(MobileNetworkClient.Instance.recipeBacklog);
                MobileNetworkClient.Instance.recipeBacklog = null;
            }
        }
        else
        {
            for (int i = 0; i < randomExtraOres; i++)
            {
                SpawnOre((Metal)Random.Range(0, Enum.GetNames(typeof(Metal)).Length));
            }
        }


    }

    private void OnRecipeReceived(Recipe r)
    {
        Initialize(r);
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
            SpawnOre((Metal)Random.Range(1,Enum.GetNames(typeof(Metal)).Length));
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
       newOre.transform.localScale = new Vector3();
       newOre.transform.DOScale(Vector3.one, .25f);
       newOre.GetComponent<Ore>().Initialize(metal).onMined += OnOreMinded;
    }

    void OnOreMinded(Metal metal)
    {
        minedOres.Add(metal);
        //send metal to forge
        if (recipes[0].metalRecipe.ContainsSequence(minedOres) || recipes[0].metalRecipe.ContainsAll(minedOres))
        {
            SendMetalsRequest request = new SendMetalsRequest();
            request.from = MinigameRoom.Mining;
            request.to = MinigameRoom.Cleaning;
            int grade = 10;
            for (int i = 0; i < recipes[0].recipeSize; i++)
            {
                if (recipes[0].metalRecipe[i] != minedOres[i])
                {
                    grade -= 3;
                }
                request.metals.Add(recipes[0].metalRecipe[i]);
                minedOres.Remove(recipes[0].metalRecipe[i]);
                
            }

            request.grade = grade;
            //recipes.RemoveAt(0);
            //Remove the currect ores
            //Send "Ore sent" accuracy grade
        }
        
    }
}


