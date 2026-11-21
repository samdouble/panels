using Panels;
using System.Runtime.CompilerServices;
using Xunit;

namespace Panels.Tests.Snapshot
{
	public class Snapshot
	{
		[Theory]
		[InlineData("Snapshot/testjson01/bd.json", "Snapshot/testjson01/images")]
		public void Verify_testjson01(string configPath, string imagesPath) =>
			AssertMatchesApprovedLog(configPath, imagesPath);

		[Theory]
		[InlineData("Snapshot/testxml01/bd.xml", "Snapshot/testxml01/images")]
		public void Verify_testxml01(string configPath, string imagesPath) =>
			AssertMatchesApprovedLog(configPath, imagesPath);

		[Theory]
		[InlineData("Snapshot/testxml02/bd.xml", "Snapshot/testxml01/images")]
		public void Verify_testxml02_skipFirstRow(string configPath, string imagesPath) =>
			AssertMatchesApprovedLog(configPath, imagesPath);

		[Theory]
		[InlineData("Snapshot/testyaml01/bd.yaml", "Snapshot/testjson01/images")]
		public void Verify_testyaml01(string configPath, string imagesPath) =>
			AssertMatchesApprovedLog(configPath, imagesPath);

		private static void AssertMatchesApprovedLog(string configPath, string imagesPath, [CallerMemberName] string testName = "")
		{
			var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
				?? throw new InvalidOperationException("Project directory not found");
			var result = Program.GeneratePdf(
				Path.Combine(projectDirectory, configPath),
				Path.Combine(projectDirectory, imagesPath),
				"./output.pdf");
			var approvedPath = Path.Combine(projectDirectory, "Snapshot", "snapshots", $"Snapshot.{testName}.verified.txt");
			var approved = File.ReadAllText(approvedPath);
			Assert.Equal(approved, result);
		}
	}
}