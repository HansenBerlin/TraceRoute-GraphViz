using System.Net.NetworkInformation;

namespace TraceRoute;

public class TracertEntry
{
    private readonly int _hopId;
    private readonly long _roundtripTime;
    private readonly string _address;
    private readonly string _hostname;
    private readonly long _replyTime;
    private readonly IPStatus _replyStatus;
    
    public TracertEntry(int hopId, string address, string hostname, long replyTime, IPStatus replyStatus, long roundtripTime)
    {
        _hopId = hopId;
        _address = address;
        _hostname = hostname;
        _replyTime = replyTime;
        _replyStatus = replyStatus;
        _roundtripTime = roundtripTime;
    }

    public void Print()
    {
        string hostName = string.IsNullOrEmpty(_hostname) ? _address : $"{_hostname} [{_address}]";
        string replyStatus = _replyStatus == IPStatus.TimedOut ? "Request Timed Out." : $"{_replyTime} ms";
        string print = $"{_hopId} | {hostName} | {replyStatus} | {_roundtripTime}";
        Console.WriteLine(print);
    }
}