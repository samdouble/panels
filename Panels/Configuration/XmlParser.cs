using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Panels.Configuration
{
	public sealed class XmlParser
	{
		public static Comic Read(string xmlPath)
		{
			// XSD Validation
			var xsdPath = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory,
				"Assets/schema.xsd"
			);
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
