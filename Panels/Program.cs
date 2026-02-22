using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
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
				Description = "Path to the configuration file (XML, JSON, or YAML)",
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
				var configPath = parseResult.GetValue(configOption);
				if (!string.IsNullOrEmpty(configPath))
				{
					ConfigurationParser.Read(configPath);
					Console.WriteLine("Config file is valid");
				}
				else
				{
					Console.WriteLine("No config file specified");
				}
				return 0;
			});
			rootCommand.Subcommands.Add(validateCommand);

			ParseResult parseResult = rootCommand.Parse(args);
			return parseResult.Invoke();
		}

		public static string GeneratePdf(string? configFile, string? imagesFolderPath, string? outputFile)
		{
			if (string.IsNullOrEmpty(configFile))
				throw new ArgumentException("Config file cannot be null or empty", nameof(configFile));
			if (string.IsNullOrEmpty(imagesFolderPath))
				throw new ArgumentException("Images folder path cannot be null or empty", nameof(imagesFolderPath));
			if (string.IsNullOrEmpty(outputFile))
				throw new ArgumentException("Output file cannot be null or empty", nameof(outputFile));

			LogWriter logWriter = new LogWriter();
			logWriter.Log("Starting PDF generation...");
			PdfWriter writer = new PdfWriter(@$"{outputFile}");
			PdfDocument pdfDocument = new PdfDocument(writer);
			pdfDocument.SetDefaultPageSize(PageSize.A4);
			Document document = new Document(pdfDocument);
			Comic comic = ConfigurationParser.Read(configFile);
			comic.Initialize(document, imagesFolderPath);
			comic.Render(logWriter);
			if (comic.ShowPageNumbers) {
				AddPageNumbers(pdfDocument);
			}
			document.Close();
			pdfDocument.Close();
			logWriter.Log("Generated " + outputFile);
			return logWriter.Contents;
		}

		private static void AddPageNumbers(PdfDocument pdfDocument)
		{
			int numberOfPages = pdfDocument.GetNumberOfPages();
			if (numberOfPages == 0) return;
			const float footerHeight = 64f;
			for (int i = 1; i <= numberOfPages; i++)
			{
				var page = pdfDocument.GetPage(i);
				var pageSizeForPage = page.GetPageSize();
				var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDocument);
				var footerRect = new Rectangle(0, 0, pageSizeForPage.GetWidth(), footerHeight);
				var layoutCanvas = new Canvas(canvas, footerRect);
				layoutCanvas.Add(new Paragraph(i.ToString()).SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));
				layoutCanvas.Close();
			}
		}
	}
}