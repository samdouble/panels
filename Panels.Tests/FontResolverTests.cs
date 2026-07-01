using Panels.Utils;

namespace Panels.Tests;

public class FontResolverTests
{
	[Theory]
	[InlineData("Comicsam-Bold")]
	[InlineData("comicsam-bold")]
	[InlineData("Comicsam-Regular")]
	public void Create_resolves_built_in_fonts(string fontName)
	{
		var font = FontResolver.Create(fontName);
		Assert.NotNull(font);
	}

	[Fact]
	public void Create_uses_default_font_when_value_is_missing()
	{
		var font = FontResolver.Create(null);
		Assert.NotNull(font);
	}

	[Fact]
	public void Create_throws_for_unknown_font()
	{
		Assert.Throws<FileNotFoundException>(() => FontResolver.Create("Unknown-Font"));
	}
}
