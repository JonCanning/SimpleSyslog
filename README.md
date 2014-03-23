SimpleSyslog
============

    Syslog.Initialize("host.syslog.com", 12345);
    Syslog.Info("Hello world!");
    
Sender will be the calling type

Customise the message format - e.g. to use with Loggly

    Syslog.MessageFormat = "[bdda562d-b644-457e-9876-1fed861e7139@41058 tag=\"Example1\"] {message}"; 
    
Fix the sender and the facility

    Syslog.Initialize("host.syslog.com", 12345, "MyAppName", 17);

Set the sender to a type

    Syslog.Info<MyClass>("Hello world!");
    
Set the sender

    Syslog.Info("Sent from me", "Hello world!");
    
Format the message

    Syslog.Info("Hello {0}", "World!");
