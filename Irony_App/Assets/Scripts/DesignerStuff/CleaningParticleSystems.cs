using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningParticleSystems : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles, scrapes; // this will keep it private but visible to drag and drop in inspector
    public AudioClip OVERDONE, NORMAL;
    private AudioSource audiosource;
    public float SoundMarg;
    public float SoundMax;

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
        Debug.Log(SoundMarg);
        Debug.Log(SoundMax);

    }
}
