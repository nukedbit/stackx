using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.Messaging;
using ServiceStack.OrmLite;
using StackX.ServiceInterface;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;

namespace StackX.Tests
{
    public partial class FeaturesTests
    {
        [Test]
        public void Can_CreateLogs()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Log>();

            var client = CreateAdminAuthClient();

            var response = client.Post(new CreateLog()
            {
                Content = "error",
                Level = LogLevel.Error,
                Tag =  "Tag1"
            });

            response.Result.Should().NotBeNull();
        }
        
        [TestCase("error")]
        [TestCase("an")]
        [TestCase("erRor")]
        [TestCase("hapPened")]
        public void Query_Content_Contains(string contains)
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Log>();

            var client = CreateAdminAuthClient();

            client.Post(new CreateLog()
            {
                Content = "An error happened",
                Level = LogLevel.Error,
                Tag =  "Tag1"
            });

            var queryResponse = client.Get(new QueryLogs()
            {
                Level = LogLevel.Error,
                ContentContains = contains
            });

            queryResponse.Results.Count.Should().Be(1);
        }
        
        [TestCase("Tag1")]
        [TestCase("Tag2")]
        public void Can_Filter_Logs_ByTag(string tag)
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Log>();

            var client = CreateAdminAuthClient();

            var response = client.SendAll(new[]
            {
                new CreateLog()
                {
                    Content = "error",
                    Level = LogLevel.Error,
                    Tag = tag
                },
                
                new CreateLog()
                {
                    Content = "error",
                    Level = LogLevel.Error,
                    Tag = tag
                },
                
                new CreateLog()
                {
                    Content = "error",
                    Level = LogLevel.Error,
                    Tag = "tagYY"
                }

            });

            var queryResponse = client.Get(new QueryLogs()
            {
                Level = LogLevel.Error,
                Tag = tag
            });

            queryResponse.Results.Count.Should().Be(2);
        }
        
        
        [Test]
        public async Task SoftDeleteOldLog()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Log>();

            var client = CreateAdminAuthClient();

            await Db.SaveAsync(new Log()
            {
                Content = "err",
                CreatedBy = "a",
                ModifiedBy = "a",
                CreatedDate = DateTime.UtcNow.AddDays(-3),
                ModifiedDate = DateTime.UtcNow.AddDays(-3)
            });
            
            await Db.SaveAsync(new Log()
            {
                Content = "err",
                CreatedBy = "a",
                ModifiedBy = "a",
                CreatedDate = DateTime.UtcNow.AddDays(-4),
                ModifiedDate = DateTime.UtcNow.AddDays(-4)
            });
            
            await client.PostAsync(new CreateLog()
            {
                Content = "active",
                Level = LogLevel.Error,
                Tag =  "Tag1"
            });
            
            //var mqServer = appHost.Container.Resolve<IMessageService>();
            await client.SendAsync(new DeleteLogHistory()
            {
                SoftDeleteAfter = TimeSpan.FromDays(1),
                HardDeleteAfter = TimeSpan.FromDays(10)
            });

            var logs = await Db.SelectAsync<Log>();

            logs.Count(l => l.DeletedDate is not null).Should().Be(2);
            Assert.True(logs.Any(l => l.Content == "active" && l.DeletedDate == null));
        }
        
        
        [Test]
        public async Task HardDeleteOldLog()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Log>();

            var client = CreateAdminAuthClient();

            await Db.SaveAsync(new Log()
            {
                Content = "err",
                CreatedBy = "a",
                ModifiedBy = "a",
                CreatedDate = DateTime.UtcNow.AddDays(-2),
                ModifiedDate = DateTime.UtcNow.AddDays(-2),
            });
            
            await Db.SaveAsync(new Log()
            {
                Content = "err2",
                CreatedBy = "a",
                ModifiedBy = "a",
                CreatedDate = DateTime.UtcNow.AddDays(-4),
                ModifiedDate= DateTime.UtcNow.AddDays(-4)
            });
            
            await client.PostAsync(new CreateLog()
            {
                Content = "active",
                Level = LogLevel.Error,
                Tag =  "Tag1"
            });
            
            //var mqServer = appHost.Container.Resolve<IMessageService>();
            await client.SendAsync(new DeleteLogHistory()
            {
                SoftDeleteAfter = TimeSpan.FromDays(1),
                HardDeleteAfter = TimeSpan.FromDays(4)
            });

            var logs = await Db.SelectAsync<Log>();

            logs.Count(l => l.DeletedDate is null).Should().Be(1);
            
            logs.Count(l => l.DeletedDate is not null).Should().Be(1);

            logs.Count.Should().Be(2);
            
            Assert.True(logs.Any(l => l.Content == "active" && l.DeletedDate == null));
        }
        
        
        [Test]
        public async Task ShouldNotSoftDeleteOrHardDeleteLogs()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Log>();

            var client = CreateAdminAuthClient();

            await Db.SaveAsync(new Log()
            {
                Content = "err",
                CreatedBy = "a",
                ModifiedBy = "a",
                CreatedDate = DateTime.UtcNow.AddHours(-12),
                ModifiedDate = DateTime.UtcNow.AddHours(-12),
            });
            
            await Db.SaveAsync(new Log()
           {
               Content = "err2",
               CreatedBy = "a",
               ModifiedBy = "a",
               CreatedDate = DateTime.UtcNow.AddHours(-23),
               ModifiedDate = DateTime.UtcNow.AddHours(-23),
           });
            
            await Db.SaveAsync(new Log()
            {
                Content = "err2",
                CreatedBy = "a",
                ModifiedBy = "a",
                CreatedDate = DateTime.UtcNow.AddHours(-23).AddMinutes(-59),
                ModifiedDate = DateTime.UtcNow.AddHours(-23).AddMinutes(-59),
            });
             
            await client.SendAsync(new DeleteLogHistory()
            {
                SoftDeleteAfter = TimeSpan.FromDays(1),
                HardDeleteAfter = TimeSpan.FromDays(4)
            });

            var logs = await Db.SelectAsync<Log>();

            logs.Count(l => l.DeletedDate is null).Should().Be(3);
        }
        
        [Test]
        public void GreatherThanReturnTwoLog()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Log>();

            var client = CreateAdminAuthClient();
            var resultsFromDate = DateTime.UtcNow.AddDays(-30);
            
            client.Post(new CreateLog()
            {
                Content = "error",
                Level = LogLevel.Error,
                Tag =  "Tag1",
                LogDate = DateTime.UtcNow.AddDays(-35)
            });

            
            client.Post(new CreateLog()
            {
                Content = "error",
                Level = LogLevel.Error,
                Tag =  "Tag1",
                LogDate = DateTime.UtcNow.AddDays(-15)
            });
            
            client.Post(new CreateLog()
            {
                Content = "error",
                Level = LogLevel.Error,
                Tag =  "Tag1",
                LogDate = DateTime.UtcNow
            });

            
            var response = client.Get(new QueryLogs()
            {
                LogDateGreaterThanOrEqualTo = resultsFromDate
            });

            response.Results.Count.Should().Be(2);
        }
        
        
        [Test]
        public void GreatherThanReturnZeroLogs()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Log>();

            var client = CreateAdminAuthClient();
            var resultsFromDate = DateTime.UtcNow.AddDays(-30);
            
            client.Post(new CreateLog()
            {
                Content = "error",
                Level = LogLevel.Error,
                Tag =  "Tag1",
                LogDate = DateTime.UtcNow.AddDays(-35)
            });

            
            client.Post(new CreateLog()
            {
                Content = "error",
                Level = LogLevel.Error,
                Tag =  "Tag1",
                LogDate = DateTime.UtcNow.AddDays(-45)
            });

            
            var response = client.Get(new QueryLogs()
            {
                LogDateGreaterThanOrEqualTo = resultsFromDate
            });

            response.Results.Count.Should().Be(0);
        }
        
        
        [Test]
        public void GreatherThanReturnAll()
        {
            var Db = appHost.GetContainer().Resolve<IDbConnectionFactory>().OpenDbConnection();
            Db.DropAndCreateTable<Log>();

            var client = CreateAdminAuthClient();
            var resultsFromDate = DateTime.UtcNow.AddDays(-30);
            
            client.Post(new CreateLog()
            {
                Content = "error",
                Level = LogLevel.Error,
                Tag =  "Tag1",
                LogDate = DateTime.UtcNow.AddDays(-35)
            });

            
            client.Post(new CreateLog()
            {
                Content = "error",
                Level = LogLevel.Error,
                Tag =  "Tag1",
                LogDate = DateTime.UtcNow.AddDays(-45)
            });
            
            client.Post(new CreateLog()
            {
                Content = "error",
                Level = LogLevel.Error,
                Tag =  "Tag1",
                LogDate = DateTime.UtcNow
            });

            
            var response = client.Get(new QueryLogs());

            response.Results.Count.Should().Be(3);
        }
    }
}