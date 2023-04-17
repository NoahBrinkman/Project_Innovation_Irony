using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    [SerializeField]
    float speed;
    
    private float speedMultiplier;
    private float TimeSpeed;
    public bool minecartStop;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimeSpeed = 1 + Time.deltaTime;
        transform.Translate(Vector3.forward * speedMultiplier * Time.deltaTime);
        //If the player has pressed on an ore, the stop the minecart. When the player clicks the ore away, the minecart moves again.
        if (minecartStop) { speedMultiplier = 0f; } else { speedMultiplier = speed; }
    }
}
