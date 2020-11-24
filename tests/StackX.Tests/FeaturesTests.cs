using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using FluentAssertions;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Validation;

using StackX.ServiceInterface;
using StackX.ServiceModel;

namespace StackX.Tests
{
    public partial class FeaturesTests
    {
        const string BaseUri = "http://localhost:5008/";
        private readonly ServiceStackHost appHost;

        private string JwtUserToken;

        public FeaturesTests()
        {
            appHost = new StackXAppHost()
            {
                ConfigureFn = (host, container) =>
                {
                    host.Plugins.Add(new ValidationFeature());
                    host.Plugins.Add(new ApplicationsFeature());
                    host.Plugins.Add(new LanguagesFeature());
                    host.Plugins.Add(new TaxonomiesFeature());
                    host.Plugins.Add(new TranslationsFeature());
                    host.Plugins.Add(new DevicesFeature());
                    host.Plugins.Add(new LogsOnDbFeature());
                    host.Plugins.Add(new FileFeature());
                    
                    

                    var factory = container.Resolve<IDbConnectionFactory>();
                    using (var connection = factory.OpenDbConnection())
                    {
                        connection.CreateTableIfNotExists<ApiKey>();
                    }

                    var authRepo = container.Resolve<IAuthRepository>();
                    authRepo.InitSchema();
                    var admin = authRepo.CreateUserAuth(new AppUser
                    {
                        Id = 1,
                        UserName = "admin",
                        Email = "admin@email.com",
                        DisplayName = "Admin User",
                        City = "London",
                        Roles = new List<string>
                        {
                                Roles.Admin
                        },

                    }, "p@55wOrd");

                    authRepo.AssignRoles(admin, new[] { Roles.Admin });

                    var member = authRepo.CreateUserAuth(new AppUser
                    {
                        Id = 2,
                        UserName = "member",
                        Email = "member@member.com",
                        DisplayName = "Admin User",
                        City = "London",
                        Roles = new List<string>
                        {
                                Roles.Member
                        }
                    }, "p@55wOrd");

                        //     appHost.AssertPlugin<AuthFeature>().AuthEvents.Add((IAuthEvents)new AppUserAuthEvents());
                    }
            }
                .Init()
                .Start(BaseUri);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        public static IServiceClient CreateClient() => new JsonServiceClient(BaseUri);

        private static IServiceClient CreateAdminAuthClient()
        {
            var authClient = CreateClient();
            var response = authClient.Post(new Authenticate
            {
                provider = CredentialsAuthProvider.Name,
                UserName = "admin@email.com",
                Password = "p@55wOrd",
                //   RememberMe = true,
            });
            //authClient.SetCredentials("admin@email.com","p@55wOrd");
            return authClient;
        }

        private static IServiceClient CreateMemberAuthClient()
        {
            var authClient = CreateClient();
            authClient.Post(new Authenticate
            {
                provider = CredentialsAuthProvider.Name,
                UserName = "member@member.com",
                Password = "p@55wOrd",
                RememberMe = true,
            });
            return authClient;
        }
    }
}