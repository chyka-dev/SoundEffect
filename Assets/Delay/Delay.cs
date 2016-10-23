using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Delay : MonoBehaviour
{
    [SerializeField]
    string filepath;

    /// <summary>
    /// 減衰率.
    /// </summary>
    [SerializeField]
    [Range(0.1f, 1f)]
    float attenuation;

    /// <summary>
    /// 遅延時間 (sec).
    /// </summary>
    [SerializeField]
    float delay;

    /// <summary>
    /// 繰り返し回数.
    /// </summary>
    [SerializeField]
    [Range(1, 10)]
    int repeat;

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
