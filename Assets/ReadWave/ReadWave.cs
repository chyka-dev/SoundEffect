﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ReadWave : MonoBehaviour
{
    [SerializeField]
    string filepath;

    MonoPCM pcm;
    IEnumerator<float> samples;

    void Awake()
    {
        pcm = Wave.ReadMono(filepath);
        samples = Sample(pcm.Samples).GetEnumerator();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for(int n = 0; n < data.Length; n = n + channels)
        {
            data[n] = samples.Current;
            samples.MoveNext();
        }
    }

    public IEnumerable<float> Sample(float[] original)
    {
        int phase = 0;

        while(true)
        {
            var sample = original[phase++];
            if(phase >= original.Length)
            {
                phase = 0;
            }

            yield return sample;
        }
    }
}
