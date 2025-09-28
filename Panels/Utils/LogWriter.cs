using System;
using System.IO;
using System.Reflection;

namespace Panels.Utils
{
	public sealed class LogWriter
	{
		private static readonly object Padlock = new object();
		private string contents = string.Empty;

		public string Contents
		{
			get
			{
				return contents;
			}
		}
		public void Log(string logMessage)
		{
			lock (Padlock)
			{
				contents += logMessage + "\n";
			}
		}
	}
}