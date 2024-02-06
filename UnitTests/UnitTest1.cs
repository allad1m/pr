using System.Globalization;
using System.Windows;
using WpfApp2.Sessions;
using WpfApp2.Utils;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void TokenManager_ReadToken()
        {
            string expectedToken = Guid.NewGuid().ToString();
            TokenManager.Instance.AccessToken = expectedToken;
            string actualToken = TokenManager.Instance.AccessToken;
            Assert.Equal(expectedToken, actualToken);
        }
        [Fact]
        public void TokenManager_WriteToken()
        {
            string expectedToken = Guid.NewGuid().ToString();
            TokenManager.Instance.AccessToken = expectedToken;
            Assert.Equal(expectedToken, TokenManager.Instance.AccessToken);
        }
        [Fact]
        public void BooleanToVisibilityConverter_Convert()
        {
            var converter = new BooleanToVisibilityConverter();
            var actualValue = (Visibility)converter.Convert(
                true,
                typeof(Visibility),
                null,
                CultureInfo.InvariantCulture
                );
            Assert.Equal(Visibility.Visible, actualValue);
        }
        [Fact]
        public void BooleanToIntConverter_Convert()
        {
            var converter = new BooleanToIntConverter();
            var actualValue = (int)converter.Convert(
                true,
                typeof(int),
                null,
                CultureInfo.InvariantCulture
                );
            Assert.Equal(1, actualValue);
        }
        [Fact]
        public void BooleanToStringConverter_Convert()
        {
            var converter = new BooleanToStringConverter();
            var actualValue = (string)converter.Convert(
                true,
                typeof(string),
                null,
                CultureInfo.InvariantCulture
                );
            Assert.Equal("Истина", actualValue);
        }
    }
}