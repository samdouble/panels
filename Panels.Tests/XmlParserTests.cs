using Panels.Configuration;

namespace Panels.Tests;

public class XmlParserTests
{
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
}
