using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;



public class Ore : MonoBehaviour
{
    [SerializeField] private OreHelper colorHelper;

    [SerializeField] private int health = 5;
    private int _health;
    public metals Metal;

    private CinemachineVirtualCamera cinCam;
    private CinemachineVirtualCamera mainCam;
    private bool isSelected = false;

    private bool isHeldDown = false;

    private bool beenMined = false;

    public Action<metals> onMined;
    // Start is called before the first frame update
    void Start()
    {
        //    Initialize(Metal);
        
    }

    public Ore Initialize(metals metal)
    {
        Material myMat = colorHelper.GetMaterial(metal);
        //This will change for something else later
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            if(mr.transform == transform) continue;
            mr.material = myMat;
        }

        _health = health;
        isSelected = false;
        beenMined = false;
        ReadAccelerometerInput.Instance.OnShake += OnShaken;
        cinCam = GetComponentInChildren<CinemachineVirtualCamera>();
        mainCam = GameObject.FindWithTag("MainVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        return this;
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
        cinCam.m_Priority = 11;
        mainCam.m_Priority = 10;
    }

    private void OnShaken()
    {
        if (!isSelected || beenMined)
        {
            return;
        }
        Debug.Log("Mined");
        beenMined = true;
        _health--;
        if (_health <= 0)
        {
            mainCam.m_Priority = 11;
            cinCam.m_Priority = 10;
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 2.5f);
        }
        transform.localScale *= .95f;
        ReadAccelerometerInput.Instance.OnEndShake += OnNoLongerShaken;

    }

    private void OnNoLongerShaken()
    {
        beenMined = false;
        Debug.Log("No longer mined");
        ReadAccelerometerInput.Instance.OnEndShake -= OnNoLongerShaken;
    }
}
