using System.Data.SqlClient;

namespace Bazam.DataNinja
{
    public class DataNinjaTransactionException : DataNinjaException
    {
        public DataNinjaQuery Query { get; private set; }

        public DataNinjaTransactionException(string methodName, SqlException innerEx, DataNinjaQuery query) : base(methodName, innerEx)
        {
            Query = query;
        }
    }
}
