using API.Middleware;
using API.SignalR;
using Application;
using Infrastructure;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(_config);
            services.AddInfrastructure(_config);
            services.AddAPI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(configurer => configurer.NoReferrer());
            app.UseXXssProtection(configurer => configurer.EnabledWithBlockMode());
            app.UseXfo(configurer => configurer.Deny());
            app.UseCsp(configurer =>
            {
                configurer
                    .BlockAllMixedContent()
                    .StyleSources(configurer => configurer.Self().CustomSources("https://fonts.googleapis.com", "https://fonts.gstatic.com"))
                    .FontSources(configurer => configurer.Self().CustomSources("https://fonts.gstatic.com", "data:"))
                    .FormActions(configurer => configurer.Self())
                    .FrameAncestors(configurer => configurer.Self())
                    .ImageSources(configurer => configurer.Self().CustomSources("blob:", "https://res.cloudinary.com"))
                    .ScriptSources(configurer => configurer.Self().CustomSources("sha256-47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU="));

            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            }
            else
            {
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
                    await next.Invoke();
                });
            }

            app.UseHttpsRedirection();

            app.UseHsts();

            app.UseRouting();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
