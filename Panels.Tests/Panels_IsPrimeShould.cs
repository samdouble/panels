using Xunit;
using Panels;

namespace Panels.Tests.Services
{
    public class Program_IsPrimeShould
    {
        [Fact]
        public void IsPrime_InputIs1_ReturnFalse()
        {
            // Program.Options options = new Program.Options();
            // Program.RunOptions(options);
            bool result = Image.IsPrime(1);

            Assert.False(result, "1 should not be prime");
        }
    }
}