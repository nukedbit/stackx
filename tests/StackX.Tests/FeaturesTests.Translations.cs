using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;

namespace StackX.Tests
{
    public partial class FeaturesTests
    {
        [Test]
        public void Anonymous_CantQuery_Translations()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Get(new QueryTranslations());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Anonymous_CantUpdate_Translation()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Send(new UpdateTranslation());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Anonymous_CantCreate_Translation()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Post(new CreateTranslation());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Anonymous_CantDelete_Translation()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Send(new DeleteTranslation());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Create_Missing_Translation()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Translation>();
            var client = CreateAdminAuthClient();

            client.Post(new CreateMissingTranslation()
            {
                Key = "hello",
                ApplicationId = 1,
                LanguageId = 1
            });
        }
    }
}