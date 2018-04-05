using CommandLine;
using Sembium.ContentStorage.Replication.Common.Config;
using Sembium.ContentStorage.Replication.Replicator.Main;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.Config
{
    public class CommandLineArgsConfigProvider : ICommandLineArgsConfigProvider
    {
        private readonly IEnumerable<ICommandLineEndpointConfigProvider> _commandLineEndpointConfigProviders;
        private readonly IRouteConfigFactory _routeConfigFactory;
        private readonly IConfigFactory _configFactory;

        private CommandLineOptions _options;

        public CommandLineArgsConfigProvider(
            IEnumerable<ICommandLineEndpointConfigProvider> commandLineEndpointConfigProviders,
            IRouteConfigFactory routeConfigFactory,
            IConfigFactory configFactory)
        {
            _commandLineEndpointConfigProviders = commandLineEndpointConfigProviders;
            _routeConfigFactory = routeConfigFactory;
            _configFactory = configFactory;
        }

        private void InitializeOptions(IEnumerable<string> args)
        {
            _options = new CommandLineOptions();
            using (var parser = new CommandLine.Parser((settings) => { settings.HelpWriter = null; }))
            {
                parser.ParseArguments<CommandLineOptions>(args)
                    .WithParsed<CommandLineOptions>(opts => { _options = opts; })
                    .WithNotParsed(errs => { });
            }
        }

        private IEndpointConfig GetEndpointConfig(string[] configInfo)
        {
            return 
                _commandLineEndpointConfigProviders
                .Where(x => x.CanProvideConfig(configInfo))
                .Select(x => x.GetConfig(configInfo))
                .SingleOrDefault();
        }

        private IEndpointConfig GetSourceConfig()
        {
            return GetEndpointConfig(_options.Source?.ToArray());
        }

        private IEndpointConfig GetDestinationConfig()
        {
            return GetEndpointConfig(_options.Destination?.ToArray());
        }

        private int GetContentCountLimit()
        {
            int result = (Int32.TryParse(_options.MaxContentNumber, out result) ? result : -1);
            return result;
        }

        private int GetConnectionCountLimit()
        {
            int result = (Int32.TryParse(_options.MaxConnectionNumber, out result) ? result : -1);
            return result;
        }

        private bool GetForceAllContents()
        {
            return _options.ForceAllContents;
        }

        private bool GetSkipDestinationCheck()
        {
            return _options.SkipDestinationCheck;
        }

        private bool GetParallelGetLists()
        {
            return _options.ParallelGetLists;
        }

        private string GetLogFileName()
        {
            return _options.LogFileName;
        }

        private DateTimeOffset? GetHashCheckMoment()
        {
            if (string.IsNullOrEmpty(_options.HashCheckMoment))
                return null;

            return DateTimeOffset.ParseExact(_options.HashCheckMoment, "yyyy-MM-dd-HHmm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        public IConfig GetConfig(IEnumerable<string> args)
        {
            InitializeOptions(args);

            var routeConfig = 
                _routeConfigFactory(
                    GetSourceConfig(), 
                    GetDestinationConfig(), 
                    GetContentCountLimit(), 
                    GetConnectionCountLimit(), 
                    GetForceAllContents(),
                    GetSkipDestinationCheck(), 
                    GetParallelGetLists(),
                    GetHashCheckMoment()
                );

            return _configFactory(new[] { routeConfig }, GetLogFileName());
        }
    }
}
