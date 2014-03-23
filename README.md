SimpleSyslog
============

    Logger.Initialize("host.syslog.com", 12345);
    Logger.Information("Hello world!");
    
Sender will be the calling type

Customise the message format - e.g. to use with Loggly

    Logger.MessageFormat = "[bdda562d-b644-457e-9876-1fed861e7139@41058 tag=\"Example1\"] {message}"; 
    
Fix the sender and the facility

    Logger.Initialize("host.syslog.com", 12345, "MyAppName", 17);

Set the sender to a different type

    Logger.Information<MyClass>("Hello world!");