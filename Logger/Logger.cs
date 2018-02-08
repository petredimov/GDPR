using DatabaseNamespace;
using DbServices;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logger
{
	public enum ConsoleOutput
	{
		DISABLED = 0,
		ENABLED = 1
	}

	[Flags]
	public enum LogLevel
	{
		Console = 0x00,
		Debug = 0x01,
		Info = 0x02,
		Warning = 0x04,
		Error = 0x08,
		DisableConsole = 0x10,

		AuditTrail = 0x10,
		Usage = 0x20,
		Alarm = 0x40,
	}


	public class Log
	{
		#region Private members
		public static SortedList<Int64, FileInfo> logFilesSortedList = new System.Collections.Generic.SortedList<Int64, FileInfo>();

		private static ILog Logging = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private static string configurationFilePath;
		private const string defaultDataLogFolder = "C:\\Log";
		private const string defaultClientLogPath = "C:\\Log";
		private const string dateFormat = "{dd/MM/yyyy HH:mm:ss,fff}";

		// Pattern for every log line
		private static string DEBUG_PATTERN_FORMAT = String.Format("%date{0} [%thread] %-5level %location (%file:%line) - %message %exception %newline", dateFormat);
		private static string INFO_PATTERN_FORMAT = String.Format("%date{0} [%thread] %-5level - %message %exception %newline", dateFormat);
		private const string MAX_FILESIZE = "10MB";

		private static DateTime LastLogUsage = DateTime.MinValue;
		private static readonly TimeSpan LogUsageInterval = TimeSpan.FromMinutes(10);

		public static long LogFolderSizeLimit { get; set; }
		public static string LogDataPath { get; set; }

		#endregion

		#region Getters/Setters

		private static IAuditService auditService = null;

		static Log()
		{
			ConfigureFromCode();
			auditService = new AuditService();
		}

		private static string GetDataPath()
		{
			return "C:\\Log";
		}
		public static string SetHeaderFormat()
		{
			Assembly assembly = Assembly.GetEntryAssembly();
			if (assembly != null)
			{
				return string.Empty;
			}
			else
			{
				return String.Format("Version: 1.0 - WebClient MVC platform");
			}
		}

		#endregion

		/// <summary>
		/// Configure log from code with/without logPath
		/// </summary>
		/// <param name="logPath"></param>
		public static void ConfigureFromCode(string logPath = null)
		{
			Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

			PatternLayout patternLayout = new PatternLayout();
			patternLayout.Header = SetHeaderFormat();
			patternLayout.ConversionPattern = INFO_PATTERN_FORMAT;

			patternLayout.ActivateOptions();

			RollingFileAppender roller = new RollingFileAppender();
			roller.AppendToFile = true;

			if (logPath != null)
			{
				if (Directory.Exists(logPath))
				{
					roller.File = String.Format(@"{0}.log", Path.Combine(logPath, SetLogFilename()));
				}
				else
				{
					string path = Path.Combine(GetDataPath(), "Log");
					roller.File = String.Format(@"{0}.log", Path.Combine((GetDataPath() != String.Empty) ? path : defaultDataLogFolder, SetLogFilename()));
				}
			}
			else
			{
				string assemblyName = String.Empty;
				Assembly assembly = Assembly.GetEntryAssembly();
				if (assembly != null)
				{
					assemblyName = assembly.GetName().Name;
				}
				else
				{
					assemblyName = "WebClient";
				}

				string dataLogPath = SetLogPath(assemblyName);

				if (dataLogPath != String.Empty && Directory.Exists(dataLogPath))
				{
					roller.File = String.Format(@"{0}.log", Path.Combine(dataLogPath, SetLogFilename()));
				}
				else
				{
					roller.File = String.Format(@"{0}.log", Path.Combine(defaultDataLogFolder, SetLogFilename()));
				}
			}

			roller.Layout = patternLayout;
			roller.RollingStyle = RollingFileAppender.RollingMode.Composite;
			roller.MaxSizeRollBackups = 5;
			roller.MaximumFileSize = (LogFolderSizeLimit > 0) ? LogFolderSizeLimit.ToString() + "KB" : MAX_FILESIZE;
			roller.StaticLogFileName = true;
			roller.ActivateOptions();
			hierarchy.Root.AddAppender(roller);
			hierarchy.Root.Level = Level.Info;
			hierarchy.Configured = true;
		}

		public static string SetLogPath(string assemblyName)
		{
			return Path.Combine(Environment.CurrentDirectory, assemblyName + "_" + DateTime.Now.ToString("yyyy-mm-yy_HH_mm_ss") + ".txt");
		}

		/// <summary>
		/// Configure log from config.xml file
		/// </summary>
		/// <returns></returns>
		public static bool ConfigureFromConfig()
		{
			bool configured = false;
			bool createFlag = false;

			// Tells the logging system the correct path.
			Assembly a = Assembly.GetEntryAssembly();

			if (a != null && a.Location != null)
			{
				string path = a.Location + ".config";

				if (File.Exists(path))
				{
					log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
					configurationFilePath = path;
					configured = true;
				}
				else
				{
					path = FindConfigInPath(Path.GetDirectoryName(a.Location));
					if (File.Exists(path))
					{
						log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
						configurationFilePath = path;
						configured = true;
					}
				}
			}

			if (!configured)
			{
				// Tells the logging system the correct path.
				a = Assembly.GetExecutingAssembly();

				if (a != null && a.Location != null)
				{
					string path = a.Location + ".config";

					if (File.Exists(path))
					{
						log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
						configurationFilePath = path;
						configured = true;
					}
					else
					{
						path = FindConfigInPath(Path.GetDirectoryName(a.Location));
						if (File.Exists(path))
						{
							log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
							configurationFilePath = path;
							configured = true;
						}
					}
				}
			}

			if (!configured)
			{
				// Tells the logging system the correct path.
				a = Assembly.GetCallingAssembly();

				if (a != null && a.Location != null)
				{
					string path = a.Location + ".config";

					if (File.Exists(path))
					{
						log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
						configurationFilePath = path;
						configured = true;
					}
					else
					{
						path = FindConfigInPath(Path.GetDirectoryName(a.Location));
						if (File.Exists(path))
						{
							log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
							configurationFilePath = path;
							configured = true;
						}
					}
				}
			}

			if (configured)
			{
				log4net.Repository.Hierarchy.Logger logger = Logging.Logger as log4net.Repository.Hierarchy.Logger;

				if (logger != null)
				{
					if (logger.Hierarchy.Root.Appenders != null && logger.Hierarchy.Root.Appenders.Count > 0)
					{
						foreach (log4net.Appender.IAppender appender in logger.Hierarchy.Root.Appenders)
						{
							log4net.Appender.FileAppender fileAppender = appender as log4net.Appender.FileAppender;
						}
						createFlag = true;
					}
					else
					{
						createFlag = false;
					}
				}
			}
			return createFlag;
		}

		private static string FindConfigInPath(string path)
		{
			string[] files = Directory.GetFiles(path);

			if (files != null && files.Length > 0)
			{
				foreach (string file in files)
				{
					if (Path.GetExtension(file).Trim('.').ToLower(CultureInfo.CurrentCulture) == "config")
					{
						return file;
					}
				}
			}

			// Not found.
			return string.Empty;
		}

		public static string SetLogFilename()
		{
			Assembly assembly = Assembly.GetEntryAssembly();
			if (assembly != null)
			{
				return String.Format("{0}{1}", assembly.GetName().Name, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
			}
			else
			{
				return String.Format("{0}{1}", "WebClient", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">string</param>
		/// <param name="level">enum LogLevel</param>
		/// <param name="exception">Exception</param>
		/// <param name="consoleOutput">true or false</param>
		/// <param name="softwareDiagnostic">true or false</param>
		/// <param name="sender"></param>
		public static void Write(object message, LogLevel level = LogLevel.Info, string userId = "", Exception exception = null, ConsoleOutput consoleOutput = ConsoleOutput.ENABLED, bool softwareDiagnostic = false, [CallerMemberName]string sender = "")
		{
			StringBuilder messageBuilder = new StringBuilder();

			try
			{
				messageBuilder.AppendLine(message.ToString());

				// ERROR
				if (level.HasFlag(LogLevel.Error))
				{

					if (exception == null)
						Logging.Error(message);
					else
						Logging.Error(message, exception);
				}
				// WARNING
				else if (level.HasFlag(LogLevel.Warning))
				{
					if (exception == null)
						Logging.Warn(message);
					else
						Logging.Warn(message, exception);
				}
				// INFO
				else if (level.HasFlag(LogLevel.Info))
				{
					if (exception == null)
						Logging.Info(message);
					else
						Logging.Info(message, exception);
				}
				// DEBUG
				else if (level.HasFlag(LogLevel.Debug))
				{
					if (exception == null)
						Logging.Debug(message);
					else
						Logging.Debug(message, exception);
				}

				// Print out message on console 
				if (!level.HasFlag(LogLevel.DisableConsole))
				{
					Console.WriteLine(message);
					if (exception != null)
					{
						Console.WriteLine("!exception - " + exception.Message);
						Console.WriteLine(exception.ToString());
					}
				}
			}
			catch (Exception ex)
			{

			}
			finally
			{
				auditService.Insert(new Audit() { UserId = userId, Action = (int)level, Date = DateTime.Now, Message = messageBuilder.ToString() });
			}
		}

		private static string GetCurrentUsage()
		{
			Int64 ramUsage = 0;
			Double cpuUsage = 0;

			try
			{
				Process[] process = Process.GetProcessesByName(GetAssemblyName());
				if (process != null && process.Length > 0)
				{
					ramUsage = (process[0].PrivateMemorySize64) / 1024 / 1024;
				}

				PerformanceCounter cpuCounter = new PerformanceCounter();
				cpuCounter.CategoryName = "Processor";
				cpuCounter.CounterName = "% Processor Time";
				cpuCounter.InstanceName = "_Total";

				cpuUsage = cpuCounter.NextValue();

			}
			catch (Exception ex)
			{
			}

			return "Ram: " + ramUsage + "MB Cpu: " + cpuUsage + "%";
		}

		private static string GetAssemblyName()
		{
			string name = "SenseApp";

			Assembly assembly = Assembly.GetEntryAssembly();
			if (assembly != null)
			{
				name = assembly.GetName().Name;
			}

			return name;
		}
	}
}
