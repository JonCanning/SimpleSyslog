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
    
Format the message

    Syslog.Info("Hello {0}", "World!");
    
Use the Trace Listener
    
    Trace.Listeners.Add(new SyslogTraceListener());
    Trace.WriteLine("Hello trace!");
    
Use the Console Writer

    Console.SetOut(new SyslogConsoleWriter());
    Console.WriteLine("Hello console!");

SimpleSyslog.Portable
=====================

In this version you need to specify the client and the sender. Also, the API is asynchronous.

    await Syslog.Initialize("host.syslog.com", 12345, "Jons Phone");
    await Syslog.Info("AppStart", "Hello world!");
    
You can still use the generic overload to set the sender:

    await Syslog.Info<AppStart>("Hello world!");
