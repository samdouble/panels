using System.Reflection;
using Newtonsoft.Json;
using Panels;
using Panels.Configuration;
using Panels.Elements;

namespace Panels.Tests;

public class JsonParserTests
{
	private static string ProjectRoot =>
		Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
		?? throw new InvalidOperationException("Project root not found");

	[Fact]
	public void Read_loads_comic_from_file()
	{
		var jsonPath = Path.Combine(ProjectRoot, "Snapshot/testjson01/bd.json");

		var comic = JsonParser.Read(jsonPath);

		Assert.Equal(10, comic.FontSize);
		Assert.Equal(2, comic.children.Count);
		Assert.IsType<Slot>(comic.children[0]);
	}

	[Fact]
	public void ReadFromString_deserializes_typed_children()
	{
		const string json = """
			{
			  "fontSize": 12,
			  "children": [
			    {
			      "$type": "slot",
			      "panels": [
			        {
			          "image": "0000.png",
			          "elements": [
			            { "$type": "text", "text": "Hello" },
			            { "$type": "description", "text": "Alt text" }
			          ]
			        }
			      ]
			    },
			    { "$type": "newpage" }
			  ]
			}
			""";

		var comic = JsonParser.ReadFromString(json);

		Assert.Equal(12, comic.FontSize);
		Assert.Equal(2, comic.children.Count);
		Assert.IsType<Slot>(comic.children[0]);
		Assert.IsType<NewPage>(comic.children[1]);

		var slot = (Slot)comic.children[0];
		Assert.Single(slot.panels);
		Assert.Equal(2, slot.panels[0].elements.Count);
		Assert.IsType<Text>(slot.panels[0].elements[0]);
		Assert.IsType<Description>(slot.panels[0].elements[1]);
	}

	[Fact]
	public void Write_round_trips_comic_through_file()
	{
		var sourcePath = Path.Combine(ProjectRoot, "Snapshot/testjson01/bd.json");
		var outputPath = Path.Combine(Path.GetTempPath(), $"panels-json-{Guid.NewGuid():N}.json");

		try
		{
			var comic = JsonParser.Read(sourcePath);
			JsonParser.Write(comic, outputPath);

			var roundTrip = JsonParser.Read(outputPath);

			Assert.Equal(comic.FontSize, roundTrip.FontSize);
			Assert.Equal(comic.MarginTop, roundTrip.MarginTop);
			Assert.Equal(comic.RowsPerPage, roundTrip.RowsPerPage);
			Assert.Equal(comic.children.Count, roundTrip.children.Count);
		}
		finally
		{
			File.Delete(outputPath);
		}
	}

	[Fact]
	public void ReadFromString_throws_when_json_is_null()
	{
		var exception = Assert.Throws<InvalidOperationException>(() => JsonParser.ReadFromString("null"));
		Assert.Equal("Failed to deserialize JSON", exception.Message);
	}

	[Fact]
	public void ReadFromString_throws_when_type_is_unknown()
	{
		const string json = """
			{
			  "$type": "unknown",
			  "children": []
			}
			""";

		var exception = Assert.Throws<JsonSerializationException>(() => JsonParser.ReadFromString(json));
		Assert.IsType<InvalidOperationException>(exception.InnerException);
		Assert.Contains("Type not found: unknown", exception.InnerException!.Message);
	}

	[Fact]
	public void CustomSerializationBinder_uses_short_names_for_known_types()
	{
		var binder = CreateSerializationBinder();
		InvokeBindToName(binder, typeof(Slot), out var assemblyName, out var typeName);

		Assert.Null(assemblyName);
		Assert.Equal("slot", typeName);
	}

	[Fact]
	public void CustomSerializationBinder_falls_back_to_full_name_for_unknown_types()
	{
		var binder = CreateSerializationBinder();
		InvokeBindToName(binder, typeof(string), out var assemblyName, out var typeName);

		Assert.Null(assemblyName);
		Assert.Equal(typeof(string).FullName, typeName);
	}

	[Fact]
	public void CustomSerializationBinder_resolves_known_type_names()
	{
		var binder = CreateSerializationBinder();
		var bindToType = binder.GetType().GetMethod("BindToType")
			?? throw new InvalidOperationException("BindToType not found");

		var type = bindToType.Invoke(binder, new object?[] { null, "panel" });

		Assert.Equal(typeof(Panel), type);
	}

	[Fact]
	public void CustomSerializationBinder_resolves_assembly_qualified_type_names()
	{
		var binder = CreateSerializationBinder();
		var bindToType = binder.GetType().GetMethod("BindToType")
			?? throw new InvalidOperationException("BindToType not found");

		var type = bindToType.Invoke(binder, new object?[] { typeof(string).Assembly.FullName, typeof(string).FullName });

		Assert.Equal(typeof(string), type);
	}

	private static object CreateSerializationBinder()
	{
		var binderType = typeof(JsonParser).GetNestedType("CustomSerializationBinder", BindingFlags.NonPublic)
			?? throw new InvalidOperationException("CustomSerializationBinder not found");
		return Activator.CreateInstance(binderType)!;
	}

	private static void InvokeBindToName(object binder, Type serializedType, out string? assemblyName, out string? typeName)
	{
		var method = binder.GetType().GetMethod("BindToName")
			?? throw new InvalidOperationException("BindToName not found");
		var parameters = new object?[] { serializedType, null, null };
		method.Invoke(binder, parameters);
		assemblyName = (string?)parameters[1];
		typeName = (string?)parameters[2];
	}
}
