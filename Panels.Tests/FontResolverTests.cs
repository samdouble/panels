using Panels.Utils;

namespace Panels.Tests;

public class FontResolverTests
{
	[Theory]
	[InlineData("Comic Neue")]
	[InlineData("comic neue")]
	[InlineData("Comic Neue Bold")]
	[InlineData("Bangers")]
	[InlineData("Comicsam-Bold")]
	[InlineData("comicsam-bold")]
	[InlineData("Comicsam-Regular")]
	public void Create_resolves_built_in_fonts(string fontName)
	{
		var font = FontResolver.Create(fontName);
		Assert.NotNull(font);
	}

	[Fact]
	public void Create_uses_comic_neue_as_default_font()
	{
		Assert.Equal("Comic Neue Bold", FontResolver.DefaultFontName);
		Assert.NotNull(FontResolver.Create(null));
		Assert.NotNull(FontResolver.Create(FontResolver.DefaultFontName));
	}

	[Fact]
	public void Create_throws_for_unknown_font()
	{
		Assert.Throws<FileNotFoundException>(() => FontResolver.Create("Unknown-Font"));
	}
}
