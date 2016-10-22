using UnityEngine;
using System.Collections;

public class WriteWave : MonoBehaviour
{
    void Awake()
    {
        MonoPCM pcm = Wave.ReadMono("a.wav");
        pcm.BitsPerSample = 8;
        Wave.WriteMono(pcm, "WriteWave/mine.wav");
    }
}
