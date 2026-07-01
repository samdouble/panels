using Panels.Utils;

namespace Panels.Tests;

public class PercentageParserTests
{
	[Theory]
	[InlineData("10%", 10f)]
	[InlineData("15.5%", 15.5f)]
	[InlineData("0%", 0f)]
	[InlineData(" 10% ", 10f)]
	public void ParseRequired_accepts_percentage_values(string value, float expected)
	{
		Assert.Equal(expected, PercentageParser.ParseRequired(value, "maxPaddingLeft"));
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("   ")]
	public void ParseRequired_treats_missing_values_as_zero(string? value)
	{
		Assert.Equal(0f, PercentageParser.ParseRequired(value, "maxPaddingLeft"));
	}

	[Theory]
	[InlineData("10")]
	[InlineData("15")]
	public void ParseRequired_rejects_plain_numbers(string value)
	{
		var exception = Assert.Throws<FormatException>(() =>
			PercentageParser.ParseRequired(value, "maxPaddingLeft")
		);
		Assert.Contains("must be specified as a percentage", exception.Message);
	}
}
