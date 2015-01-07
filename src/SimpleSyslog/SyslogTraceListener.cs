using System.Diagnostics;

namespace SimpleSyslog
{
  public class SyslogTraceListener : TraceListener
  {
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
