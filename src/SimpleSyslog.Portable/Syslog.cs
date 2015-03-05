using System;
using System.Text;
using System.Threading.Tasks;
using Sockets.Plugin;

namespace SimpleSyslog
{
  public static class Syslog
  {
    static UdpSocketClient udpClient;
    static int facility;
    static string fixedSender;
    static string clientName;
    public static string MessageFormat { get; set; }

    public static async Task Initialize(string hostName, int port, string clientName, int facility = 16, string sender = null)
    {
      Syslog.clientName = clientName;
      fixedSender = sender;
      Syslog.facility = facility;
      udpClient = new UdpSocketClient();
      await udpClient.ConnectAsync(hostName, port);
    }

    public static async Task Log(LogLevel logLevel, string sender, string message, params object[] args)
    {
      message = string.Format(message, args);
      sender = (fixedSender ?? sender).Replace(' ', '_');
      await Send(logLevel, message, sender);
    }

    public static async Task Log<T>(LogLevel logLevel, string message, params object[] args) where T : class
    {
      await Log(logLevel, typeof(T).Name, message, args);
    }

    static async Task LogWithSender(string sender, LogLevel logLevel, string message, params object[] args)
    {
      await Log(logLevel, sender, message, args);
    }

    public async static Task Emergency(string sender, object arg)
    {
      await Emergency(sender, arg.ToString());
    }

    public async static Task Emergency(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Emergency, message, args);
    }

    public async static Task Emergency<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Emergency, message, args);
    }

    public async static Task Alert(string sender, object arg)
    {
      await Alert(sender, arg.ToString());
    }

    public async static Task Alert(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Alert, message, args);
    }

    public async static Task Alert<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Alert, message, args);
    }

    public async static Task Critical(string sender, object arg)
    {
      await Critical(sender, arg.ToString());
    }

    public async static Task Critical(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Critical, message, args);
    }

    public async static Task Critical<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Critical, message, args);
    }

    public async static Task Error(string sender, object arg)
    {
      await Error(sender, arg.ToString());
    }

    public async static Task Error(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Error, message, args);
    }

    public async static Task Error<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Error, message, args);
    }

    public async static Task Warn(string sender, object arg)
    {
      await Warn(sender, arg.ToString());
    }

    public async static Task Warn(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Warn, message, args);
    }

    public async static Task Warn<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Warn, message, args);
    }

    public async static Task Notice(string sender, object arg)
    {
      await Notice(sender, arg.ToString());
    }

    public async static Task Notice(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Notice, message, args);
    }

    public async static Task Notice<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Notice, message, args);
    }

    public async static Task Info(string sender, object arg)
    {
      await Info(sender, arg.ToString());
    }

    public async static Task Info(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Info, message, args);
    }

    public async static Task Info<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Info, message, args);
    }

    public async static Task Debug(string sender, object arg)
    {
      await Debug(sender, arg.ToString());
    }

    public async static Task Debug(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Debug, message, args);
    }

    public async static Task Debug<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Debug, message, args);
    }

    static async Task Send(LogLevel logLevel, string message, string sender)
    {
      if (udpClient == null)
        return;
      var messageFormat = MessageFormat ?? "{message}";
      message = messageFormat.Replace("{message}", message);
      message = string.Format("<{0}>{1} {2} {3} {4}", facility * 8 + (int)logLevel, DateTime.UtcNow.ToString("s"), clientName, sender, message);
      var bytes = Encoding.UTF8.GetBytes(message);
      await udpClient.SendAsync(bytes);
    }
  }
}