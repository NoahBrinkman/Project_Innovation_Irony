using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadAccelerometerInput : MonoBehaviour
{
    private static ReadAccelerometerInput _instance;


    public static ReadAccelerometerInput Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;
    }

    public Action OnShake;
    
    public Action OnEndShake;
    [SerializeField] private float threshold = 2;
   // private float updateInterval = 1.0f / 60.0f;
    //private float lowPassKernelInSeconds = 1.0f;
    private float lowPassFilterFactor;
    private Vector3 lowPassValue;
    
    private void Start()
    {
        threshold *= threshold;
        lowPassValue = Input.acceleration;

    }

    private void Update()
    {
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;
        if (deltaAcceleration.sqrMagnitude >= threshold)
        {
            OnShake?.Invoke();
            
        }
        else
        {
            OnEndShake?.Invoke();
        
        }
    }

    private void FixedUpdate()
    {
        
    }
}
