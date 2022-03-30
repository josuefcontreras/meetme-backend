using Domain;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();


                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                if (context.Database.IsNpgsql())
                {
                    // DOC LINK: https://www.npgsql.org/doc/types/datetime.html
                    //In PostgreSQL, timestamp with time zone represents a UTC timestamp, 
                    //while timestamp without time zone represents a local or unspecified 
                    //time zone. Starting with 6.0, Npgsql maps UTC DateTime to timestamp 
                    //with time zone, and Local/ Unspecified DateTime to timestamp without 
                    //time zone; trying to send a non - UTC DateTime as timestamptz will throw 
                    //an exception, etc.Npgsql also supports reading and writing DateTime 
                    //to timestamp with time zone, but only with Offset = 0.Prior to 6.0, timestamp 
                    //with time zone would be converted to a local timestamp when read - see below for 
                    //more details. The precise improvements and breaking changes are detailed in the 6.0 
                    //breaking changes; to revert to the pre - 6.0 behavior, add the following at the start 
                    //of your application, before any Npgsql operations are invoked:

                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                }

                context.Database.Migrate();

                await ApplicationDbContextSeed.SeedData(context, userManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occured suring migration.");

                throw;
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


    }

}
