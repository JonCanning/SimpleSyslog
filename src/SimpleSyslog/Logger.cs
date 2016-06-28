namespace SimpleSyslog
{
    public class Logger
    {
        readonly string _sender;

        public Logger(string sender)
        {
            _sender = sender;
        }

        internal void SendLog(LogLevel logLevel, string message, params object[] args)
        {
            message = string.Format(message, args);
            Syslog.Send(logLevel, message, _sender);
        }

        public void Emergency(string message, params object[] args)
        {
            SendLog(LogLevel.Emergency, message, args);
        }

        public void Alert(string message, params object[] args)
        {
            SendLog(LogLevel.Alert, message, args);
        }

        public void Critical(string message, params object[] args)
        {
            SendLog(LogLevel.Critical, message, args);
        }

        public void Error(string message, params object[] args)
        {
            SendLog(LogLevel.Error, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            SendLog(LogLevel.Warn, message, args);
        }

        public void Notice(string message, params object[] args)
        {
            SendLog(LogLevel.Notice, message, args);
        }

        public void Info(string message, params object[] args)
        {
            SendLog(LogLevel.Info, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            SendLog(LogLevel.Debug, message, args);
        }
    }
}