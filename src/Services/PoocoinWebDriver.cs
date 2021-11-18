using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
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

        public async Task<string> GetGraphFileName(string coinAddress, TimeSpan timeout)
        {
            Directory.CreateDirectory(ImagePath);

            var filePath = Path.Combine(ImagePath, $"{coinAddress}_{Guid.NewGuid():N}.png");
            var url = $"{BaseUrl}{coinAddress}";

            _webDriver.Url = url;
            await Task.Delay(timeout).ConfigureAwait(false);

            var element = _webDriver.FindElement(By.XPath("//*[starts-with(@id, 'tradingview_')]"));
            ((WebElement)element).GetScreenshot().SaveAsFile(filePath);

            return filePath;
        }

        public void Dispose()
        {
            _webDriver.Dispose();
        }
    }
}
