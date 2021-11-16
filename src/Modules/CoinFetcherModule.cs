using Discord;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace StonkBot.Modules
{
    public class CoinFetcherModule : ModuleBase<SocketCommandContext>
    {
        [Command("price")]
        public async Task GetPriceAsync()
        {
            await ReplyAsync("Fetching data please wait..").ConfigureAwait(false);

            var process = Process.Start(@".\coin-fetcher.exe");
            await process.WaitForExitAsync().ConfigureAwait(false);

            var lines = await FetchData().ConfigureAwait(false);
            await Context.Channel.SendFileAsync("image.png", "").ConfigureAwait(false);
        }

        private Task<StringBuilder> FetchData()
        {

            throw new NotImplementedException();
        }
    }
}
