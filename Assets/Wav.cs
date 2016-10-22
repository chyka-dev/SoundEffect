using UnityEngine;
using System.Collections;

public struct Wav
{
    public Wav(
        int riffChunkId,
        int riffChunkSize,
        int riffFormatType,
        int fmtChunkId,
        int fmtChunkSize,
        short fmtChunkFormatType,
        short fmtChannel,
        int fmtSamplesPerSec,
        int fmtBytesPerSec,
        short fmtBlockSize,
        short fmtBitsPerSample,
        int dataChunkId,
        int dataChunkSize,
        byte[] data = null
    )
    {
        RiffChunkId = riffChunkId;
        RiffChunkSize = riffChunkSize;
        RiffFormatType = riffFormatType;
        FmtChunkId = fmtChunkId;
        FmtChunkSize = fmtChunkSize;
        FmtChunkFormatType = fmtChunkFormatType;
        FmtChannel = fmtChannel;
        FmtSamplesPerSec = fmtSamplesPerSec;
        FmtBytesPerSec = fmtBytesPerSec;
        FmtBlockSize = fmtBlockSize;
        FmtBitsPerSample = fmtBitsPerSample;
        DataChunkId = dataChunkId;
        DataChunkSize = dataChunkSize;
        Data = data;
    }

    public int RiffChunkId { get; private set; }

    public int RiffChunkSize { get; private set; }

    public int RiffFormatType { get; private set; }

    public int FmtChunkId { get; private set; }

    public int FmtChunkSize { get; private set; }

    public short FmtChunkFormatType { get; private set; }

    public short FmtChannel { get; private set; }

    public int FmtSamplesPerSec { get; private set; }

    public int FmtBytesPerSec { get; private set; }

    public short FmtBlockSize { get; private set; }

    public short FmtBitsPerSample { get; private set; }

    public int DataChunkId { get; private set; }

    public int DataChunkSize { get; private set; }

    public byte[] Data { get; set; }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="Wav"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current <see cref="Wav"/>.</returns>
    public override string ToString()
    {
        return string.Format("[Wav: RiffChunkId={0}, RiffChunkSize={1}, RiffFormatType={2}, FmtChunkId={3}, FmtChunkSize={4}, FmtChunkFormatType={5}, FmtChannel={6}, FmtSamplesPerSec={7}, FmtBytesPerSec={8}, FmtBlockSize={9}, FmtBitsPerSample={10}, DataChunkId={11}, DataChunkSize={12}]", RiffChunkId, RiffChunkSize, RiffFormatType, FmtChunkId, FmtChunkSize, FmtChunkFormatType, FmtChannel, FmtSamplesPerSec, FmtBytesPerSec, FmtBlockSize, FmtBitsPerSample, DataChunkId, DataChunkSize);
    }
}
