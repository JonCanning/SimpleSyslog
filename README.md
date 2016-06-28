SimpleSyslog
============

Initialize Syslog with the host and port

    Syslog.Initialize("host.syslog.com", 12345)

Optionally set facility, sender, useLocalTime, and messageFormat

	Syslog.Initialize("host.syslog.com", 12345, facility: 15, sender: "My App", useLocalTime: true, messageFormat: "This is the syslog {message}")

Log with a sender

	Syslog.Info("GlobalCommunication", "Hello World")

Log with calling type as the sender

	this.Logger().Info("Hello World")

Format the message with arguments

    this.Logger().Info("Hello {0}", "World")