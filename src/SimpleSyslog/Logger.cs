using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSyslog
{
  public static class Logger
  {
    static readonly AutoResetEvent AutoResetEvent = new AutoResetEvent(true);
    static UdpClient udpClient;
    static int facility;
    static string sender;

    public static void Initialize(string hostName, int port, string sender = null, int facility = 0)
    {
      udpClient = new UdpClient(hostName, port);
      Logger.sender = sender;
      Logger.facility = facility;
    }

    static void Log(LogLevel logLevel, string message, params object[] args)
    {
      message = string.Format(message, args);
      if (sender == null)
      {
        var stackTrace = new StackFrame(2);
        sender = stackTrace.GetMethod().DeclaringType.Name;
      }
      Task.Factory.StartNew(() => Send(logLevel, message));
    }

    public static void Fatal(string message, params object[] args)
    {
      Log(LogLevel.Fatal, message, args);
    }

    public static void Error(string message, params object[] args)
    {
      Log(LogLevel.Error, message, args);
    }

    public static void Warn(string message, params object[] args)
    {
      Log(LogLevel.Warn, message, args);
    }

    public static void Info(string message, params object[] args)
    {
      Log(LogLevel.Info, message, args);
    }

    public static void Debug(string message, params object[] args)
    {
      Log(LogLevel.Debug, message, args);
    }

    public static void Trace(string message, params object[] args)
    {
      Log(LogLevel.Trace, message, args);
    }

    public static void All(string message, params object[] args)
    {
      Log(LogLevel.All, message, args);
    }

    static void Send(LogLevel logLevel, string message)
    {
      message = string.Format("<{0}>{1} {2} {3} {4}", facility * 8 + (int)logLevel, DateTime.UtcNow.ToString("s"), Dns.GetHostName(), sender, message);
      var bytes = Encoding.ASCII.GetBytes(message);
      AutoResetEvent.WaitOne();
      AutoResetEvent.Reset();
      udpClient.Send(bytes, bytes.Length);
      AutoResetEvent.Set();
    }
  }
}