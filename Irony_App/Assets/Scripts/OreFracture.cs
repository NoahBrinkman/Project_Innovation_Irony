using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreFracture : MonoBehaviour
{
    public GameObject fractured;

    public Ore ore;

    void Start()
    {
        ore = GetComponent<Ore>();
    }
    public void Update()
    {
        if (ore.beenMined)
        {
            Instantiate(fractured, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }

    }
}
