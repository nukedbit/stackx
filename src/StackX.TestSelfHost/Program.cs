using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Funq;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using ServiceStack.Validation;
using StackX.ServiceInterface;
using StackX.ServiceModel;

namespace StackX.TestSelfHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseModularStartup<Startup>()
                .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://localhost:5000/")
                .Build();

            host.Run();
        }
    }

    public class Startup : ModularStartup
    {
        public Startup(IConfiguration configuration) : base(configuration, typeof(StackXAppHostBase).Assembly)
        {
            
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public new void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseServiceStack(new AppHost());

            app.Run(context =>
            {
                context.Response.Redirect("/metadata");
                return Task.FromResult(0);
            });
        }
    }

    public class AppHost : StackXAppHostBase
    {
        public AppHost()
            : base("StackX.TestSelfHost", typeof(StackXAppHostBase).Assembly, typeof(CreateLanguage).Assembly) { }

        public override void Configure(Container container)
        {
            base.Configure(container);
            this.Plugins.Add(new ValidationFeature()); 
            this.Plugins.Add(new LanguagesFeature());
            this.Plugins.Add(new ApplicationsFeature());
            this.Plugins.Add(new TaxonomiesFeature());
            this.Plugins.Add(new TranslationsFeature());
            this.Plugins.Add(new FileFeature());
            this.Plugins.Add(new LogsOnDbFeature());
        }
    }
}