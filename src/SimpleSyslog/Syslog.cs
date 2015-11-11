﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSyslog
{
  public static class Syslog
  {
    private static bool useLocalTime;
    private static UdpClient udpClient;
    private static int facility;
    private static string fixedSender;
    internal static int stackHeight = 2;
    public static string MessageFormat { get; set; }

    public static void Initialize(string hostName, int port, int facility = 16, string sender = null, bool useLocalTime = false)
    {
      fixedSender = sender;
      udpClient = new UdpClient(hostName, port);
      Syslog.facility = facility;
      Syslog.useLocalTime = useLocalTime;
    }

    public static void Log(LogLevel logLevel, string sender, string message, params object[] args)
    {
      message = string.Format(message, args);
      sender = (fixedSender ?? sender).Replace(' ', '_');
      Task.Factory.StartNew(() => Send(logLevel, message, sender));
    }

    public static void Log<T>(LogLevel logLevel, string message, params object[] args) where T : class
    {
      Log(logLevel, typeof(T).Name, message, args);
    }

    private static void LogWithSender(LogLevel logLevel, string message, params object[] args)
    {
      var stackTrace = new StackFrame(stackHeight);
      var declaringType = stackTrace.GetMethod().DeclaringType ?? typeof(Syslog);
      Log(logLevel, declaringType.Name, message, args);
    }

    public static void Emergency(object arg)
    {
      Emergency(arg.ToString());
    }

    public static void Emergency(string message, params object[] args)
    {
      LogWithSender(LogLevel.Emergency, message, args);
    }

    public static void Emergency<T>(string message, params object[] args) where T : class
    {
      Log<T>(LogLevel.Emergency, message, args);
    }

    public static void Alert(object arg)
    {
      Alert(arg.ToString());
    }

    public static void Alert(string message, params object[] args)
    {
      LogWithSender(LogLevel.Alert, message, args);
    }

    public static void Alert<T>(string message, params object[] args) where T : class
    {
      Log<T>(LogLevel.Alert, message, args);
    }

    public static void Critical(object arg)
    {
      Critical(arg.ToString());
    }

    public static void Critical(string message, params object[] args)
    {
      LogWithSender(LogLevel.Critical, message, args);
    }

    public static void Critical<T>(string message, params object[] args) where T : class
    {
      Log<T>(LogLevel.Critical, message, args);
    }

    public static void Error(object arg)
    {
      Error(arg.ToString());
    }

    public static void Error(string message, params object[] args)
    {
      LogWithSender(LogLevel.Error, message, args);
    }

    public static void Error<T>(string message, params object[] args) where T : class
    {
      Log<T>(LogLevel.Error, message, args);
    }

    public static void Warn(object arg)
    {
      Warn(arg.ToString());
    }

    public static void Warn(string message, params object[] args)
    {
      LogWithSender(LogLevel.Warn, message, args);
    }

    public static void Warn<T>(string message, params object[] args) where T : class
    {
      Log<T>(LogLevel.Warn, message, args);
    }

    public static void Notice(object arg)
    {
      Notice(arg.ToString());
    }

    public static void Notice(string message, params object[] args)
    {
      LogWithSender(LogLevel.Notice, message, args);
    }

    public static void Notice<T>(string message, params object[] args) where T : class
    {
      Log<T>(LogLevel.Notice, message, args);
    }

    public static void Info(object arg)
    {
      Info(arg.ToString());
    }

    public static void Info(string message, params object[] args)
    {
      LogWithSender(LogLevel.Info, message, args);
    }

    public static void Info<T>(string message, params object[] args) where T : class
    {
      Log<T>(LogLevel.Info, message, args);
    }

    public static void Debug(object arg)
    {
      Debug(arg.ToString());
    }

    public static void Debug(string message, params object[] args)
    {
      LogWithSender(LogLevel.Debug, message, args);
    }

    public static void Debug<T>(string message, params object[] args) where T : class
    {
      Log<T>(LogLevel.Debug, message, args);
    }

    private static void Send(LogLevel logLevel, string message, string sender)
    {
      if (udpClient == null)
        return;
      var messageFormat = MessageFormat ?? "{message}";
      message = messageFormat.Replace("{message}", message);
      message =
        $"<{facility*8 + (int) logLevel}>{(useLocalTime ? DateTime.Now.ToString("s") : DateTime.UtcNow.ToString("s"))} {Dns.GetHostName()} {sender} {message}";
      var bytes = Encoding.ASCII.GetBytes(message);
      udpClient.BeginSend(bytes, bytes.Length, x => udpClient.EndSend(x), null);
    }
  }
}