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
            string parentDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
                ?? throw new Exception("Parent directory not found");
            string fullXmlPath = Path.Combine(parentDirectory, xmlPath);
            string fullImagesPath = Path.Combine(parentDirectory, imagesPath);

            Program.Options options = new Program.Options();
            options.Config = fullXmlPath;
            options.Images = fullImagesPath;
            options.Output = @"./output.pdf";
            string result = Program.GeneratePdf(options);
            return Verify(result, Settings);
        }
    }
}
