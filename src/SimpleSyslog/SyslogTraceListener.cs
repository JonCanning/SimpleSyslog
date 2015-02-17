using System.Diagnostics;

namespace SimpleSyslog
{
  public class SyslogTraceListener : TraceListener
  {
    public SyslogTraceListener() {}

    public SyslogTraceListener(string hostName, int port, int facility = 16, string sender = null)
    {
      Syslog.Initialize(hostName, port, facility, sender);
    }

    public SyslogTraceListener(string initializeData)
    {
      var dataItems = initializeData.Split(',');
      var hostName = dataItems[0];
      var port = int.Parse(dataItems[1]);
      var facility = dataItems.Length > 2 ? int.Parse(dataItems[2]) : 16;
      var sender = dataItems.Length > 3 ? dataItems[3] : null;
      Syslog.Initialize(hostName, port, facility, sender);
    }

    static void Log(string message)
    {
      var currentStackHeight = Syslog.stackHeight;
      Syslog.stackHeight = 6;
      Syslog.Info(message);
      Syslog.stackHeight = currentStackHeight;
    }

    public override void Write(string message)
    {
      Log(message);
    }

    public override void WriteLine(string message)
    {
      Log(message);
    }
  }
}