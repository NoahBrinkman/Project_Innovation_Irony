using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class FireSize : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ReadMicInput rmi;
    [SerializeField] CamShake CS;
    VisualEffect fire;
    ExposedProperty EP;
    float fireSize;
    void Start()
    {
        EP = "FlameSize";
        fire = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        fireSize = Mathf.SmoothStep(4, 8, rmi.volume/2);
        fire.SetFloat(EP, fireSize);
        Debug.Log(rmi.volume);
        CS.shakeAmount = rmi.loudness/2;
    }
}
