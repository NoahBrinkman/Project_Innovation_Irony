using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles, scrapes; // this will keep it private but visible to drag and drop in inspector
    public AudioClip OVERDONE, NORMAL;
    private AudioSource audiosource;
    public float SoundMarg;
    public float SoundMax;

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
        if (SoundMarg <= SoundMax)
        {
            audiosource.clip = NORMAL;
        } else
        {
            audiosource.clip = OVERDONE;
        }
        if (!audiosource.isPlaying) { audiosource.Play();}
        //StartCoroutine(WaitForSound(OVERDONE));
    }

    IEnumerator WaitForSound(AudioClip Sound)
    {
        
        yield return new WaitUntil(() => !audiosource.isPlaying);
        audiosource.Stop();
        
    }
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
