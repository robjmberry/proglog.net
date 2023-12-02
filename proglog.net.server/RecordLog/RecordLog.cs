namespace proglog.net.server.RecordLog;

public class RecordLog : IRecordLog, IDisposable
{
    private readonly Mutex _mutex = new();

    public List<Record> Records { get; private set; } = [];

    public int Append(byte[] value)
    {
        _mutex.WaitOne();
        var offset = Records.Count;
        Records.Add(new Record(value, offset));
        _mutex.ReleaseMutex();
        return offset;
    }

    public Record Read(int offset)
    {
        _mutex.WaitOne();
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(offset, Records.Count);
        var record = Records[offset];
        _mutex.ReleaseMutex();
        return record;
    }

    public void Dispose() => _mutex?.Dispose();
}