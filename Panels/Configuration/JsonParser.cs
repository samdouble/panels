using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Panels.Configuration
{
	public sealed class JsonParser
	{
		public static Comic Read(string jsonPath)
		{
			return JsonConvert.DeserializeObject<Comic>(File.ReadAllText(jsonPath));
		}
	}
}
