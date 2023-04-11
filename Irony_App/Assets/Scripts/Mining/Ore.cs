using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using shared;
using UnityEngine;
using Random = UnityEngine.Random;



public class Ore : MonoBehaviour
{
    [SerializeField] private OreHelper colorHelper;

    [SerializeField] private int health = 5;
    private int _health;
    public Metal Metal;

    private CinemachineVirtualCamera cinCam;
    private CinemachineVirtualCamera mainCam;
    private bool isSelected = false;

    private bool isHeldDown = false;

    private bool beenMined = false;

<<<<<<< HEAD:Irony_App/Assets/Scripts/Mining/Ore.cs
    public Action<Metal> onMined;

    private MeshRenderer mR;
    // Start is called before the first frame update
    void Start()
    {
        //    Initialize(Metal);
        mR = GetComponent<MeshRenderer>();
=======
    public Action<metals> onMined;

    //get the moving minecart
    private Minecart mc;
    
    void Start()
    {
        //    Initialize(Metal);
        //Get "Minecart" Script from gameObject
        mc = FindAnyObjectByType<Minecart>().GetComponent<Minecart>();
        
>>>>>>> DesignerTrench:Irony_App/Assets/Scripts/Ore.cs
    }

    public Ore Initialize(Metal metal)
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
                mc.minecartStop = false;
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
        mc.minecartStop = true;


    }

    private void OnShaken()
    {
        if (!isSelected || beenMined || _health <= 0)
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
<<<<<<< HEAD:Irony_App/Assets/Scripts/Mining/Ore.cs
            if(mR != null)
                mR.enabled = false;
=======
            GetComponent<MeshRenderer>().enabled = false;
            mc.minecartStop = false;
>>>>>>> DesignerTrench:Irony_App/Assets/Scripts/Ore.cs
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
