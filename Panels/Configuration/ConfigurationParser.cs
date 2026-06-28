using Panels;
using System;
using System.IO;
using System.Reflection;

namespace Panels.Configuration
{
	public sealed class ConfigurationParser
	{
		public static Comic Read(string? configurationPath)
		{
			if (string.IsNullOrEmpty(configurationPath))
				throw new ArgumentException("Configuration path cannot be null or empty", nameof(configurationPath));

			Console.WriteLine($"Reading config file at {configurationPath}");
			var extension = Path.GetExtension(configurationPath);
			switch (extension)
			{
				case ".json":
					return JsonParser.Read(configurationPath);
				case ".xml":
					return XmlParser.Read(configurationPath);
				case ".yaml":
				case ".yml":
					return YamlParser.Read(configurationPath);
				default:
					throw new Exception($"Unsupported file extension: {extension}");
			}
		}
	}
}