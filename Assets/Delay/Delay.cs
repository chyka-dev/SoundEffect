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
        samples = DelayedSamples(pcm.Samples, pcm.Fs).GetEnumerator();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for(int n = 0; n < data.Length; n = n + channels)
        {
            data[n] = samples.Current;
            samples.MoveNext();
        }
    }

    public IEnumerable<float> Samples(float[] original)
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

    public IEnumerable<float> DelayedSamples(float[] original, int fs)
    {
        int phase = 0;

        while(true)
        {
            var sample = original[phase++];

            // 繰り返し回数だけ繰り返す.
            for(int i = 1; i <= repeat; i++)
            {
                // delayから使用する過去のサンプルのインデックスを特定する.
                var index = (int)(phase - (i * (fs * delay)));
                if(index >= 0)
                {
                    // 現在のサンプルに過去の減衰した音を足す.
                    sample += (float)Math.Pow(attenuation, repeat) * original[index];
                }
            }

            if(phase >= original.Length)
            {
                phase = 0;
            }

            yield return sample;
        }
    }
}
