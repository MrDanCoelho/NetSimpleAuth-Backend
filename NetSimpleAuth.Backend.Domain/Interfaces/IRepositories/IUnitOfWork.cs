using System;
using System.Data;

namespace NetPOC.Backend.Domain.Interfaces.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection DbConnection { get; }
        IDbTransaction DbTransaction { get; }
        void Begin();
        void Rollback();
        void Commit();
    }
}