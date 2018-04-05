using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.Common
{
    public static class Utils
    {
        public static string GetExeName()
        {
            var result = System.IO.Path.GetFileName(Assembly.GetEntryAssembly().GetName().CodeBase);
            return System.IO.Path.ChangeExtension(result, System.IO.Path.GetExtension(result).ToLowerInvariant());
        }

        public static string GetAssemblyTitle()
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyAttribute = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute));
            return assemblyAttribute.Title;
        }

        public static string GetAssemblyCompany()
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyAttribute = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute));
            return assemblyAttribute.Company;
        }

        public static bool IsSingleInstance()
        {
            var processName = System.IO.Path.GetFileNameWithoutExtension(GetExeName());
            var processCount =
                Process.GetProcesses()
                .Where(x => !string.IsNullOrEmpty(x.ProcessName) && !x.ProcessName.EndsWith(".vshost"))
                .Where(x => string.Equals(processName, System.IO.Path.GetFileNameWithoutExtension(x.ProcessName)))
                .Count();

            return (processCount <= 1);
        }
    }
}
