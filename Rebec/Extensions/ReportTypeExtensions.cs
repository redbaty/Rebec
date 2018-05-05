using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LazyCache;

namespace Rebec
{
    internal static class ReportTypeExtensions
    {
        public static Stream GetResourceStream(this ReportType templateType)
        {
            var assembly = Globals.Cache.GetOrAdd("ResAssembly", () => Assembly.GetAssembly(typeof(ReportBuilder)));
            var resources = assembly.GetManifestResourceNames();
            var resourceName = resources.FirstOrDefault(i => i.EndsWith(templateType.GetEnumDescription()));

            if (resourceName == null)
            {
                throw new InvalidOperationException(
                    $"Unable to find the enum's file, please report this to the developer. Enum: {templateType}");
            }

            var stream = assembly.GetManifestResourceStream(resourceName);
            return stream;
        }
    }
}