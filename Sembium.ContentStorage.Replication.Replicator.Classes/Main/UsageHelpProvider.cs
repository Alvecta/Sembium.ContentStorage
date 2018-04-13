using CommandLine;
using CommandLine.Text;
using Sembium.ContentStorage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.Main
{
    public class UsageHelpProvider : IUsageHelpProvider
    {
        public string GetUsageHelp()
        {
            using (var parser = new CommandLine.Parser((settings) => { settings.HelpWriter = null; }))
            {
                var args = Environment.GetCommandLineArgs();

                var r = parser.ParseArguments<CommandLineOptions>(args).WithNotParsed(errs => { });

                var help = HelpText.AutoBuild(r, 
                            (ht) => ht,
                            (ex) => ex
                        );

                help.AddPreOptionsLine(" ");
                help.AddPreOptionsLine(string.Format("Usage: {0} --from Source --to Destination [--number number] [--log LogFileName]", ExeUtils.GetExeName()));
                help.AddPreOptionsLine(string.Format("   or: {0} --config ConfigFileName", ExeUtils.GetExeName()));

                help.AddPostOptionsLine("");
                help.AddPostOptionsLine("Endpoint type: dir (directory) | cs (content storage)");
                help.AddPostOptionsLine("Connection info:");
                help.AddPostOptionsLine("  dir     : directory_name");
                help.AddPostOptionsLine("  cs      : container_name token");
                help.AddPostOptionsLine("");

                help.AddPostOptionsLine(string.Format(@"Example: {0} -f cs TestContainer werlewkj32lwkejr324pfwer -t dir D:\Backup\TestContainer", ExeUtils.GetExeName()));
                help.AddPostOptionsLine("");

                return help;
            }
        }
    }
}
