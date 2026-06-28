using Newtonsoft.Json;
using System;
using System.IO;
using YamlDotNet.Serialization;

namespace Panels.Configuration
{
	public sealed class YamlParser
	{
		public static Comic Read(string yamlPath)
		{
			using var reader = new StreamReader(yamlPath);
			var deserializer = new Deserializer();
			var yamlObject = deserializer.Deserialize(reader) ?? throw new InvalidOperationException("Failed to deserialize YAML");
			var json = JsonConvert.SerializeObject(yamlObject);
			return JsonParser.ReadFromString(json);
		}
	}
}