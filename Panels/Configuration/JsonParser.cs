using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Panels.Configuration
{
	public sealed class JsonParser
	{
		private static readonly Dictionary<string, Type> TypeMap = new Dictionary<string, Type>
		{
			{ "comic", typeof(Comic) },
			{ "slot", typeof(Slot) },
			{ "newpage", typeof(NewPage) },
			{ "panel", typeof(Panel) },
			{ "text", typeof(Panels.Elements.Text) },
			{ "description", typeof(Panels.Elements.Description) }
		};

		private static readonly Dictionary<Type, string> ReverseTypeMap = new Dictionary<Type, string>
		{
			{ typeof(Comic), "comic" },
			{ typeof(Slot), "slot" },
			{ typeof(NewPage), "newpage" },
			{ typeof(Panel), "panel" },
			{ typeof(Panels.Elements.Text), "text" },
			{ typeof(Panels.Elements.Description), "description" }
		};

		public static Comic Read(string jsonPath)
		{
			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
				SerializationBinder = new CustomSerializationBinder()
			};
			return JsonConvert.DeserializeObject<Comic>(File.ReadAllText(jsonPath), settings);
		}

		public static void Write(Comic comic, string jsonPath)
		{
			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
				SerializationBinder = new CustomSerializationBinder(),
				Formatting = Formatting.Indented
			};
			File.WriteAllText(jsonPath, JsonConvert.SerializeObject(comic, settings));
		}

		private class CustomSerializationBinder : ISerializationBinder
		{
			public void BindToName(Type serializedType, out string assemblyName, out string typeName)
			{
				assemblyName = null;
				typeName = ReverseTypeMap.TryGetValue(serializedType, out var shortName) ? shortName : serializedType.FullName;
			}

			public Type BindToType(string assemblyName, string typeName)
			{
				return TypeMap.TryGetValue(typeName, out var type) ? type : Type.GetType($"{typeName}, {assemblyName}");
			}
		}
	}
}
