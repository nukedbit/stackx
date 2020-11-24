using System;
using System.Linq;
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
        public void Application_Admin_Exists()
        {
            var client = CreateAdminAuthClient();
            var response = client.Get(new QueryApplications());

            response.Results
                .Select(a => a.Name)
                .Should()
                .ContainSingle("Admin");
        }

        [Test]
        public void Member_CantAccess_Applications()
        {
            var client = CreateMemberAuthClient();

            Assert.Throws<WebServiceException>(() =>
                client.Get(new QueryApplications()));
        }
        
        
        [Test]
        public void Anonymous_CantQuery_Applications()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Get(new QueryApplications());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Anonymous_CantUpdate_Application()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Send(new UpdateApplication());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
  
        
        [Test]
        public void Anonymous_CantCreate_Application()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Send(new CreateApplication());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Anonymous_CantDelete_Application()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Send(new DeleteApplication());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Application_IsSoftDeleted()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Application>();
            
            var client = CreateAdminAuthClient();

            var response = client.Post(new CreateApplication()
            {
                Name = "prova"
            });
            
            client.Delete(new DeleteApplication()
            {
                Id = response.Id
            });

            var app = Db.SingleById<Application>(response.Id);

            app.DeletedBy.Should().NotBeNullOrEmpty();
            app.DeletedDate.Should().NotBeNull();
        }
    }
}