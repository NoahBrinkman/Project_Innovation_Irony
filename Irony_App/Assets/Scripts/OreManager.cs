using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private List<Transform> possibleSpawnLocations = new List<Transform>();
    private List<Transform> bufferTransforms;
    private List<Recipe> recipes;

    private List<Ore> ores;

    [SerializeField] private int randomExtraOres = 5;
    [SerializeField] private Collider bounds;
    private void Start()
    {
        bufferTransforms = new List<Transform>(possibleSpawnLocations);
        Initialize(debugRecipes);

    }

    public void Initialize(List<Recipe> Precipes)
    {
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
       /* Vector3 spawnPosition = Vector3.zero;
        float randomX ;
        float randomY ;
        float randomZ ;
        do
        {
            randomX = Random.Range(bounds.bounds.min.x - 10, bounds.bounds.max.x + 10);
            randomY = Random.Range(bounds.bounds.min.y-10, bounds.bounds.max.y +10);
            randomZ = Random.Range(bounds.bounds.min.z - 10, bounds.bounds.max.z + 10);
        } while (bounds.bounds.Contains(new Vector3(randomX,randomY,randomZ)));
        
        spawnPosition = new Vector3(randomX, randomY, randomZ);
        GameObject newOre = Instantiate(oreObject, bounds.ClosestPointOnBounds(spawnPosition), quaternion.identity);
        Ore oreComponent = newOre.GetComponent<Ore>();
        oreComponent.Initialize(metal);*/
       int r = Random.Range(0, bufferTransforms.Count);
       Transform spawnPoint = bufferTransforms[r];
       bufferTransforms.Remove(bufferTransforms[r]);
       GameObject newOre = Instantiate(oreObject, spawnPoint.position, spawnPoint.rotation);
       newOre.GetComponent<Ore>().Initialize(metal);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
