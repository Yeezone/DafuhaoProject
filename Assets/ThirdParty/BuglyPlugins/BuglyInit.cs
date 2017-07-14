// ----------------------------------------
//
//  BuglyInit.cs
//
//  Author:
//       Yeelik, <bugly@tencent.com>
//
//  Copyright (c) 2015 Bugly, Tencent.  All rights reserved.
//
// ----------------------------------------
//
using UnityEngine;
using System.Collections;

public class BuglyInit : MonoBehaviour
{
	/// <summary>
	/// Your Bugly App ID. Every app has a special identifier that allows Bugly to associate error monitoring data with your app.
	/// Your App ID can be found on the "Setting" page of the app you are trying to monitor.
	/// </summary>
	/// <example>A real App ID looks like this: 90000xxxx</example>
	private const string BuglyAppID = "YOUR APP ID GOES HERE";

	void Awake ()
	{
		// Enable the debug log print
		BuglyAgent.ConfigDebugMode (false);
		// Config default channel, version, user 
		BuglyAgent.ConfigDefault (null, null, null, 0);
		// Config auto report log level, default is LogSeverity.LogError, so the LogError, LogException log will auto report
		BuglyAgent.ConfigAutoReportLogLevel (LogSeverity.LogError);
		// Config auto quit the application make sure only the first one c# exception log will be report, please don't set TRUE if you do not known what are you doing.
		BuglyAgent.ConfigAutoQuitApplication (false);
		// If you need register Application.RegisterLogCallback(LogCallback), you can replace it with this method to make sure your function is ok.
		BuglyAgent.RegisterLogCallback (null);

		// Init the bugly sdk and enable the c# exception handler.
		BuglyAgent.InitWithAppId (BuglyAppID);

		// If you has init the sdk in Android or iOS project, please comment last line and uncomment follow method to enable c# exception handler only:
		// BuglyAgent.EnableExceptionHandler ();

		Destroy (this);
	}
}

