CloudFile Demo ReadMe

CloudFileCSDemo is simple C# application, to demo how to use the EaseFilter Cloud File Sytem SDK. 

You can install the demo application as the Windows service with below command in dos prompt:
Cloudtierdemo.exe -installservice

You can run the demo application as the console app with below command in dos prompt:
CloudFileCSDemo.exe -console

1.	It will generate a test cloud folder "TestCloudFolder" in the application folder, if you want to have more test cloud files, you can put your test files to the "TestSourceFolder".
	        
2.	You can browse the "TestCloudFolder" and read the virtual files there, the virtual file just like the regular physical files in local.

3.	For demo purpose, when you read the cloud file, it will retrieve the file data from the "TestSourceFolder", you can change it to your remote sever, or in the cloud in code.

