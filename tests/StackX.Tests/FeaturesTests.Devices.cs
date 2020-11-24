using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Data;
using StackX.ServiceModel;
using ServiceStack.OrmLite;
using StackX.ServiceModel.Types;

namespace StackX.Tests
{
    public partial class FeaturesTests
    {
        [Test]
        public void CreateDevice()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Device>();
            var client = CreateAdminAuthClient();

            var response = client.Send(new RegisterDevice()
            {
                DeviceId = "XXXXXXXXXXXXXXXXXX",
                DeviceKindId = 1,
                Description = "XXXXXXX"
            });

            response.DeviceBlocked.Should().BeFalse();
        }

        [Test]
        public void MultipleCallsCreateDevice()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Device>();
            var client = CreateAdminAuthClient();

            var response = client.Send(new RegisterDevice()
            {
                DeviceId = "XXXXXXXXXXXXXXXXXX",
                DeviceKindId = 1,
                Description = "XXXXXXX"
            });

            response = client.Send(new RegisterDevice()
            {
                DeviceId = "XXXXXXXXXXXXXXXXXX2",
                DeviceKindId = 1,
                Description = "XXXXXXX"
            });

            response.DeviceBlocked.Should().BeFalse();
        }

    }
}