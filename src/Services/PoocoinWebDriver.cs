using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using StonkBot.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StonkBot.Services
{
    public class PoocoinWebDriver : IDisposable
    {
        private readonly IWebDriver _webDriver;

        private const string ImagePath = "image/";
        private const string BaseUrl = "https://poocoin.app/tokens/";

        public PoocoinWebDriver(BrowserEngineType type)
        {
            switch (type)
            {
                case BrowserEngineType.Firefox:
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddArgument("--headless");
                    _webDriver = new FirefoxDriver(firefoxOptions);
                    break;

                default: throw new ArgumentException($"The engine '{type}' is not supported.", nameof(type));
            }   
        }

        public async Task LoadAsync(string tokenAddress, TimeSpan timeout)
        {
            _webDriver.Url = $"{BaseUrl}{tokenAddress}";
            await Task.Delay(timeout).ConfigureAwait(false);
        }

        public string GetGraphFileName()
        {
            var element = (WebElement)_webDriver.FindElement(By.XPath("//*[starts-with(@id, 'tradingview_')]"));
            var screenshot = element.GetScreenshot();

            Directory.CreateDirectory(ImagePath);
            var filePath = Path.Combine(ImagePath, $"{Guid.NewGuid():N}.png");
            screenshot.SaveAsFile(filePath);

            return filePath;
        }

        public TokenData GetTokenData()
        {
            var element = _webDriver.FindElement(By.XPath("/html/body/div[1]/div/div[1]/div[2]/div/div[1]/div[2]"));
            var fullText = element.Text;

            var totalSupply = GetTotalSupply(fullText);
            var marketCap = GetMarketCap(fullText);
            var lpHoldings = GetLpHoldings(fullText);

            var tokenData = new TokenData
            {
                TotalSupply = totalSupply,
                MarketCap = marketCap,
                LpHoldings = lpHoldings
            };

            return tokenData;
        }

        private static string GetTotalSupply(string fullText)
        {
            var startText = "Total Supply:\r\n";
            var startIndex = fullText.IndexOf(startText, StringComparison.OrdinalIgnoreCase);
            var endIndex = fullText.IndexOf("Market Cap:", StringComparison.OrdinalIgnoreCase);

            var subString = fullText.Substring(startIndex, endIndex - startIndex).Trim();
            var totalSupply = subString.Replace(startText, "");

            return totalSupply;
        }

        private static string GetMarketCap(string fullText)
        {
            var startText = "Market Cap:";
            var startIndex = fullText.IndexOf(startText, StringComparison.OrdinalIgnoreCase);
            startIndex = fullText.IndexOf("$", startIndex, StringComparison.OrdinalIgnoreCase);
            var endIndex = fullText.IndexOf("Pc v2", StringComparison.OrdinalIgnoreCase);

            var subString = fullText.Substring(startIndex, endIndex - startIndex).Trim();
            var marketCap = subString.Replace(startText, "");

            return marketCap;
        }

        private static string GetLpHoldings(string fullText)
        {
            var startText = "LP Holdings:\r\n";
            var startIndex = fullText.IndexOf(startText, StringComparison.OrdinalIgnoreCase);
            var endIndex = fullText.IndexOf("| Chart", StringComparison.OrdinalIgnoreCase);

            var subString = fullText.Substring(startIndex, endIndex - startIndex).Trim();
            var lpHoldings = subString.Replace(startText, "");
            return lpHoldings;
        }

        public void Dispose()
        {
            _webDriver.Dispose();
        }
    }
}
