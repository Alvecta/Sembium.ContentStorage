using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.Main
{
    public class CommandLineOptions
    {
        [Option('f', "from", HelpText = "Source type and connection info", Required = false)]
        public IEnumerable<string> Source { get; set; }

        [Option('t', "to", HelpText = "Destination type and connection info", Required = false)]
        public IEnumerable<string> Destination { get; set; }

        [Option('n', "maxContentNumber", HelpText = "Max number of contents to be replicated. Default 20", Required = false)]
        public string MaxContentNumber { get; set; }

        [Option('o', "maxConnectionNumber", HelpText = "Max number of parallel connections", Required = false)]
        public string MaxConnectionNumber { get; set; }

        [Option('a', "all", HelpText = "Force replication of all contents", Required = false)]
        public bool ForceAllContents { get; set; }

        [Option('s', "skipDestinationCheck", HelpText = "Skip the check for unknown destination contents", Required = false)]
        public bool SkipDestinationCheck { get; set; }

        [Option('l', "log", HelpText = "Log file name", Required = false)]
        public string LogFileName { get; set; }

        [Option('c', "config", HelpText = "Config file name", Required = false)]
        public string ConfigFileName { get; set; }

        [Option('p', "parallel", HelpText = "Gets source and destination content lists in parallel", Required = false)]
        public bool ParallelGetLists { get; set; }

        [Option('h', "hashCheckMoment", HelpText = "Checks the source and destination contents hashes until specified moment instead of performing replication", Required = false)]
        public string HashCheckMoment { get; set; }
    }
}
