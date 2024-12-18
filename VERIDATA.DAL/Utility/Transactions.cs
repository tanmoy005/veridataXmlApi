using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace VERIDATA.DAL.Utility
{
    public class Transactions
    {
        private Transactions()
        { }

        public static void Run(DbContext dbContext, Func<IDbContextTransaction, bool> act)
        {
            if (dbContext != null && act != null)
            {
                IExecutionStrategy executionStrategy = dbContext.Database.CreateExecutionStrategy();

                executionStrategy.Execute(() =>
                {
                    using IDbContextTransaction ret = dbContext.Database.BeginTransaction();
                    if (ret != null)
                    {
                        try
                        {
                            if (act.Invoke(ret))
                            {
                                ret.Commit();
                            }
                        }
                        catch (Exception)
                        {
                            ret.Rollback();
                            throw new Exception("Error during transaction, rolling back");
                        }
                    }
                    else
                    {
                        throw new Exception("Error while starting transaction");
                    }
                });
            }
        }
    }
}