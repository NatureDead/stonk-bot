using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StonkBot.Settings
{
    public static class Reflections
    {
        public static Assembly Assembly => Assembly.GetEntryAssembly();
        public static Process Process => Process.GetCurrentProcess();
        public static AppDomain AppDomain => AppDomain.CurrentDomain;

        public static Stream GetManifestResourceStream(string resourceName)
        {
            var manifestResources = Assembly.GetManifestResourceNames();
            var manifestResource = manifestResources.FirstOrDefault(x => x.EndsWith(resourceName));

            if (manifestResource == null)
                throw new InvalidOperationException($"Manifest resource \"{resourceName}\" not found.");

            return Assembly.GetManifestResourceStream(manifestResource);
        }

        public static string GetBasePath()
        {
            using var processModule = Process.MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
    }
}