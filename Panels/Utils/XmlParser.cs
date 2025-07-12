using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Panels.Utils
{
    public sealed class XmlParser
    {
        public static XmlNode Read(string xmlPath)
        {
            string xsdPath = Path.Combine(
                Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Panels/Resources/schema.xsd"
            );

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xsdPath);
            settings.ValidationType = ValidationType.Schema;

            XmlReader reader = XmlReader.Create(@"" + xmlPath, settings);
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            ValidationEventHandler eventHandler = new ValidationEventHandler(ComicsValidationEventHandler);
            document.Validate(eventHandler);

            return document.DocumentElement;
        }

        static void ComicsValidationEventHandler(object sender, ValidationEventArgs e)
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
