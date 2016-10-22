using System;
using System.Collections;
using System.IO;
using UnityEngine;

public struct MonoPCM
{
    public MonoPCM(int fs, int bits, float[] samples)
    {
        Fs = fs;
        BitsPerSample = bits;
        Samples = samples;
    }

    /// <summary>
    /// 標本化周波数.
    /// </summary>
    /// <value>The fs.</value>
    public int Fs { get; set; }

    /// <summary>
    /// 量子化ビット数.
    /// </summary>
    /// <value>The bits.</value>
    public int BitsPerSample { get; set; }

    /// <summary>
    /// 量子化バイト数.
    /// </summary>
    /// <value>The bytes.</value>
    public int BytesPerSample { get { return BitsPerSample / 8; } }

    /// <summary>
    /// サンプル数.
    /// </summary>
    /// <value>The length.</value>
    public int NumSamples { get { return Samples.Length; } }

    /// <summary>
    /// サンプル.
    /// </summary>
    /// <value>The samples.</value>
    public float[] Samples { get; private set; }

    /// <summary>
    /// サンプルをバイトに変換します.
    /// 量子化ビット数が8の場合は1byte/sample, 16の場合は2bytes/sample.
    /// </summary>
    /// <value>The samples in bytes.</value>
    public byte[] SamplesInBytes
    {
        get
        {
            byte[] ret = new byte[NumSamples * BytesPerSample];
            for(int i = 0; i < NumSamples; i++)
            {
                var s = Samples[i];
                s += 1.0f; // [0 : 2]
                s /= 2; // [0 : 1]

                if(BytesPerSample == 2)
                {
                    s *= UInt16.MaxValue + 1; // [0 : 65536]
                    if(s > UInt16.MaxValue)
                    {
                        s = (float)UInt16.MaxValue;
                    }
                    else if(s < 0)
                    {
                        s = 0.0f;
                    }

                    s += 0.5f; // 四捨五入用
                    s -= Int16.MaxValue + 1;
                    var bytes = BitConverter.GetBytes((short)s);

                    // 0 1 2 4 <- sample index
                    // 0 2 4 6 <- ret index
                    ret[i * 2] = bytes[0];
                    ret[(i * 2) + 1] = bytes[1];
                    continue;
                }

                // Bytes == 1
                s *= Byte.MaxValue + 1; // [0 : 256]
                if(s > Byte.MaxValue)
                {
                    s = (float)Byte.MaxValue;
                }
                else if(s < 0)
                {
                    s = 0f;
                }

                s += 0.5f;
                s -= Byte.MaxValue + 1;
                ret[i] = BitConverter.GetBytes((char)s)[0];
            }

            return ret;
        }
    }
}

public struct StereoPCM
{
    public StereoPCM(int fs, int bits, int len, float[] lsamples, float[] rsamples)
    {
        Fs = fs;
        Bits = bits;
        Length = len;
        LSamples = lsamples;
        RSamples = rsamples;
    }

    public int Fs { get; private set; }

    public int Bits { get; private set; }

    public int Length { get; private set; }

    public float[] LSamples { get; private set; }

    public float[] RSamples { get; private set; }
}


public static class g
{
    public static float BytesToFloat(byte[] bytes)
    {
        return BitConverter.ToSingle(bytes, 0);
    }

    public static double BytesToDouble(byte[] bytes)
    {
        return BitConverter.ToDouble(bytes, 0);
    }

    public static string BytesToString(byte[] bytes)
    {
        var sb = new System.Text.StringBuilder(bytes.Length);
        foreach(var b in bytes)
        {
            sb.Append((char)b);
        }

        return sb.ToString();
    }

    public static int BytesToInt(byte[] bytes)
    {
        return BitConverter.ToInt32(bytes, 0);
    }

    public static short BytesToShort(byte[] bytes)
    {
        return BitConverter.ToInt16(bytes, 0);
    }

    public static byte[] Convert2LittleEndian(byte[] bytes)
    {
        byte[] newBytes = new byte[bytes.Length];
        bytes.CopyTo(newBytes, 0);

        if(BitConverter.IsLittleEndian)
        {
            Array.Reverse(newBytes);
        }

        return newBytes;
    }

    public static void d(object o)
    {
        Debug.Log(o);
    }
}