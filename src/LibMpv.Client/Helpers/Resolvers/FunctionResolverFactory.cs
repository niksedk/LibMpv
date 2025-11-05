using System.Runtime.InteropServices;

namespace LibMpv.Client;

public enum LibMpvPlatformID 
{
    Win32NT = 1,
    Unix = 2,
    MacOSX = 3,
    Android = 4,
    Other = 10
}

public static class FunctionResolverFactory
{
    public static LibMpvPlatformID GetPlatformId()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return LibMpvPlatformID.MacOSX;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return LibMpvPlatformID.Win32NT;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var isAndroid = RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"));
            return isAndroid  ? LibMpvPlatformID.Android : LibMpvPlatformID.Unix;
        }
        
        return LibMpvPlatformID.Other;
    }

    public static IFunctionResolver Create()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return new MacFunctionResolver();
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsFunctionResolver();
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var isAndroid = RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"));
            return isAndroid ? new AndroidFunctionResolver() : new LinuxFunctionResolver();
        }

        throw new PlatformNotSupportedException();
    }
}
