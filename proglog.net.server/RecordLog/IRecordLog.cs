namespace proglog.net.server.RecordLog;

public interface IRecordLog
{
    int Append(byte[] value);

    Record Read(int offset);
}