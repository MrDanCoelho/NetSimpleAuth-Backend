using System.Data;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;

namespace NetSimpleAuth.Backend.Infra.Repositories;

public class UnitOfWork(IDbConnection dbConnection) : IUnitOfWork
{
    private IDbTransaction _dbTransaction;

    IDbConnection IUnitOfWork.DbConnection => dbConnection;
    IDbTransaction IUnitOfWork.DbTransaction => _dbTransaction;

    public void Begin()
    {
        if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
            
        if (_dbTransaction != null) return;
            
        _dbTransaction = dbConnection.BeginTransaction();
    }

    public void Dispose()
    {
        _dbTransaction?.Dispose();
        _dbTransaction = null;
    }

    public void Rollback()
    {
        _dbTransaction.Rollback();
    }

    public void Commit()
    {
        if (_dbTransaction == null) return;

        try
        {
            _dbTransaction.Commit();
        }
        catch
        {
            Rollback();
            throw;
        }
        finally
        {
            Dispose();
        }
    }
}