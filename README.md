SimpleSyslog
============

    Logger.Initialize("host.syslog.com", 12345);
    Logger.Info("Hello world!");

Customise the message format - e.g. to use with Loggly

    Logger.MessageFormat = "[bdda562d-b644-457e-9876-1fed861e7139@41058 tag=\"Example1\"] {message}";
