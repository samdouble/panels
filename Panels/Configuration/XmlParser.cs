using Newtonsoft.Json;
using Panels;
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

			XmlDocument document = new XmlDocument();
			using (XmlReader reader = XmlReader.Create(xmlPath, settings))
			{
				document.Load(reader);
			}
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
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "Panels.Assets.schema.xsd";

			using (var stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream != null)
				{
					var tempPath = Path.Combine(Path.GetTempPath(), "schema.xsd");
					using (var fileStream = File.Create(tempPath))
					{
						stream.CopyTo(fileStream);
					}
					Console.WriteLine($"Loaded schema.xsd from embedded resource to {tempPath}");
					return tempPath;
				}
			}
			throw new FileNotFoundException("Could not find schema.xsd file.");
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