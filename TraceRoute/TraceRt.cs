using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;

namespace TraceRoute;

public class TraceRt
{
    public async Task<List<string>> TracertIpsAsync(string ipAddress, int maxHops, int timeout, bool isPrinted)
    {
        List<string> ips = new();
        IPAddress address = IPAddress.Parse(ipAddress);
        Ping ping = new Ping();
        PingOptions pingOptions = new PingOptions(1, true);
        Stopwatch pingReplyTime = new Stopwatch();
        PingReply reply;
 
        do
        {
            pingReplyTime.Start();
            reply = await ping.SendPingAsync(address, timeout, new byte[] { 0 }, pingOptions);
            pingReplyTime.Stop();
 
            string hostname = string.Empty;
            try
            {
                var ipHostEntry = await Dns.GetHostEntryAsync(reply.Address);
                hostname = ipHostEntry.HostName;
            }
            catch (Exception) { }

            string ipaddress = string.IsNullOrEmpty(hostname) ? reply.Address.ToString() : $"{hostname} [{reply.Address}]";
            ips.Add(ipaddress);
            if (isPrinted)
            {
                var entry = new TracertEntry(pingOptions.Ttl, reply.Address.ToString(), hostname,
                    pingReplyTime.ElapsedMilliseconds, reply.Status, reply.RoundtripTime);
                entry.Print();
            }

            pingOptions.Ttl++;
            pingReplyTime.Reset();
        }
        while (reply.Status != IPStatus.Success && pingOptions.Ttl <= maxHops);

        return ips;
    }
}