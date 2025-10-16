using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Panels.Configuration
{
	public sealed class XmlParser
	{
		public static Comic Read(string xmlPath)
		{
			// XSD Validation
			var xsdPath = GetSchemaPath();
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.Schemas.Add(null, xsdPath);
			settings.ValidationType = ValidationType.Schema;

			XmlReader reader = XmlReader.Create(@$"{xmlPath}", settings);
			XmlDocument document = new XmlDocument();
			document.Load(reader);
			ValidationEventHandler eventHandler = new ValidationEventHandler(ComicsValidationEventHandler);
			document.Validate(eventHandler);

			// Deserialization
			XmlSerializer serializer = new XmlSerializer(typeof(Comic));
			using (StringReader stringReader = new StringReader(document.OuterXml))
			{
				var result = serializer.Deserialize(stringReader);
				return result as Comic ?? throw new InvalidOperationException("Failed to deserialize XML");
			}
		}

		private static string GetSchemaPath()
		{
			// Try to read from embedded resource first
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "Panels.Assets.schema.xsd";

			using (var stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream != null)
				{
					// Create a temporary file to hold the schema content
					var tempPath = Path.Combine(Path.GetTempPath(), "schema.xsd");
					using (var fileStream = File.Create(tempPath))
					{
						stream.CopyTo(fileStream);
					}
					Console.WriteLine($"Loaded schema.xsd from embedded resource to {tempPath}");
					return tempPath;
				}
			}

			// Fallback to file system search (for development or if resource is not found)
			var possiblePaths = new[]
			{
				// Standard build output location
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "schema.xsd"),
				// Debian package location (primary)
				Path.Combine("/usr", "share", "Panels", "Assets", "schema.xsd"),
				// Alternative Debian package location
				Path.Combine("/usr", "lib", "Panels", "Assets", "schema.xsd"),
				// Assembly location (for embedded resources or side-by-side)
				Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "Assets", "schema.xsd"),
				// Current working directory
				Path.Combine(Directory.GetCurrentDirectory(), "Assets", "schema.xsd"),
				// Development/relative path
				"Assets/schema.xsd"
			};

			// Debug: Print all paths being searched
			Console.WriteLine("Embedded resource not found, searching for schema.xsd file in the following locations:");
			foreach (var path in possiblePaths)
			{
				Console.WriteLine($"  - {path} (exists: {File.Exists(path)})");
			}

			foreach (var path in possiblePaths)
			{
				if (File.Exists(path))
				{
					Console.WriteLine($"Found schema.xsd file at {path}");
					return path;
				}
			}

			throw new FileNotFoundException(
				$"Could not find schema.xsd file. Searched embedded resource '{resourceName}' and file paths: {string.Join(", ", possiblePaths)}"
			);
		}

		static void ComicsValidationEventHandler(object? sender, ValidationEventArgs e)
		{
			if (e.Severity == XmlSeverityType.Warning)
			{
				Console.Write("WARNING: ");
				Console.WriteLine(e.Message);
			}
			else if (e.Severity == XmlSeverityType.Error)
			{
				Console.Write("ERROR: ");
				Console.WriteLine(e.Message);
			}
		}
	}
}