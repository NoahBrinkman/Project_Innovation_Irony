using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles, scrapes, done; // this will keep it private but visible to drag and drop in inspector
    public AudioClip OVERDONE, NORMAL, DONE;
    private AudioSource audiosource;
    public AudioSource Victory;
    public float SoundValue;
    public float SoundMarg;
    public float SoundMax;
    private float VolumeS;

    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }
    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Start()
    {
        ReadAccelerometerInput.Instance.OnShake += PlayParticleSystem;
        audiosource = GetComponent<AudioSource>();

    }


    void PlayParticleSystem()
    {
        particles.Play();
        scrapes.Play();
        Handheld.Vibrate();
        shakeDuration = 0.15f;
        if (SoundValue <= SoundMax - SoundMarg)
        {

            audiosource.clip = NORMAL;

        }
        else if (SoundValue >= SoundMax - SoundMarg && SoundValue <= SoundMax + SoundMarg)
        {
            audiosource.clip = DONE;
            done.Play();
        } 
        else
        {
            audiosource.clip = OVERDONE;
            
        }
        if (!audiosource.isPlaying) { audiosource.Play();}
        //StartCoroutine(WaitForSound(Victory));
        
    }

    //IEnumerator WaitForSound(AudioSource Sound)
    //{
        
    //    yield return new WaitUntil(() => Victory.isPlaying);
    //    Destroy(Victory);

    //}
    private void Update()
    {
        //Debug.Log(SoundMarg);
        //.Log(SoundMax);
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}
