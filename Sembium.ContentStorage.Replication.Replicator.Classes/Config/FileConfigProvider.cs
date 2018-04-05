using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.Config
{
    public class FileConfigProvider : IFileConfigProvider
    {
        private readonly ICommandLineArgsConfigProvider _commandLineArgsConfigProvider;

        public FileConfigProvider(ICommandLineArgsConfigProvider commandLineArgsConfigProvider)
        {
            _commandLineArgsConfigProvider = commandLineArgsConfigProvider;
        }

        public IConfig GetConfig(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
                throw new System.IO.FileNotFoundException(string.Format("Config file not found: {0}", fileName), fileName);
            
            var lines = System.IO.File.ReadAllLines(fileName).Where(x => (!string.IsNullOrEmpty(x)) && (!x.StartsWith("#")));

            var config = GetNameValueCollection(lines);

            var skipDestinationCheck = GetBoolOption(config, "skipDestinationCheck");
            var parallelGetLists = GetBoolOption(config, "parallel");
            var forceAllContents = GetBoolOption(config, "all");

            var args = 
                SplitArgs(
                    "--from", config["from"], 
                    "--to", config["to"],
                    "--maxContentNumber", config["maxContentNumber"], 
                    "--maxConnectionNumber", config["maxConnectionNumber"],
                    (skipDestinationCheck ? "--skipDestinationCheck" : null),
                    (parallelGetLists ? "--parallel" : null),
                    (forceAllContents ? "--all" : null),
                    "--log", FormatLogFileName(config["log"]));

            if (!string.IsNullOrEmpty(config["hashCheckMoment"]))
            {
                args = args.Concat(SplitArgs("--hashCheckMoment", config["hashCheckMoment"]));
            }

            return _commandLineArgsConfigProvider.GetConfig(args);
        }

        private static bool GetBoolOption(NameValueCollection config, string optionName)
        {
            return 
                new[] { "1", "true", "yes" }
                .Where(x => x.Equals(config[optionName], StringComparison.InvariantCultureIgnoreCase))
                .Any();
        }

        private NameValueCollection GetNameValueCollection(IEnumerable<string> lines)
        {
            var result = new NameValueCollection();

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { '=' }, 2);
                var name = parts[0].Trim().ToLowerInvariant();
                var value = (parts.Count() > 1 ? parts[1].Trim() : null);
                result.Add(name, value);
            }

            return result;
        }

        private string FormatLogFileName(string logFileName)
        {
            if (string.IsNullOrEmpty(logFileName))
                return logFileName;

            var regex = new Regex(@"%([a-zA-Z]+)%");
            var formats =
                regex.Matches(logFileName)
                .Cast<Match>()
                .Select(x => x.Groups.Cast<Group>().Last())
                .Select(x => x.Value)
                .Distinct();

            var now = DateTimeOffset.UtcNow;
            var result = logFileName;

            foreach (var format in formats)
            {
                result = Regex.Replace(result, string.Format("%{0}%", format), now.ToString(format));
            }

            return result;
        }

        private IEnumerable<string> SplitArgs(params string[] args)
        {
            foreach (var arg in args.Where(x => x != null))
            {
                var parts = Regex.Matches(arg, @"[\""].+?[\""]|[^ ]+").Cast<Match>().Select(m => m.Value);
                
                foreach (var part in parts)
                {
                    yield return part.Trim('"').Replace(@"\\", @"\");
                }
            }
        }
    }
}
