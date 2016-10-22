using UnityEngine;
using System.Collections;
using System;

public class ReadWave : MonoBehaviour
{
    [SerializeField]
    string filepath;
    int phase = 0;
    MonoPCM pcm;

    void Awake()
    {
        pcm = Wave.ReadMono(filepath);
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for(int n = 0; n < data.Length; n = n + channels)
        {
            data[n] = pcm.Samples[phase++];
            if(phase >= pcm.NumSamples)
            {
                phase = 0;
            }

            if(channels == 2)
            {
                data[n + 1] = data[n];
            }
        }
    }
}