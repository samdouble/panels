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
		[InlineData("Snapshot/test01/bd.xml", "Snapshot/test01/images")]
		public Task Verify_test01(string xmlPath, string imagesPath)
		{
			var parentDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
				?? throw new Exception("Parent directory not found");
			var fullXmlPath = Path.Combine(parentDirectory, xmlPath);
			var fullImagesPath = Path.Combine(parentDirectory, imagesPath);
			var result = Program.GeneratePdf(fullXmlPath, fullImagesPath, @"./output.pdf");
			return Verify(result, Settings);
		}
	}
}