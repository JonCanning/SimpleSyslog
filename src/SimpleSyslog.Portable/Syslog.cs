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
    private static bool useLocalTime;
    public static string MessageFormat { get; set; }

    public static async Task Initialize(string hostName, int port, string clientName, int facility = 16, string sender = null, bool useLocalTime = false)
    {
      Syslog.clientName = clientName;
      fixedSender = sender;
      Syslog.facility = facility;
      Syslog.useLocalTime = useLocalTime;
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

    public static async Task Emergency(string sender, object arg)
    {
      await Emergency(sender, arg.ToString());
    }

    public static async Task Emergency(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Emergency, message, args);
    }

    public static async Task Emergency<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Emergency, message, args);
    }

    public static async Task Alert(string sender, object arg)
    {
      await Alert(sender, arg.ToString());
    }

    public static async Task Alert(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Alert, message, args);
    }

    public static async Task Alert<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Alert, message, args);
    }

    public static async Task Critical(string sender, object arg)
    {
      await Critical(sender, arg.ToString());
    }

    public static async Task Critical(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Critical, message, args);
    }

    public static async Task Critical<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Critical, message, args);
    }

    public static async Task Error(string sender, object arg)
    {
      await Error(sender, arg.ToString());
    }

    public static async Task Error(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Error, message, args);
    }

    public static async Task Error<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Error, message, args);
    }

    public static async Task Warn(string sender, object arg)
    {
      await Warn(sender, arg.ToString());
    }

    public static async Task Warn(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Warn, message, args);
    }

    public static async Task Warn<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Warn, message, args);
    }

    public static async Task Notice(string sender, object arg)
    {
      await Notice(sender, arg.ToString());
    }

    public static async Task Notice(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Notice, message, args);
    }

    public static async Task Notice<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Notice, message, args);
    }

    public static async Task Info(string sender, object arg)
    {
      await Info(sender, arg.ToString());
    }

    public static async Task Info(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Info, message, args);
    }

    public static async Task Info<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Info, message, args);
    }

    public static async Task Debug(string sender, object arg)
    {
      await Debug(sender, arg.ToString());
    }

    public static async Task Debug(string sender, string message, params object[] args)
    {
      await LogWithSender(sender, LogLevel.Debug, message, args);
    }

    public static async Task Debug<T>(string message, params object[] args) where T : class
    {
      await Log<T>(LogLevel.Debug, message, args);
    }

    static async Task Send(LogLevel logLevel, string message, string sender)
    {
      if (udpClient == null)
        return;
      var messageFormat = MessageFormat ?? "{message}";
      message = messageFormat.Replace("{message}", message);
      message =
        $"<{facility*8 + (int) logLevel}>{(useLocalTime ? DateTime.Now.ToString("s") : DateTime.UtcNow.ToString("s"))} {clientName} {sender} {message}";
      var bytes = Encoding.UTF8.GetBytes(message);
      await udpClient.SendAsync(bytes);
    }
  }
}