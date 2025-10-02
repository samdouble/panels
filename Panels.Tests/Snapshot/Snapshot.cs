using Panels;
using System.Reflection;
using Xunit;

namespace Panels.Tests.Snapshot
{
	public class Snapshot
	{
		private static readonly VerifySettings Settings;

		static Snapshot()
		{
			Settings = new VerifySettings();
			Settings.UseDirectory("snapshots");
		}

		[Theory]
		[InlineData("Snapshot/testjson01/bd.json", "Snapshot/testjson01/images")]
		public Task Verify_testjson01(string configPath, string imagesPath)
		{
			var parentDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
				?? throw new Exception("Parent directory not found");
			var fullXmlPath = Path.Combine(parentDirectory, configPath);
			var fullImagesPath = Path.Combine(parentDirectory, imagesPath);
			var result = Program.GeneratePdf(fullXmlPath, fullImagesPath, @"./output.pdf");
			return Verify(result, Settings);
		}

		[Theory]
		[InlineData("Snapshot/testxml01/bd.xml", "Snapshot/testxml01/images")]
		public Task Verify_testxml01(string configPath, string imagesPath)
		{
			var parentDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
				?? throw new Exception("Parent directory not found");
			var fullXmlPath = Path.Combine(parentDirectory, configPath);
			var fullImagesPath = Path.Combine(parentDirectory, imagesPath);
			var result = Program.GeneratePdf(fullXmlPath, fullImagesPath, @"./output.pdf");
			return Verify(result, Settings);
		}
	}
}