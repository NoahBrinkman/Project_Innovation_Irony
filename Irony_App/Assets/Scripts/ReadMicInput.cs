using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadMicInput : MonoBehaviour
{
    private float volume;
    [SerializeField] private Transform scaleIdentifier;
    [SerializeField] private int sampleWindow;
    private AudioSource source;
    [SerializeField] private Vector3 maxScale;
    private AudioClip micClip;
    [SerializeField] private float sensitivity = 75;
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
        if (Input.GetMouseButton(0))
        {
            float volume = GetLoudnessFromMicrophone() * sensitivity;
            scaleIdentifier.transform.localScale = Vector3.Lerp(Vector3.one, maxScale,volume);
        }
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
