using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace NetSimpleAuth.Backend.API
{
    /// <summary>
    /// Program startup class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main program method
        /// </summary>
        /// <param name="args">The main program args</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Host builder creation method
        /// </summary>
        /// <param name="args">The program args</param>
        /// <returns>The host builder</returns>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}