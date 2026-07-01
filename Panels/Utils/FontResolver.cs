using iText.IO.Font;
using iText.Kernel.Font;
using System;
using System.Collections.Generic;
using System.IO;

namespace Panels.Utils
{
	public static class FontResolver
	{
		public const string DefaultFontName = "Comic Neue Bold";

		private static readonly Dictionary<string, byte[]> BuiltInFonts = new(StringComparer.OrdinalIgnoreCase)
		{
			["Comic Neue"] = Properties.Resources.ComicNeue_Regular,
			["Comic Neue Bold"] = Properties.Resources.ComicNeue_Bold,
			["Bangers"] = Properties.Resources.Bangers_Regular,
			["Comicsam-Bold"] = Properties.Resources.Comicsam_Bold,
			["Comicsam-Regular"] = Properties.Resources.Comicsam_Regular,
		};

		public static PdfFont Create(string? font)
		{
			var fontName = string.IsNullOrWhiteSpace(font) ? DefaultFontName : font.Trim();

			if (BuiltInFonts.TryGetValue(fontName, out var bytes))
			{
				return PdfFontFactory.CreateFont(bytes, PdfEncodings.CP1252);
			}

			if (File.Exists(fontName))
			{
				return PdfFontFactory.CreateFont(fontName, PdfEncodings.CP1252);
			}

			throw new FileNotFoundException(
				$"Font not found: '{fontName}'. Use a built-in font name ({string.Join(", ", BuiltInFonts.Keys)}) or a path to a .ttf file."
			);
		}
	}
}
