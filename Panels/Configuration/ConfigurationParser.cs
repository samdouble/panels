using System;
using System.IO;
using System.Reflection;
using Panels;

namespace Panels.Configuration
{
	public sealed class ConfigurationParser
	{
		public static Comic Read(string configurationPath)
		{
			Console.WriteLine($"Reading config file at {configurationPath}");
			var extension = Path.GetExtension(configurationPath);
			switch (extension)
			{
				case ".json":
					return JsonParser.Read(configurationPath);
				case ".xml":
					return XmlParser.Read(configurationPath);
				default:
					throw new Exception($"Unsupported file extension: {extension}");
			}
		}
	}
}
