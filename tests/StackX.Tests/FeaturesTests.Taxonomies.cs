using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using ServiceStack;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;

namespace StackX.Tests
{
    public partial class FeaturesTests
    {
        [Test]
        public void Anonymous_CantQuery_Taxonomies()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Get(new QueryTaxonomies());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Anonymous_CantUpdate_Taxonomy()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Send(new UpdateTaxonomy());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Anonymous_CantCreate_Taxonomy()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Send(new CreateTaxonomy());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
        
        [Test]
        public void Anonymous_CantDelete_Taxonomy()
        {
            var client = CreateClient();
            
            Action queryApps = () =>
                client.Send(new DeleteTaxonomy());
            
            queryApps.Should()
                .Throw<WebServiceException>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }
    }
}