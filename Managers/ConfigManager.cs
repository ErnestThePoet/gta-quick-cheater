using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using NuGet.Versioning;
using GTAQuickCheater.Entities;

namespace GTAQuickCheater.Managers
{
    internal class ConfigManager
    {
        public record CheatConfig(string version, List<CheatSet> cheatSets);

        private CheatConfig cheatConfig;

        private const string MIN_CONFIG_VERSION = "1.0.0";

        public ConfigManager(string configPath)
        {
            string rawJson = "";
            try
            {
                rawJson = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configPath));
            }
            catch
            {
                AppManager.ErrorExit($"{ResManager.FindString("FailedToLoadText")} {configPath}");
            }

            try
            {
                var deserializeResult = JsonSerializer.Deserialize<CheatConfig>(rawJson);
                if (deserializeResult == null)
                {
                    throw new Exception("JsonSerializer.Deserialize returned null");
                }

                cheatConfig = deserializeResult;
            }
            catch (Exception ex)
            {
                AppManager.ErrorExit(
                    $"{configPath} {ResManager.FindString("DeserializationErrorText")}: {ex.Message}");
            }

            SemanticVersion? configVersion;
            SemanticVersion.TryParse(cheatConfig!.version, out configVersion);

            if(configVersion == null)
            {
                AppManager.ErrorExit("Invalid version field in config file");
            }

            var comparer = new VersionComparer();
            if (comparer.Compare(SemanticVersion.Parse(MIN_CONFIG_VERSION), configVersion) > 0)
            {
                AppManager.ErrorExit(
                    $"Config file version {cheatConfig.version} not supported " +
                    $"(at least {MIN_CONFIG_VERSION} required)");
            }
        }

        public List<CheatSet> GetCheatSets()
        {
            return cheatConfig.cheatSets;
        }
    }
}
