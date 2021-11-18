using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using StonkBot.Services;

namespace StonkBot
{
    public class Startup : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;

        private ILogService _logService;
        private ConfigurationService _configurationService;
        private DiscordSocketClient _discordSocketClient;
        private CommandService _commandService;

        public Startup()
        {
            _serviceProvider = ConfigureServices();
        }

        private ServiceProvider ConfigureServices()
        {
            _logService = new ConsoleLogService();
            _configurationService = new ConfigurationService();

            _discordSocketClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = _configurationService.ConfigFile.LogSeverity
            });

            _commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = _configurationService.ConfigFile.LogSeverity
            });

            return new ServiceCollection()
                .AddSingleton(_logService)
                .AddSingleton(_configurationService)
                .AddSingleton(_discordSocketClient)
                .AddSingleton(_commandService)
                .AddSingleton<DiscordService>()
                .BuildServiceProvider();
        }

        public async Task RunAsync()
        {
            try
            {
                _discordSocketClient.Log += _logService.LogAsync;
                _commandService.Log += _logService.LogAsync;

                var botToken = _configurationService.ConfigFile.BotToken;
                await _discordSocketClient.LoginAsync(TokenType.Bot, botToken).ConfigureAwait(false);
                await _discordSocketClient.StartAsync().ConfigureAwait(false);

                await RegisterCommands().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

            await Task.Delay(-1).ConfigureAwait(false);
        }

        private async Task RegisterCommands()
        {
            _discordSocketClient.MessageReceived += async (message) =>
            {
                if (!(message is SocketUserMessage userMessage)) return;
                if (userMessage.Source != MessageSource.User) return;

                var argPos = 1;
                if (userMessage.HasCharPrefix('!', ref argPos))
                {
                    var commandContext = new SocketCommandContext(_discordSocketClient, userMessage);
                    await _commandService.ExecuteAsync(commandContext, argPos, _serviceProvider).ConfigureAwait(false);
                }
            };

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider).ConfigureAwait(false);
        }

        public void Dispose()
        {
            if (_serviceProvider == null) return;

            var discordClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();

            discordClient.StopAsync().GetAwaiter().GetResult();
            discordClient.LogoutAsync().GetAwaiter().GetResult();

            _serviceProvider.Dispose();
        }
    }
}