using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using Panels.Configuration;
using Panels.Utils;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Panels
{
	public class Program
	{
		static int Main(string[] args)
		{
			Option<string> configOption = new("--config", "-c")
			{
				Description = "Path to the XML file",
				DefaultValueFactory = parseResult => string.Empty
			};
			RootCommand rootCommand = new("Panels: an app to create comics in PDF format from images and XML");

			// Generate
			Option<string> imagesOption = new("--images", "-i")
			{
				Description = "Path to the folder containing the images",
				DefaultValueFactory = parseResult => string.Empty
			};
			Option<string> outputOption = new("--output", "-o")
			{
				Description = "Name of the generated PDF",
				DefaultValueFactory = parseResult => "Images.pdf"
			};
			Command generateCommand = new("generate", "Generate the PDF file.");
			generateCommand.Add(configOption);
			generateCommand.Add(imagesOption);
			generateCommand.Add(outputOption);
			generateCommand.SetAction(parseResult =>
			{
				var contents = GeneratePdf(
					parseResult.GetValue(configOption),
					parseResult.GetValue(imagesOption),
					parseResult.GetValue(outputOption)
				);
				File.WriteAllText(@"./debug-output.txt", contents);
				return 0;
			});
			rootCommand.Subcommands.Add(generateCommand);

			// Validate
			Command validateCommand = new("validate", "Validate the config file.");
			validateCommand.Add(configOption);
			validateCommand.SetAction(parseResult =>
			{
				XmlParser.Read(parseResult.GetValue(configOption));
				Console.WriteLine("Config file is valid");
				return 0;
			});
			rootCommand.Subcommands.Add(validateCommand);

			ParseResult parseResult = rootCommand.Parse(args);
			return parseResult.Invoke();
		}

		public static string GeneratePdf(string configFile, string imagesFolderPath, string outputFile)
		{
			LogWriter logWriter = new LogWriter();
			logWriter.Log("Starting PDF generation...");
			PdfWriter writer = new PdfWriter(@$"{outputFile}");
			PdfDocument pdfDocument = new PdfDocument(writer);
			pdfDocument.SetDefaultPageSize(PageSize.A4);
			Document document = new Document(pdfDocument);
			Comic comic = ConfigurationParser.Read(configFile);
			comic.Initialize(document, imagesFolderPath);
			comic.Render(logWriter);
			document.Close();
			pdfDocument.Close();
			logWriter.Log("Generated " + outputFile);
			return logWriter.Contents;
		}
	}
}