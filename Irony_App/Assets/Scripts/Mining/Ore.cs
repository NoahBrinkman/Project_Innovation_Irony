using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cinemachine;
using DG.Tweening;
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

    public bool beenMined = false;
    public bool beenChipped = false;
    public Action<Metal> onMined;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private ParticleSystem Rocks, Sparks;
    [SerializeField] private ParticleSystem Reveal;

    public GameObject Dwayne;
    public GameObject TheRock;

    [SerializeField] private CamShake camTarget;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip mine, lastMine;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize(Metal);
        meshRenderer = GetComponent<MeshRenderer>();
        ps = GetComponent<ParticleSystem>();
        

    }
    public Ore Initialize(Metal metal)
    {
        Material myMat = colorHelper.GetMaterial(metal);
        //This will change for something else later
         meshRenderer.material = myMat;
         ps.GetComponent<Renderer>().material = myMat;
         Reveal.GetComponent<Renderer>().material = myMat;
        _health = health;
        isSelected = false;
        beenMined = false;
        audioSource.clip = mine;
        ReadAccelerometerInput.Instance.OnShake += OnShaken;
        cinCam = GetComponentInChildren<CinemachineVirtualCamera>();
        mainCam = GameObject.FindWithTag("MainVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        return this;
    }

    public void Update()
    {
        if (beenMined) return;
        if (isSelected && !isHeldDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSelected = false;
                cinCam.m_Priority--;
                mainCam.m_Priority++;
               // mc.minecartStop = false;
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
        // mc.minecartStop = true;

    }

    private void OnShaken()
    {
        if (!isSelected || beenChipped || _health <= 0)
        {
            return;
        }
        camTarget.GetComponent<CamShake>().shakeDuration = 0.2f;
        Sparks.Play();
        Handheld.Vibrate();
        beenChipped = true;
        Debug.Log("Mined");
        _health--;
        audioSource.Play();
        if (_health <= 0)
        {
            Instantiate(TheRock, Dwayne.transform.position, Quaternion.identity);
            Destroy(Dwayne);
            Rocks.Play();
            Reveal.Play();
            camTarget.GetComponent<CamShake>().shakeDuration = 0.75f;
            beenMined = true;
            audioSource.clip = lastMine;
            audioSource.Play();
            ReadSwipeInput.Instance.OnSwipeLeft += SendOffRejected;
            ReadSwipeInput.Instance.OnSwipeRight += SendOffAccepted;
            if(meshRenderer != null)
                meshRenderer.enabled = false;

        }
        transform.localScale *= .95f;
        ReadAccelerometerInput.Instance.OnEndShake += OnNoLongerShaken;

    }

    private void OnNoLongerShaken()
    {
        beenChipped = false;
        ReadAccelerometerInput.Instance.OnEndShake -= OnNoLongerShaken;
    }

    private void SendOffAccepted()
    {
        ReadSwipeInput.Instance.OnSwipeLeft -= SendOffRejected;
        ReadSwipeInput.Instance.OnSwipeRight -= SendOffAccepted;
        StartCoroutine(DisableObject(true));
        onMined?.Invoke(Metal);
    }

    private void SendOffRejected()
    {            
        ReadSwipeInput.Instance.OnSwipeLeft -= SendOffRejected;
        ReadSwipeInput.Instance.OnSwipeRight -= SendOffAccepted;
        
        StartCoroutine(DisableObject(false));
    }

    private IEnumerator DisableObject(bool accepted)
    {
      
        mainCam.m_Priority = 11;
        cinCam.m_Priority = 10;
        yield return new WaitForSeconds(2.5f);
        transform.DOMoveX(accepted ? 50 : -50,1).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        yield break;
        
    }
    

}
