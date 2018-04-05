using CommandLine;
using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Replicator.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.Config
{
    public class CommandLineConfigProvider : IConfigProvider
    {
        private readonly ICommandLineArgsConfigProvider _commandLineArgsConfigProvider;
        private readonly IFileConfigProvider _fileConfigProvider;
        private readonly IMergeConfigProvider _mergeConfigProvider;

        public CommandLineConfigProvider(
            ICommandLineArgsConfigProvider commandLineArgsConfigProvider,
            IFileConfigProvider fileConfigProvider,
            IMergeConfigProvider mergeConfigProvider)
        {
            _commandLineArgsConfigProvider = commandLineArgsConfigProvider;
            _fileConfigProvider = fileConfigProvider;
            _mergeConfigProvider = mergeConfigProvider;
        }

        public IConfig GetConfig()
        {
            var args = Environment.GetCommandLineArgs();

            var options = new CommandLineOptions();
            using (var parser = new CommandLine.Parser((settings) => { settings.HelpWriter = null; }))
            {
                parser.ParseArguments<CommandLineOptions>(args)
                    .WithParsed(opts => { options = opts; })
                    .WithNotParsed(errors => { });
            }

            var configFileConfig = (string.IsNullOrEmpty(options.ConfigFileName) ? null : _fileConfigProvider.GetConfig(options.ConfigFileName));
            var commandLineConfig = _commandLineArgsConfigProvider.GetConfig(args);

            return _mergeConfigProvider.GetConfig(configFileConfig, commandLineConfig);
        }

        public bool CanProvide()
        {
            return (Environment.GetCommandLineArgs() != null) && (Environment.GetCommandLineArgs().Count() > 1);
        }

        private IConfig GetConfigFileConfig(string configFileName)
        {
            return _fileConfigProvider.GetConfig(configFileName);
        }
    }
}
