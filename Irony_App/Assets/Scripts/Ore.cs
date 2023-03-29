using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;



public class Ore : MonoBehaviour
{
    [SerializeField] private OreColourHelper colorHelper;
    public metals Metal;

    private CinemachineVirtualCamera cinCam;
    private CinemachineVirtualCamera mainCam;
    private bool isSelected = false;

    private bool isHeldDown = false;
    // Start is called before the first frame update
    void Start()
    {
        //    Initialize(Metal);
    
    }

    public void Initialize(metals metal)
    {
        Material myMat = colorHelper.GetMaterial(metal);
        //This will change for something else later
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            if(mr.transform == transform) continue;
            mr.material = myMat;
        }

        cinCam = GetComponentInChildren<CinemachineVirtualCamera>();
        mainCam = GameObject.FindWithTag("MainVirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (isSelected && !isHeldDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSelected = false;
                cinCam.m_Priority--;
                mainCam.m_Priority++;
                
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHeldDown = false;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("äm called");
        //Set my camera as the one
        isSelected = true;
        isHeldDown = true;
        cinCam.m_Priority++;
        mainCam.m_Priority--;;
    }
}
