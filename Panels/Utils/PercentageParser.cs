using System;
using System.Globalization;

namespace Panels.Utils
{
	public static class PercentageParser
	{
		public static float ParseRequired(string? value, string propertyName)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return 0f;
			}

			var trimmed = value.Trim();
			if (!trimmed.EndsWith('%'))
			{
				throw new FormatException(
					$"{propertyName} must be specified as a percentage (e.g. \"10%\"), but got \"{value}\"."
				);
			}

			var numberPart = trimmed[..^1].Trim();
			if (!float.TryParse(numberPart, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
			{
				throw new FormatException($"Invalid percentage value for {propertyName}: \"{value}\".");
			}

			return result;
		}

		public static string Format(float value)
		{
			return $"{value.ToString(CultureInfo.InvariantCulture)}%";
		}
	}
}
