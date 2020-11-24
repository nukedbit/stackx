using System;
using System.Collections.Generic;
using System.Linq;
using Funq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.IO;
using ServiceStack.OrmLite;
using ServiceStack.Validation;
using StackX.ServiceInterface;
using StackX.ServiceModel;

namespace StackX.Tests
{
    class StackXAppHost : AppSelfHostBase
    {
        private static string authKey = "cWRl10kvS/Za4moVOIaVsrWwTpXXuZ0mZf/gNLUhDW4=";
        public Action<StackXAppHost,Container> ConfigureFn { get; set; }

        public StackXAppHost() : base(nameof(FeaturesTests),
            typeof(StackXAppHostBase).Assembly)
        {
            Licensing.RegisterLicense(Environment.GetEnvironmentVariable("SERVICESTACK_LICENSE"));
        }
        
        public override void Configure(Container container)
        {
            container.AddSingleton<IAuthRepository>(c =>
                new InMemoryAuthRepository());
            
            var dbFactory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            container.Register<IDbConnectionFactory>(dbFactory);
            
            container.AddSingleton<IAuthRepository>(c =>
                new OrmLiteAuthRepository<AppUser, UserAuthDetails>(c.Resolve<IDbConnectionFactory>())
                {
                    UseDistinctRoleTables = true
                });
            var memoryVirtualFiles = new MemoryVirtualFiles();
            VirtualFiles = memoryVirtualFiles;
            Plugins.Add(new AuthFeature(() => new CustomUserSession(),
                new IAuthProvider[]
                {
                    new BasicAuthProvider(AppSettings),
                    new CredentialsAuthProvider(AppSettings),
                    // new NetCoreIdentityAuthProvider(AppSettings)
                    // {
                    //     // Adapter to enable ServiceStack Auth in MVC
                    //     AdminRoles = {"Admin"}, 
                    // },
                    new JwtAuthProvider(AppSettings)
                    {
                        RequireSecureConnection = false,
                        AuthKeyBase64 = authKey
                    },
                })
            {
                
            });
            Plugins.Add(new AutoQueryFeature()
            {
                
            });
            Plugins.Add(new ValidationFeature()); 
            ConfigureFn?.Invoke(this, container);
        }
    }
}