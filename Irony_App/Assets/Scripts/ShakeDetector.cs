using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    
    
    private int mineValue;
    
    private void Start()
    {
        ReadAccelerometerInput.Instance.OnShake += OnShake;
        Input.gyro.enabled = true;
    }

    private void OnDestroy()
    {
        ReadAccelerometerInput.Instance.OnShake -= OnShake;
    }

    private void Update()
    {
        transform.rotation = Input.gyro.attitude;
    }

    void OnShake()
    {
        transform.localScale *= 1.01f;
        Debug.Log("SHAKE");
    }
}
