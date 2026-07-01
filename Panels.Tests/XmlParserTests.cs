using System.Reflection;
using System.Xml.Schema;
using Panels;
using Panels.Configuration;

namespace Panels.Tests;

public class XmlParserTests
{
	private static string ProjectRoot =>
		Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
		?? throw new InvalidOperationException("Project root not found");

	[Fact]
	public void Read_allows_whitespace_inside_empty_elements()
	{
		var xmlPath = Path.Combine(Path.GetTempPath(), $"panels-test-{Guid.NewGuid():N}.xml");
		File.WriteAllText(xmlPath, """
			<?xml version="1.0" encoding="UTF-8"?>
			<comic version="1.0">
				<slot>
					<panel image="0000.png">
						<description text="Hi">
						</description>
					</panel>
				</slot>
				<newpage />
			</comic>
			""");

		try
		{
			var comic = XmlParser.Read(xmlPath);
			Assert.Equal(2, comic.children.Count);
		}
		finally
		{
			File.Delete(xmlPath);
		}
	}

	[Fact]
	public void Read_deserializes_comic_attributes()
	{
		var xmlPath = Path.Combine(Path.GetTempPath(), $"panels-test-{Guid.NewGuid():N}.xml");
		File.WriteAllText(xmlPath, """
			<?xml version="1.0" encoding="UTF-8"?>
			<comic
				version="1.0"
				fontSize="14"
				rowsPerPage="4"
				showPageNumbers="false"
				skipFirstRow="true"
				marginTop="32"
				verticalPanelSpacing="8"
			>
				<slot maxPaddingLeft="10%" maxPaddingRight="15%">
					<panel image="0000.png">
						<text text="Hello" character="Alice" left="5" top="10" width="100" />
					</panel>
				</slot>
			</comic>
			""");

		try
		{
			var comic = XmlParser.Read(xmlPath);

			Assert.Equal(14, comic.FontSize);
			Assert.Equal(4, comic.RowsPerPage);
			Assert.False(comic.ShowPageNumbers);
			Assert.True(comic.SkipFirstRow);
			Assert.Equal(32, comic.MarginTop);
			Assert.Equal(8, comic.VerticalPanelSpacing);
			Assert.Single(comic.children);

			var slot = Assert.IsType<Slot>(comic.children[0]);
			Assert.Equal("10%", slot.MaxLeftPadding);
			Assert.Equal("15%", slot.MaxRightPadding);
		}
		finally
		{
			File.Delete(xmlPath);
		}
	}

	[Fact]
	public void Read_loads_snapshot_config()
	{
		var xmlPath = Path.Combine(ProjectRoot, "Snapshot/testxml01/bd.xml");

		var comic = XmlParser.Read(xmlPath);

		Assert.Equal(3, comic.RowsPerPage);
		Assert.Single(comic.children);
	}

	[Fact]
	public void ComicsValidationEventHandler_logs_warnings_and_errors()
	{
		var handler = typeof(XmlParser).GetMethod(
			"ComicsValidationEventHandler",
			BindingFlags.Static | BindingFlags.NonPublic
		) ?? throw new InvalidOperationException("Validation handler not found");

		var warning = CreateValidationEventArgs(new XmlSchemaException("schema warning"), XmlSeverityType.Warning);
		var error = CreateValidationEventArgs(new XmlSchemaException("schema error"), XmlSeverityType.Error);

		using var console = new StringWriter();
		var originalOut = Console.Out;
		try
		{
			Console.SetOut(console);
			handler.Invoke(null, new object?[] { null, warning });
			handler.Invoke(null, new object?[] { null, error });
		}
		finally
		{
			Console.SetOut(originalOut);
		}

		var output = console.ToString();
		Assert.Contains("WARNING: schema warning", output);
		Assert.Contains("ERROR: schema error", output);
	}

	private static ValidationEventArgs CreateValidationEventArgs(XmlSchemaException exception, XmlSeverityType severity)
	{
		return (ValidationEventArgs)Activator.CreateInstance(
			typeof(ValidationEventArgs),
			BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
			binder: null,
			args: new object[] { exception, severity },
			culture: null
		)!;
	}
}
