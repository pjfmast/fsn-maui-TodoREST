namespace TodoREST;

public static class Constants
{
    // URL of REST service

    // URL of REST service (Android does not use localhost)
    // Use http cleartext for local deployment. Change to https for production
    public static string LocalhostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
    public static string Scheme = "https"; // or http
    public static string Port = "7047";
    public static string RestUrl = $"{Scheme}://{LocalhostUrl}:{Port}/api/todoitems/{{0}}";
    public static string RegisterUrl = $"{Scheme}://{LocalhostUrl}:{Port}/register";
    public static string LoginUrl = $"{Scheme}://{LocalhostUrl}:{Port}/login";
    public const string SecureStorageAuthenticationKey = "Authentication";
}
