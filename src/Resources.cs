using StonkBot.Settings;
using System.IO;

namespace StonkBot
{
    public static class Resources
    {
        public static void CreateResource(string basePath, string resourceName)
        {
            var path = Path.Combine(basePath, resourceName);
            if (File.Exists(path)) return;

            using var stream = Reflections.GetManifestResourceStream(resourceName);
            using var fileStream = new FileStream(path, FileMode.CreateNew);

            stream.CopyTo(fileStream);
        }
    }
}
