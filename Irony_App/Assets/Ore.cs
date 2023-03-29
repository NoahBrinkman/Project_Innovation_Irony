using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField, Tooltip("A string for now an enum later")] private string materialType;

    // Start is called before the first frame update
    void Start()
    {
        randomizeOre();   
    
    }

    void randomizeOre()
    {
        Material newMat = new Material(GetComponent<MeshRenderer>().material);
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0:
                newMat.color = Color.blue;
                break;
            case 1: newMat.color = Color.green;
                break;
            case 2: newMat.color = Color.magenta;
                break;
            case 3: newMat.color = Color.white;
                break;
            default: newMat.color = Color.black;
                break;
        }

        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            if (mr.transform == transform) continue;
            mr.material = newMat;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            randomizeOre();
        }
    }
}
