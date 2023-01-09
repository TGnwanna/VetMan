using Logic.IHelpers;
using System.Reflection;

namespace Logic.Helpers
{
    public class AppVersionService : IAppVersionService
    {
        public string Version =>
        Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
