using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using ServiceStack;
using StackX.ServiceModel;

namespace StackX.Tests
{
    public partial class FeaturesTests
    {
        [Test]
        public void Anonymous_CantQuery_Languages()
        {
            var client = CreateClient();

            Action queryLanguages = () =>
                client.Get(new QueryLanguages());

            queryLanguages.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int) HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Anonymous_CantUpdate_Language()
        {
            var client = CreateClient();
            
            Action updateLanguage = () =>
                client.Send(new UpdateLanguage());
            
            updateLanguage.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Anonymous_CantCreate_Language()
        {
            var client = CreateClient();
            
            Action createLanguage = () =>
                client.Send(new CreateLanguage());
            
            createLanguage.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Admin_CanCreate_Language()
        {
            var client = CreateAdminAuthClient();
            
            Action createLanguage = () =>
                client.Send(new CreateLanguage(){ Name = "n"});

            createLanguage.Should()
                .NotThrow<Exception>();
        }
        
        [Test]
        public void Anonymous_CantDelete_Language()
        {
            var client = CreateClient();
            
            Action deleteLanguage = () =>
                client.Send(new DeleteLanguage());
            
            deleteLanguage.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
    }
}