using CommandLine;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using Panels.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace Panels
{
    public class Program
    {
        public class Options
        {
            [Option('c', "config", Required = true, HelpText = "Path to the XML file")]
            public string Config { get; set; } = string.Empty;

            [Option('i', "images", Required = true, HelpText = "Path to the folder containing the images")]
            public string Images { get; set; } = string.Empty;

            [Option('o', "output", Default = "Images.pdf", HelpText = "Name of the generated PDF")]
            public string Output { get; set; } = string.Empty;
        }

        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Options))]
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunWithOptions)
                .WithNotParsed(HandleParseError);
        }

        public static string GeneratePdf(Options opts)
        {
            LogWriter logWriter = new LogWriter();
            logWriter.Log("Starting PDF generation...");
            PdfWriter writer = new PdfWriter(@$"{opts.Output}");
            PdfDocument pdfDocument = new PdfDocument(writer);
            pdfDocument.SetDefaultPageSize(PageSize.A4);
            Document document = new Document(pdfDocument);
            Comic comic = new Comic(document, opts.Config, opts.Images);
            comic.Render(logWriter);
            document.Close();
            pdfDocument.Close();
            logWriter.Log("Generated " + opts.Output);
            return logWriter.Contents;
        }

        public static void RunWithOptions(Options opts)
        {
            string contents = GeneratePdf(opts);
            File.WriteAllText(@"./debug-output.txt", contents);
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (Error err in errs)
            {
                Console.WriteLine("Error", err.ToString());
            }
        }
    }
}
