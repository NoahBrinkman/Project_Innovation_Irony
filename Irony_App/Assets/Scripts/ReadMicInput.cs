using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class ReadMicInput : MonoBehaviour
{
    public float volume { get; private set; }

    [SerializeField] private int sampleWindow;
    private AudioSource source;
    [SerializeField] private Vector3 maxScale;
    private AudioClip micClip;
    [SerializeField] private float sensitivity = 75;
    [SerializeField] private float minimumloudness = .01f;

    public float loudness;
    public float minimumLoudnes { get; private set; }
    private void Start()
    {
        source = GetComponent<AudioSource>();
        MicrophoneToAudioClip();

        
    }

    private void MicrophoneToAudioClip()
    {
        //Get The first microphone of device
        string microphoneName = Microphone.devices[0];
        micClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
        
    }
    
    void Update()
    {
        loudness = GetLoudnessFromMicrophone();
        if (loudness < minimumloudness)
            volume = 0;
        else
            volume = GetLoudnessFromMicrophone() * sensitivity;

    }

    float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), micClip);
    }
    
    float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);
        float totalLoudness = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}
