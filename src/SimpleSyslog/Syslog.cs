using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSyslog
{
  public static class Syslog
  {
    static UdpClient udpClient;
    static int facility;
    static string fixedSender;
    public static string MessageFormat { get; set; }

    public static void Initialize(string hostName, int port, int facility = 16, string sender = null)
    {
      fixedSender = sender;
      udpClient = new UdpClient(hostName, port);
      Syslog.facility = facility;
    }

    public static void Log(LogLevel logLevel, string sender, string message, params object[] args)
    {
      message = string.Format(message, args);
      sender = (fixedSender ?? sender).Replace(' ', '_');
      Task.Factory.StartNew(() => Send(logLevel, message, sender));
    }

    public static void Log<T>(LogLevel logLevel, string message, params object[] args)
    {
      Log(logLevel, typeof(T).Name, message, args);
    }

    static void LogWithSender(LogLevel logLevel, string message, params object[] args)
    {
      var stackTrace = new StackFrame(2);
      var senderType = stackTrace.GetMethod().DeclaringType;
      Log(logLevel, senderType.Name, message, args);
    }

    public static void Emergency(string message, params object[] args)
    {
      LogWithSender(LogLevel.Emergency, message, args);
    }

    public static void Emergency(string sender, string message, params object[] args)
    {
      Log(LogLevel.Emergency, sender, message, args);
    }

    public static void Emergency<T>(string message, params object[] args)
    {
      Log<T>(LogLevel.Emergency, message, args);
    }

    public static void Alert(string message, params object[] args)
    {
      LogWithSender(LogLevel.Alert, message, args);
    }

    public static void Alert(string sender, string message, params object[] args)
    {
      Log(LogLevel.Alert, sender, message, args);
    }

    public static void Alert<T>(string message, params object[] args)
    {
      Log<T>(LogLevel.Alert, message, args);
    }

    public static void Critical(string message, params object[] args)
    {
      LogWithSender(LogLevel.Critical, message, args);
    }

    public static void Critical(string sender, string message, params object[] args)
    {
      Log(LogLevel.Critical, sender, message, args);
    }

    public static void Critical<T>(string message, params object[] args)
    {
      Log<T>(LogLevel.Critical, message, args);
    }

    public static void Error(string message, params object[] args)
    {
      LogWithSender(LogLevel.Error, message, args);
    }

    public static void Error(string sender, string message, params object[] args)
    {
      Log(LogLevel.Error, sender, message, args);
    }

    public static void Error<T>(string message, params object[] args)
    {
      Log<T>(LogLevel.Error, message, args);
    }

    public static void Warn(string message, params object[] args)
    {
      LogWithSender(LogLevel.Warn, message, args);
    }

    public static void Warn(string sender, string message, params object[] args)
    {
      Log(LogLevel.Warn, sender, message, args);
    }

    public static void Warn<T>(string message, params object[] args)
    {
      Log<T>(LogLevel.Warn, message, args);
    }

    public static void Notice(string message, params object[] args)
    {
      LogWithSender(LogLevel.Notice, message, args);
    }

    public static void Notice(string sender, string message, params object[] args)
    {
      Log(LogLevel.Notice, sender, message, args);
    }

    public static void Notice<T>(string message, params object[] args)
    {
      Log<T>(LogLevel.Notice, message, args);
    }

    public static void Info(string message, params object[] args)
    {
      LogWithSender(LogLevel.Info, message, args);
    }

    public static void Info(string sender, string message, params object[] args)
    {
      Log(LogLevel.Info, sender, message, args);
    }

    public static void Info<T>(string message, params object[] args)
    {
      Log<T>(LogLevel.Info, message, args);
    }

    public static void Debug(string message, params object[] args)
    {
      LogWithSender(LogLevel.Debug, message, args);
    }

    public static void Debug(string sender, string message, params object[] args)
    {
      Log(LogLevel.Debug, sender, message, args);
    }

    public static void Debug<T>(string message, params object[] args)
    {
      Log<T>(LogLevel.Debug, message, args);
    }

    static void Send(LogLevel logLevel, string message, string sender)
    {
      if (udpClient == null)
        return;
      var messageFormat = MessageFormat ?? "{message}";
      message = messageFormat.Replace("{message}", message);
      message = string.Format("<{0}>{1} {2} {3} {4}", facility * 8 + (int)logLevel, DateTime.UtcNow.ToString("s"), Dns.GetHostName(), sender, message);
      var bytes = Encoding.ASCII.GetBytes(message);
      udpClient.BeginSend(bytes, bytes.Length, x => udpClient.EndSend(x), null);
    }
  }
}