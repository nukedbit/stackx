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
using StackX.Flow;
using StackX.Flow.Data;

namespace StackX.Pipeline.Tests
{
    public class PipeLinesTests
    {
        [Test]
        public async Task ExecutePipeSuccess()
        {
            var element = new Mock<FlowElement<string>>();
            element
                .Protected()
                .Setup<bool>("CanExecute", ItExpr.IsAny<string>(), ItExpr.IsAny<FlowState>())
                .Returns(true);
            element
                .Protected()
                .Setup<Task<FlowElementResult>>("OnExecuteAsync", ItExpr.IsAny<string>(), ItExpr.IsAny<FlowState>())
                .ReturnsAsync(new FlowSuccessResult() {Result = "res"});
            
             var builder = new FlowBuilder()
                 .Add(element.Object);

             var pipeline = builder.Build<string>();

             var result = await pipeline.RunAsync("test");

             result.Result.Should().Be("res");
        }


        class FailingFlowElement : FlowElement<string>
        {
            protected override Task<FlowElementResult> OnExecuteAsync(string args, FlowState state)
            {
                throw new Exception();
            }
        }
        
        [Test]
        public async Task ExecutePipeFailShouldReturnError()
        {
            var builder = new FlowBuilder()
                .Add(new FailingFlowElement());

            var pipeline = builder.Build<string>();

            var result = await pipeline.RunAsync("test");

            result
                .Should()
                .BeOfType<FlowErrorResult>();
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
            
            var pipeline = new FlowBuilder()
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
                .BeOfType<FlowSuccessResult>();
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
            
            var pipeline = new FlowBuilder()
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
                .BeOfType<FlowErrorResult>()
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
            
            var pipeline = new FlowBuilder()
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
                .BeOfType<FlowSuccessResult>();
            result.Result.Should()
                .BeOfType<Person>();
        }
    }
}