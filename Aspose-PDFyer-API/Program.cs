using Microsoft.AspNetCore;

namespace AsposeTriage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<AStartup>()
                .Build();
        }
    }
}