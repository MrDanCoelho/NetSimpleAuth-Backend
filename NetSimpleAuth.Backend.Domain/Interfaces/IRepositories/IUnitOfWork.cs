using System;
using System.Data;

namespace NetSimpleAuth.Backend.Domain.Interfaces.IRepositories
{
    /// <summary>
    /// Database Unit of Work pattern application
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection DbConnection { get; }
        IDbTransaction DbTransaction { get; }
        void Begin();
        void Rollback();
        void Commit();
    }
}