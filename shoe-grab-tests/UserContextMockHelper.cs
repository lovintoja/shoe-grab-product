﻿using Microsoft.EntityFrameworkCore;
using ShoeGrabCommonModels;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Moq;
using ShoeGrabProductManagement.Contexts;

namespace ShoeGrabProductManagementTests;
internal class UserContextMockHelper
{
    public List<Product> Products { get; set; } = [];

    public Mock<ProductContext> CreateMockContext()
    {
        var options = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        var mockContext = new Mock<ProductContext>(options) { CallBase = true };

        mockContext.Setup(c => c.Products).Returns(MockDbSet(Products));
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        return mockContext;
    }

    private DbSet<T> MockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));

        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        return mockSet.Object;
    }

    private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
        public T Current => _inner.Current;
        public ValueTask DisposeAsync() => new(Task.FromResult(() => _inner.Dispose()));
        public ValueTask<bool> MoveNextAsync() => new(_inner.MoveNext());
    }

    private class TestAsyncQueryProvider<T> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;
        internal TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;
        public IQueryable CreateQuery(Expression expression) => _inner.CreateQuery(expression);
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => _inner.CreateQuery<TElement>(expression);
        public object Execute(Expression expression) => _inner.Execute(expression);
        public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) =>
            (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(typeof(TResult).GetGenericArguments()[0])
                .Invoke(null, new[] { _inner.Execute(expression) })!;
    }
}
