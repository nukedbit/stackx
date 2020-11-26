using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using StackX.Pipeline.Data;

namespace StackX.Pipeline.Tests
{
    public class PipeLinesTests
    {
        [Test]
        public async Task ExecutePipeSuccess()
        {
            var element = new Mock<PipeElement<string>>();
            element
                .Protected()
                .Setup<bool>("CanExecute", ItExpr.IsAny<string>(), ItExpr.IsAny<PipelineState>())
                .Returns(true);
            element
                .Protected()
                .Setup<Task<PipeElementResult>>("OnExecuteAsync", ItExpr.IsAny<string>(), ItExpr.IsAny<PipelineState>())
                .ReturnsAsync(new PipeSuccessResult() {Result = "res"});
            
             var builder = new PipelineBuilder()
                 .Add(element.Object);

             var pipeline = builder.Build<string>();

             var result = await pipeline.RunAsync("test");

             result.Result.Should().Be("res");
        }


        class FailingPipeElement : PipeElement<string>
        {
            protected override Task<PipeElementResult> OnExecuteAsync(string args, PipelineState state)
            {
                throw new Exception();
            }
        }
        
        [Test]
        public async Task ExecutePipeFailShouldReturnError()
        {
            var builder = new PipelineBuilder()
                .Add(new FailingPipeElement());

            var pipeline = builder.Build<string>();

            var result = await pipeline.RunAsync("test");

            result
                .Should()
                .BeOfType<PipeErrorResult>();
        }

        class Person
        {
            [AutoIncrement]
            public int Id { get; set; }
            public string Name { get; set; }
        }
        
        [Test]
        public async Task ExecuteQueryTaskReturnOneResult()
        {
            var factory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);

            var db = await factory.OpenDbConnectionAsync();
            db.CreateTable<Person>();
            db.Save(new Person()
            {
                Name = "Mario"
            });

            db.Save(new Person()
            {
                Name = "Princess"
            });
            
            db.Save(new Person()
            {
                Name = "Luigi"
            });
            
            var pipeline = new PipelineBuilder()
                .Add(
                    DataTaskBuilder.New()
                        .SetConnection(db)
                        .Read<Person, int>()
                        .Query(args => args.Expression.Where(p => p.Id == args.PipeArgs))
                        .AsList()
                        .Build()
                ).Build<int>();

            var result = await pipeline.RunAsync(2);

            result.Should()
                .BeOfType<PipeSuccessResult>();
            result.Result.Should()
                .BeOfType<List<Person>>()
                .Which.Single()
                .Id.Should().Be(2);
        }
        
        [Test]
        public async Task ExecuteQueryTaskOnEmptyResultReturnError()
        {
            var factory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);

            var db = await factory.OpenDbConnectionAsync();
            db.CreateTable<Person>();
            
            var pipeline = new PipelineBuilder()
                .Add(
                    DataTaskBuilder.New()
                        .SetConnection(db)
                        .Read<Person, int>()
                        .Query(args => args.Expression.Where(p => p.Id == args.PipeArgs))
                        .OnEmptyOrNullRaiseError()
                        .AsList()
                        .Build()
                ).Build<int>();

            var result = await pipeline.RunAsync(2);

            result.Should()
                .BeOfType<PipeErrorResult>()
                .Which.ErrorObject.Should().Be("no results found");
        }
        
        
        [Test]
        public async Task ExecuteQueryAsSingleReturnOneResult()
        {
            var factory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);

            var db = await factory.OpenDbConnectionAsync();
            db.CreateTable<Person>();
            db.Save(new Person()
            {
                Name = "Mario"
            });

            db.Save(new Person()
            {
                Name = "Princess"
            });
            
            db.Save(new Person()
            {
                Name = "Luigi"
            });
            
            var pipeline = new PipelineBuilder()
                .Add(
                    DataTaskBuilder.New()
                        .SetConnection(db)
                        .Read<Person, int>()
                        .Query(args => args.Expression.Limit(1))
                        .AsSingle()
                        .Build()
                ).Build<int>();

            var result = await pipeline.RunAsync(2);

            result.Should()
                .BeOfType<PipeSuccessResult>();
            result.Result.Should()
                .BeOfType<Person>();
        }
    }
}