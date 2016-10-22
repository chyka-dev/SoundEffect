using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Wave
{
    public static Wav ReadWav(string fpath)
    {
        TextAsset asset = Resources.Load(fpath) as TextAsset;
        Stream s = new MemoryStream(asset.bytes);
        BinaryReader reader = new BinaryReader(s);

        int riffChunkId = reader.ReadInt32();
        int riffChunkSize = reader.ReadInt32();
        int riffFormatType = reader.ReadInt32();

        int fmtChunkId = reader.ReadInt32();
        int fmtChunkSize = reader.ReadInt32();
        short fmtChunkFormatType = reader.ReadInt16();
        short fmtChannel = reader.ReadInt16();
        int fmtSamplesPerSec = reader.ReadInt32();
        int fmtBytesPerSec = reader.ReadInt32();
        short fmtBlockSize = reader.ReadInt16();
        short fmtBitsPerSample = reader.ReadInt16();

        int dataChunkId = reader.ReadInt32();
        int dataChunkSize = reader.ReadInt32();

        byte[] data = reader.ReadBytes(dataChunkSize);

        var wav = new Wav(
                      riffChunkId,
                      riffChunkSize,
                      riffFormatType,
                      fmtChunkId,
                      fmtChunkSize,
                      fmtChunkFormatType,
                      fmtChannel,
                      fmtSamplesPerSec,
                      fmtBytesPerSec,
                      fmtBlockSize,
                      fmtBitsPerSample,
                      dataChunkId,
                      dataChunkSize,
                      data
                  );

        reader.Close();
        s.Close();

        return wav;
    }

    public static MonoPCM ReadMono(string fpath)
    {
        var wav = ReadWav(fpath);
        int bytesPerSample = wav.FmtBitsPerSample / 8;
        int numSamples = wav.DataChunkSize / bytesPerSample;
        float[] samples = new float[numSamples];
        for(int i = 0; i < numSamples; i++)
        {
            if(bytesPerSample == 1)
            {
                samples[i] = (float)wav.Data[i] / 256;
            }
            else
            {
                var sample = g.BytesToShort(new byte[] { wav.Data[i * 2], wav.Data[(i * 2) + 1] });
                samples[i] = (float)sample / (short.MaxValue + 1);
            }
        }

        MonoPCM pcm = new MonoPCM(
                          wav.FmtSamplesPerSec,
                          wav.FmtBitsPerSample,
                          samples
                      );

        return pcm;
    }

    public static bool WriteMono(MonoPCM pcm, string fpath)
    {
        var path = Path.Combine(Application.dataPath, fpath);
        FileStream s = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
        var bw = new BinaryWriter(s);

        bw.Write('R');
        bw.Write('I');
        bw.Write('F');
        bw.Write('F');
        bw.Write(36 + (pcm.NumSamples * pcm.BytesPerSample));
        bw.Write('W');
        bw.Write('A');
        bw.Write('V');
        bw.Write('E');

        bw.Write('f');
        bw.Write('m');
        bw.Write('t');
        bw.Write(' ');
        bw.Write(16); // fmtチャンクのサイズ.
        bw.Write((short)1); // wav format type
        bw.Write((short)1); // channel
        bw.Write(pcm.Fs);
        bw.Write(pcm.Fs * pcm.BytesPerSample);
        bw.Write((short)pcm.BytesPerSample);
        bw.Write((short)pcm.BitsPerSample);

        bw.Write('d');
        bw.Write('a');
        bw.Write('t');
        bw.Write('a');
        bw.Write(pcm.NumSamples * pcm.BytesPerSample);
        bw.Write(pcm.SamplesInBytes);

        bw.Flush();
        s.Flush();
        bw.Close();
        s.Close();
        return true;
    }
}
