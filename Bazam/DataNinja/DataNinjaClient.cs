using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace Bazam.DataNinja
{
    public class DataNinjaClient
    {
        #region Events
        public event DataNinjaTransactionEventHandler TransactionQueryBegun;
        public event DataNinjaTransactionEventHandler TransactionQueryCompleted;
        #endregion

        #region Constructor
        /// <summary>
        /// Allows you to instantiate a DataNinja that uses Windows Authentication (as opposed to SQL Server Authentication.
        /// </summary>
        /// <param name="serverName">The name of the server to which this DataNinja will connect.</param>
        /// <param name="databaseName">The name of the database to which this DataNinja will connect.</param>
        public DataNinjaClient(string serverName, string databaseName) : this(serverName, databaseName, string.Empty, string.Empty)
        {
            UseWindowsAuthentication = true;
        }

        public DataNinjaClient(string serverName, string databaseName, string userName, string password)
        {
            DatabaseName = databaseName;
            Password = password;
            ServerName = serverName;
            UserName = userName;
            UseWindowsAuthentication = false;
        }
        #endregion

        #region Properties
        // credentials and connection info
        private string DatabaseName { get; set; }
        private string Password { get; set; }
        private string ServerName { get; set; }
        private string UserName { get; set; }

        // windows authentication
        private bool UseWindowsAuthentication { get; set; }
        #endregion

        #region Methods
        public void NonQueryAsync(DataNinjaQuery query)
        {
            NonQueryAsync(query, null);
        }

        public void NonQueryAsync(DataNinjaQuery query, AsyncCallback cb)
        {
            SqlConnection connection = GetConnection(true);
            SqlCommand command = GetCommand(query, connection);

            try {
                DataNinjaAsyncState state = new DataNinjaAsyncState(cb, command);

                connection.Open();
                IAsyncResult ar = command.BeginExecuteNonQuery(EndNonQueryAsync, state);
            }
            catch (Exception ex) {
                throw new DataNinjaException("DataNinja.NonQueryAsync", ex);
            }
        }

        private void EndNonQueryAsync(IAsyncResult result)
        {
            DataNinjaAsyncState state = (DataNinjaAsyncState)result.AsyncState;

            try {
                if (state.Callback != null)
                    state.Callback.Invoke(result);
                state.Command.EndExecuteNonQuery(result);
            }
            catch (Exception ex) {
                throw new DataNinjaException("DataNinja.EndNonQueryAsync", ex);
            }
            finally {
                TrashCommand(state.Command);
                TrashConnection(state.Command.Connection);
            }
        }

        public void NonQuery(DataNinjaQuery query)
        {
            try {
                Execute<int>((command) => { return command.ExecuteNonQuery(); }, query);
            }
            catch (Exception ex) {
                throw new DataNinjaException("DataNinja.NonQuery", ex);
            }
        }

        public void NonQueryTransaction(params DataNinjaQuery[] queries)
        {
            DataNinjaQuery activeQuery = null;
            try {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.MaxValue)) {
                    using (SqlConnection connection = GetConnection()) {
                        connection.Open();

                        foreach (DataNinjaQuery query in queries) {
                            activeQuery = query;
                            if (TransactionQueryBegun != null)
                                TransactionQueryBegun(activeQuery);
                            using (SqlCommand command = GetCommand(query, connection)) {
                                command.ExecuteNonQuery();
                            }
                            if (TransactionQueryCompleted != null)
                                TransactionQueryCompleted(activeQuery);
                        }
                    }

                    scope.Complete();
                }
            }
            catch (SqlException sqlEx) {
                throw new DataNinjaTransactionException("DataNinja.NonQueryTransaction", sqlEx, activeQuery);
            }
            catch (Exception ex) {
                throw new DataNinjaException("DataNinja.NonQueryTransaction", ex);
            }
        }

        public IDataReader GetReader(DataNinjaQuery query)
        {
            try {
                return Execute<IDataReader>((command) => { return command.ExecuteReader(CommandBehavior.CloseConnection); }, query, false);
            }
            catch (Exception ex) {
                throw new DataNinjaException("DataNinja.GetReader", ex);
            }
        }

        public object GetScalar(DataNinjaQuery query)
        {
            try {
                return Execute<object>((command) => { return command.ExecuteScalar(); }, query);
            }
            catch (Exception ex) {
                throw new DataNinjaException("DataNinja.GetScalar", ex);
            }
        }
        #endregion

        #region Internal Utility
        private delegate T DatabaseCommand<T>(SqlCommand command);

        private T Execute<T>(DatabaseCommand<T> command, DataNinjaQuery query)
        {
            return Execute<T>(command, query, true);
        }

        private T Execute<T>(DatabaseCommand<T> command, DataNinjaQuery query, bool closeConnection)
        {
            bool goAgain = false;
            SqlConnection conn = GetConnection();

            try {
                conn.Open();
                using (SqlCommand cmd = GetCommand(query, conn)) {
                    return command(cmd);
                }
            }
            catch (SqlException ex) {
                if (ex.Number == 10053 || ex.Number == 10054) {
                    goAgain = true;
                }
                else {
                    throw new DataNinjaException("DataNinja.Execute", ex);
                }
            }
            finally {
                if (closeConnection) { conn.Close(); }
            }

            if (goAgain) {
                SqlConnection.ClearPool(GetConnection());
                SqlConnection repooledConn = GetConnection();

                try {
                    using (SqlCommand repooledCmd = GetCommand(query, repooledConn)) {
                        repooledConn.Open();
                        command(repooledCmd);
                    }
                }
                catch {
                    throw;
                }
                finally {
                    if (closeConnection) { repooledConn.Close(); }
                }
            }

            return default(T);
        }

        private string GetConnectionString(bool async)
        {
            try {
                if (ServerName == string.Empty)
                    throw new DataNinjaException("A DataNinja can't be used without a server name.");
                if (DatabaseName == string.Empty)
                    throw new DataNinjaException("A DataNinja can't be used without a database name.");
                if (UserName == string.Empty && !UseWindowsAuthentication)
                    throw new DataNinjaException("A DataNinja using SQL Server AUthentication can't be used without a user name.");
                if (Password == string.Empty && !UseWindowsAuthentication)
                    throw new DataNinjaException("A DataNinja using SQL Server Authentication can't be used without a password.");

                StringBuilder builder = new StringBuilder();
                builder.Append("Data Source=");
                builder.Append(ServerName);
                builder.Append(";Initial Catalog=");
                builder.Append(DatabaseName);
                builder.Append(";Uid=");
                builder.Append(UserName);
                builder.Append(";Pwd=");
                builder.Append(Password);
                if (async)
                    builder.Append(";Asynchronous Processing=true");
                if (UseWindowsAuthentication)
                    builder.Append(";Integrated Security=SSPI");

                return builder.ToString();
            }
            catch (DataNinjaException) {
                throw;
            }
            catch (Exception ex) {
                throw new DataNinjaException("DataNinja.GetConnectionString", ex);
            }
        }

        private SqlCommand GetCommand(DataNinjaQuery query, SqlConnection connection)
        {
            try {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = query.CommandText;
                cmd.CommandTimeout = query.CommandTimeout;
                cmd.CommandType = query.CommandType;
                cmd.Parameters.AddRange(query.GetParameters());
                return cmd;
            }
            catch (Exception ex) {
                throw new DataNinjaException("DataNinja.GetCommand", ex);
            }
        }

        private SqlConnection GetConnection()
        {
            return GetConnection(false);
        }

        private SqlConnection GetConnection(bool async)
        {
            return new SqlConnection(GetConnectionString(async));
        }

        private void TrashCommand(SqlCommand command)
        {
            if (command != null) {
                command.Dispose();
            }
        }

        private void TrashConnection(SqlConnection connection)
        {
            if (connection != null) {
                connection.Close();
                connection.Dispose();
            }
        }
        #endregion
    }
}
