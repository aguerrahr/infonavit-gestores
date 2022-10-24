1 Install liquibase
2 Configure liquibase in the Environment Variables
3 Enable TCP/PORT
	1. Open the Microsoft Management Console (mmc.exe)
	2. Uses the SQLServerManager<version>.msc file (such as C:\Windows\SysWOW64\SQLServerManager15.msc) 
	3. Click On: SQL Server Network Configuration -> Protocols for MSSQLSERVER -> TCT/IP
	4. Make sure that:
		- the configuration of TCP/IP is Enabled and keep Alive is 50000 (Tab Protocol) 
4 Download the driver jar and the auh dll file (such as mssql-jdbc_auth-9.4.0.x64.dll), put the firts one in your project and he second one aside the file liquibase.bat
5 Create the liquibase.properties file to specify your driver class path, URL, and user authentication information for the database you want to capture.
6 Check out if connection is successful running the command:
	- liquibase.bat status
7 Take a snapshot of your existing database running the command 
  (if the database has tables created before the liquibase project exist):
	- liquibase.bat generateChangeLog
8 Now you can deploy your database change by running the update command like this:
  (remember to add a new folder with the consecutive version)
	- liquibase.bat update