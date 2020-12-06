using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Octo_FireFly.App.Server.Manager.AutoUpdate.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Server.Manager.AutoUpdate
{
    public class AutoUpdater : BackgroundService, IAutoUpdater
    {
        private const string _githubLatestVersionInfoUrl = "https://api.github.com/repos/MichalSy/Octo-FireFly/releases/latest";
        private const string _installedVersionInfo = "_installedVersion.json";

        private readonly ILogger<AutoUpdater> _logger;
        private readonly HttpClient _httpClient;

        public AutoUpdater(ILogger<AutoUpdater> logger, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CheckForUpdateAsync();
        }

        public async Task CheckForUpdateAsync()
        {
            try
            {
                var localVersion = await LoadInstalledVersionAsync();
                var remoteVersion = await _httpClient.GetFromJsonAsync<VersionInfoDTO>(_githubLatestVersionInfoUrl);

                // Store actual remote version
                await File.WriteAllTextAsync(_installedVersionInfo, JsonSerializer.Serialize(remoteVersion, new JsonSerializerOptions
                {
                    WriteIndented = true
                }));

                if (localVersion != null && !localVersion.TagName.Equals(remoteVersion.TagName))
                {
                    var process = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "dotnet",
                            Arguments = "Octo-FireFly.Updater.dll " + remoteVersion.Assets.First().DownloadUrl,
                            WorkingDirectory = Environment.CurrentDirectory,
                            RedirectStandardOutput = false,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };
                    process.Start();

                    Environment.Exit(-1);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error:", ex);
            }
        }

        private async Task<VersionInfoDTO> LoadInstalledVersionAsync()
        {
            if (!File.Exists(_installedVersionInfo))
            {
                return null;
            }

            using var reader = new StreamReader(_installedVersionInfo);
            return await JsonSerializer.DeserializeAsync<VersionInfoDTO>(reader.BaseStream);
        }

        
    }
}
